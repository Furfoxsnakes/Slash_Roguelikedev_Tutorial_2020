using Godot;

namespace SlashRoguelikedevTutorial2020.Scripts
{
    public class Globals : Node
    {
        public Node GameNode => GetTree().Root.GetNode("Game");
    }
}