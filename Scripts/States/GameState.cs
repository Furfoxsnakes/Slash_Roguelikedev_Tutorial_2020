using SlashRoguelikedevTutorial2020.Characters;

namespace SlashRoguelikedevTutorial2020.Scripts.States
{
    public class GameState : State
    {
        protected Player Player => GameController.Instance.Player;

        public override void Enter()
        {
            GameController.CommandManager.OnFinished += OnCommandManagerFinished;
        }

        public override void Exit()
        {
            GameController.CommandManager.OnFinished -= OnCommandManagerFinished;
        }

        protected virtual void OnCommandManagerFinished()
        {
            
        }
    }
}