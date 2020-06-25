using Godot;
using System;
using System.Linq;
using SlashRoguelikedevTutorial2020.Characters;
using SlashRoguelikedevTutorial2020.Scripts;

public class GameController : Node
{
    public static GameController Instance => _instance;
    private static GameController _instance;

    public Node Globals;
    
    public DungeonMap DungeonMap;
    public Navigation2D Nav;
    public Line2D Line;
    public Player Player;

    public override void _EnterTree()
    {
        _instance = new GameController();
    }

    public override void _Ready()
    {
        DungeonMap = GetNode<DungeonMap>("Nav/TempMap");
        DungeonMap.GenerateMap();
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (!(@event is InputEventMouse mouseEvent)) return;

        if (mouseEvent.ButtonMask != 1) return;

        // TODO: move player to position
    }
}
