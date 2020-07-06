using System.Collections.Generic;
using Godot;
using GoRogue;
using GoRogue.GameFramework;
using GoRogue.MapGeneration;
using GoRogue.MapViews;
using SlashRoguelikedevTutorial2020.Characters;
using SlashRoguelikedevTutorial2020.Scripts;
using Vector2 = Godot.Vector2;

public class DungeonMap : TileMap
{
    public Vector2 this[int x, int y] => MapToWorld(new Vector2(x, y));
    public Vector2 this[Coord c] => this[c.X, c.Y];
    public Vector2 this[Vector2 v] => this[(int)v.x, (int)v.y];
    
    public Map Map
    {
        get;
        private set;
    }
    
    private TileMap _fog;
    private int _numMonsters = 20;

    public List<Monster> Monsters { get; private set; }
    
    public IReadOnlyFOV FOV => Map.FOV;

    public override void _Ready()
    {
        _fog = GetNode<TileMap>("Fog");
        Monsters = new List<Monster>();
    }
    
    public void GenerateMap()
    {
        var tempMap = new ArrayMap<bool>(50, 50);
        //QuickGenerators.GenerateRectangleMap(tempMap);
        QuickGenerators.GenerateRandomRoomsMap(tempMap, 20, 5, 12);
        Map = new Map(50, 50, 1, Distance.CHEBYSHEV);
        
        Map.ObjectMoved += OnObjectMoved;

        foreach (var position in tempMap.Positions())
        {
            if (tempMap[position])
                Map.SetTerrain(new FloorTerrain(position));
            else
                Map.SetTerrain(new WallTerrain(position));
        }

        // instance a player
        var playerInstance = GD.Load<PackedScene>("res://Characters/Player/Player.tscn").Instance() as Player;
        GetTree().Root.GetNode("Game").AddChild(playerInstance);
        playerInstance.Position = Map.WalkabilityView.RandomPosition(true);
        playerInstance.Moved += OnPlayerMoved;
        GameController.Instance.Player = playerInstance;
        AddCharacter(playerInstance);
        
        // generate monsters
        for (var i = 0; i < _numMonsters; ++i)
        {
            var skeleman = GD.Load<PackedScene>("res://Characters/Monsters/Skeleman.tscn").Instance() as Monster;
            GetTree().Root.GetNode("Game").AddChild(skeleman);
            skeleman.Position = Map.WalkabilityView.RandomPosition(true);
            AddCharacter(skeleman);
            Monsters.Add(skeleman);
        }
        
        Map.CalculateFOV(playerInstance.Position, playerInstance.FOVRadius, Radius.DIAMOND);
        Draw();
    }

    private void OnPlayerMoved(object sender, ItemMovedEventArgs<IGameObject> e)
    {
        var player = e.Item as Player;
        Map.CalculateFOV(e.NewPosition, player.FOVRadius, Radius.DIAMOND);
        Draw();
    }

    private void OnObjectMoved(object sender, ItemMovedEventArgs<IGameObject> e)
    {
        if (!(e.Item is Character character)) return;
        
        var fromPos = MapToWorld(new Vector2(e.OldPosition.X, e.OldPosition.Y));
        var toPos = MapToWorld(new Vector2(e.NewPosition.X, e.NewPosition.Y));
        character.TweenToPosition(fromPos, toPos);
        character.PlayMovementSound();
    }

    public void Draw()
    {
        foreach (var position in Map.Positions())
        {
            if (!Map.Explored[position])
                continue;
            
            var vectorPos = new Vector2(position.X, position.Y);

            if (Map.FOV[position] == 0)
                _fog.SetCellv(vectorPos, 0);
            else
                _fog.SetCellv(vectorPos, -1);

            var terrain = Map.GetTerrain<Terrain>(position);
            
            if (terrain.IsWalkable)
                SetCellv(vectorPos, 1);
            else
                SetCellv(vectorPos, 0);
        }
    }

    public void AddCharacter(Character character)
    {
        Map.AddEntity(character);
    }

    public void RemoveCharacter(Character character)
    {
        if (Map.Entities.Contains(character))
            Map.RemoveEntity(character);
    }

    public IGameObject GetObjectAtPos(Coord pos)
    {
        return Map.GetObject(pos);
    }

    public Character GetCharacterAtPos(Coord pos)
    {
        return Map.GetEntity<Character>(pos);
    }
}
