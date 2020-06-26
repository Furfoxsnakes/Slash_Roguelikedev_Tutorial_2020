using Godot;
using GoRogue;
using GoRogue.GameFramework;
using GoRogue.MapGeneration;
using GoRogue.MapViews;
using SlashRoguelikedevTutorial2020.Characters;
using SlashRoguelikedevTutorial2020.Scripts;

public class DungeonMap : TileMap
{
    private Map _map;
    private TileMap _fog;
    private int _numMonsters = 20;

    public IReadOnlyFOV FOV => _map.FOV;

    public override void _Ready()
    {
        _fog = GetNode<TileMap>("Fog");
    }

    public void GenerateMap()
    {
        var tempMap = new ArrayMap<bool>(50, 50);
        //QuickGenerators.GenerateRectangleMap(tempMap);
        QuickGenerators.GenerateRandomRoomsMap(tempMap, 20, 5, 12);
        _map = new Map(50, 50, 1, Distance.CHEBYSHEV);
        
        _map.ObjectMoved += OnObjectMoved;

        foreach (var position in tempMap.Positions())
        {
            _map.SetTerrain(tempMap[position]
                ? new Terrain(position, true, true)        // floor
                : new Terrain(position, false, false));    // wall
        }

        // instance a player
        var playerInstance = GD.Load<PackedScene>("res://Characters/Player/Player.tscn").Instance() as Player;
        GetTree().Root.GetNode("Game").AddChild(playerInstance);
        playerInstance.Position = _map.RandomPosition();
        playerInstance.Moved += OnPlayerMoved;
        GameController.Instance.Player = playerInstance;
        AddCharacter(playerInstance);
        
        // generate monsters
        for (var i = 0; i < _numMonsters; ++i)
        {
            var skeleman = GD.Load<PackedScene>("res://Characters/Monsters/Skeleman.tscn").Instance() as Character;
            GetTree().Root.GetNode("Game").AddChild(skeleman);
            skeleman.Position = _map.RandomPosition();
            AddCharacter(skeleman);
        }
        
        _map.CalculateFOV(playerInstance.Position, playerInstance.FOVRadius, Radius.DIAMOND);
        Draw();
    }

    private void OnPlayerMoved(object sender, ItemMovedEventArgs<IGameObject> e)
    {
        var player = e.Item as Player;
        _map.CalculateFOV(e.NewPosition, player.FOVRadius, Radius.DIAMOND);
        Draw();
    }

    private void OnObjectMoved(object sender, ItemMovedEventArgs<IGameObject> e)
    {
        if (!(e.Item is Character character)) return;
        
        var fromPos = MapToWorld(new Vector2(e.OldPosition.X, e.OldPosition.Y));
        var toPos = MapToWorld(new Vector2(e.NewPosition.X, e.NewPosition.Y));
        character.TweenToPosition(fromPos, toPos);
    }

    public void Draw()
    {
        foreach (var position in _map.Positions())
        {
            if (!_map.Explored[position])
                continue;
            
            var vectorPos = new Vector2(position.X, position.Y);

            if (_map.FOV[position] == 0)
                _fog.SetCellv(vectorPos, 0);
            else
                _fog.SetCellv(vectorPos, -1);

            var terrain = _map.GetTerrain<Terrain>(position);
            
            if (terrain.IsWalkable)
                SetCellv(vectorPos, 1);
            else
                SetCellv(vectorPos, 0);
        }
    }

    public void AddCharacter(Character character)
    {
        _map.AddEntity(character);
    }

    public void RemoveCharacter(Character character)
    {
        if (_map.Entities.Contains(character))
            _map.RemoveEntity(character);
    }

    public IGameObject GetObjectAtPos(Coord pos)
    {
        return _map.GetObject(pos);
    }
}
