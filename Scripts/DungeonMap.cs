using Godot;
using System;
using GoRogue;
using GoRogue.GameFramework;
using GoRogue.MapGeneration;
using GoRogue.MapViews;
using SlashRoguelikedevTutorial2020.Scripts;

public class DungeonMap : TileMap
{
    private Map _map;

    public override void _Ready()
    {
        
        GenerateMap();
    }

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

        foreach (var position in tempMap.Positions())
        {
            if (tempMap[position]) // floor
            {
                _map.SetTerrain(new Terrain(position, true, true));
            }
            else // wall
                _map.SetTerrain(new Terrain(position, false, false));
        }
    }
}
