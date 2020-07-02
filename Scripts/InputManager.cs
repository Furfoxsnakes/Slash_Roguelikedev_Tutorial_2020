using Godot;
using GoRogue;
using SlashRoguelikedevTutorial2020.Scripts.Commands;

namespace SlashRoguelikedevTutorial2020.Scripts
{
    public class InputManager : Node
    {
        public override void _UnhandledInput(InputEvent @event)
        {
            var player = GameController.Instance.Player;
            var map = GameController.DungeonMap;

            var dir = Direction.NONE;
            // player movement
            if (Input.IsActionPressed("MoveNorth"))
                dir = Direction.UP;
            else if (Input.IsActionPressed("MoveSouth"))
                dir = Direction.DOWN;
            else if (Input.IsActionPressed("MoveEast"))
                dir = Direction.RIGHT;
            else if (Input.IsActionPressed("MoveWest"))
                dir = Direction.LEFT;
            else if (Input.IsActionPressed("MoveNorthEast"))
                dir = Direction.UP_RIGHT;
            else if (Input.IsActionPressed("MoveSouthEast"))
                dir = Direction.DOWN_RIGHT;
            else if (Input.IsActionPressed("MoveSouthWest"))
                dir = Direction.DOWN_LEFT;
            else if (Input.IsActionPressed("MoveNorthWest"))
                dir = Direction.UP_LEFT;

            if (dir == Direction.NONE) return;

            // don't do anything if the character is still animating
            if (player.Tween.IsActive()) return;

            var newPos = player.Position + dir;
            
            var moveCommand = new MoveCommand(player, dir);
            if (moveCommand.Execute()) return;
            
            BumpCommand bumpCommand;
            var defender = map.Map.GetEntity<Character>(newPos);
            bumpCommand = defender != null ? new AttackCommand(player, defender) : new BumpCommand(player, map[newPos]);
            bumpCommand.Execute();
        }
    }
}