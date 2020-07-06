using System.Linq;
using Godot;
using GoRogue;
using Microsoft.Xna.Framework.Input;

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

                Player.InputAction.Run(moveDir);
                
                GameController.CommandManager.Run();
                GameController.StateMachine.ChangeState("EnemyTurn");
            }
        }
    }
}