using Godot;
using System;
using System.Linq;
using GoRogue;
using SlashRoguelikedevTutorial2020.Characters;
using SlashRoguelikedevTutorial2020.Scripts;
using SlashRoguelikedevTutorial2020.Scripts.Extensions;

public class GameController : Node
{
    public static GameController Instance => _instance;
    private static GameController _instance;

    public Node Globals;
    
    public Navigation2D Nav;
    public Line2D Line;
    public Player Player;

    public static DungeonMap DungeonMap;
    public static AudioStreamPlayer Audio;
    
    public override void _Ready()
    {
        _instance = new GameController();
        Audio = GetNode<AudioStreamPlayer>("Audio");
        DungeonMap = GetNode<DungeonMap>("Nav/TempMap");
        DungeonMap.GenerateMap();
    }
}
