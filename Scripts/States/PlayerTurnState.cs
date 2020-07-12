using System;
using System.Linq;
using System.Threading.Tasks;
using Godot;
using GoRogue;
using SlashRoguelikedevTutorial2020.Scripts.Commands;

namespace SlashRoguelikedevTutorial2020.Scripts.States
{
    public class PlayerTurnState : GameState
    {
        public override void HandleInput(InputEvent @event)
        {
            if (Player.Tween.IsActive()) return;

            if (@event is InputEventKey eventKey)
            {
                if (!eventKey.IsPressed()) return;

                var keyString = OS.GetScancodeString(eventKey.Scancode);

                var moveDir = Direction.NONE;

                foreach (var action in Player.InputMapping.Keys.Where(action => action == keyString))
                {
                    moveDir = Player.InputMapping[action];
                    break;
                }

                if (moveDir == Direction.NONE) return;

                var moveCommand = new MoveCommand(Player, moveDir);
                moveCommand.OnSuccessMethod = OnMoveSuccess;

                GameController.CommandManager.PushAndRun(moveCommand);
            }
        }

        private bool OnMoveSuccess(CommandBase arg)
        {
            GameController.StateMachine.ChangeState("EnemyTurn");
            return true;
        }
    }
}