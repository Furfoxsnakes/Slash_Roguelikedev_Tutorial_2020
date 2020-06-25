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

    private void OnObjectAdded(object sender, ItemEventArgs<IGameObject> e)
    {
        if (e.Item.Layer != 0) return;

        var vectorPos = new Vector2(e.Position.X, e.Position.Y);

        if (e.Item.IsWalkable) // floor
            SetCellv(vectorPos, 1);
        else
            SetCellv(vectorPos, 0);
    }

    public void GenerateMap()
    {
        var tempMap = new ArrayMap<bool>(50, 50);
        QuickGenerators.GenerateRectangleMap(tempMap);
        _map = new Map(50, 50, 1, Distance.CHEBYSHEV);
        
        _map.ObjectAdded += OnObjectAdded;
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
        playerInstance.Position = new Coord(1, 1);
        GameController.Instance.Player = playerInstance;
        AddCharacter(playerInstance);
        
        // instance a skeleman
        var skeleman = GD.Load<PackedScene>("res://Characters/Monsters/Skeleman.tscn").Instance() as Character;
        GetTree().Root.GetNode("Game").AddChild(skeleman);
        skeleman.Position = new Coord(2, 2);
        AddCharacter(skeleman);
    }

    private void OnObjectMoved(object sender, ItemMovedEventArgs<IGameObject> e)
    {
        if (e.Item is Character character)
            character.GlobalPosition = MapToWorld(new Vector2(e.NewPosition.X, e.NewPosition.Y));
    }

    public void AddCharacter(Character character)
    {
        _map.AddEntity(character);
    }
}
