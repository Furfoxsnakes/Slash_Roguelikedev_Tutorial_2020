using Godot;
using GoRogue;

namespace SlashRoguelikedevTutorial2020.Scripts.Commands
{
    public class BumpCommand : CommandBase
    {
        protected Character Character;
        protected Vector2 ToVector;

        public BumpCommand(Character character, Vector2 toVector)
        {
            Character = character;
            ToVector = toVector;
        }

        public BumpCommand(Character character, Coord c)
        {
            Character = character;
            ToVector = new Vector2(c.X, c.Y);
        }

        public override void Execute()
        {
            Finish(CommandResult.Failure);
            
            var startingPos = Character.GlobalPosition;
            Character.Tween.InterpolateProperty(Character, "global_position", startingPos, ToVector, 0.1f);
            Character.Tween.InterpolateProperty(Character, "global_position", ToVector, startingPos, 0.1f);
            Character.Tween.Start();
            //return true;
        }
    }
}