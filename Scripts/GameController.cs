using Godot;
using System;
using System.Linq;
using SlashRoguelikedevTutorial2020.Characters;

public class GameController : Node
{
    public static GameController Instance => _instance;
    private static GameController _instance = new GameController();

    public TileMap TileMap;
    public Navigation2D Nav;
    public Line2D Line;
    public Player Player;

    public override void _Ready()
    {
        TileMap = GetNode<TileMap>("Nav/TempMap");
        Nav = GetNode<Navigation2D>("Nav");
        Line = GetNode<Line2D>("Line2D");
        Player = GetNode<Player>("Player");
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (!(@event is InputEventMouse mouseEvent)) return;

        if (mouseEvent.ButtonMask != 1) return;

        // TODO: move player to position
    }
}
