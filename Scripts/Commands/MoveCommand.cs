using Godot;
using GoRogue;
using GoRogue.GameFramework;
using SlashRoguelikedevTutorial2020.Characters;

namespace SlashRoguelikedevTutorial2020.Scripts.Commands
{
    public class MoveCommand : CommandBase
    {
        private Character _character;
        private Direction _dir;

        public MoveCommand(Character character, Direction direction)
        {
            _character = character;
            _dir = direction;
        }

        public override async void Execute()
        {
            var currentPos = _character.Position;
            var targetPos = _character.Position + _dir;
            
            // attempt to move character
            if (_character.MoveIn(_dir))
            {
                // tween the character to the new position
                _character.TweenToPosition(GameController.DungeonMap[currentPos], GameController.DungeonMap[targetPos], 0.1f);
                // wait for tween to finish and then flag this command as completed
                await _character.Tween.ToSignal(_character.Tween, "tween_completed");
                Finish(CommandResult.Success);
            }
            else
            {
                // TODO: bump something
                var gameObject = _character.CurrentMap.GetObject(targetPos);
                
                var startingPos = _character.GlobalPosition;
                var toPos = GameController.DungeonMap[targetPos];
                
                _character.Tween.InterpolateProperty(_character, "global_position", startingPos, toPos, 0.1f);
                _character.Tween.Start();
                await _character.Tween.ToSignal(_character.Tween, "tween_completed");
                _character.Tween.InterpolateProperty(_character, "global_position", toPos, startingPos, 0.1f);
                _character.Tween.Start();

                await _character.ToSignal(_character.Tween, "tween_completed");

                if (gameObject is Character target)
                {
                    _character.AttackCharacter(target);
                    Finish(CommandResult.Success);
                }
                else
                {
                    Finish(CommandResult.Failure);
                }

            }
        }
    }
}