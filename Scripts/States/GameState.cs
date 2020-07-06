using SlashRoguelikedevTutorial2020.Characters;

namespace SlashRoguelikedevTutorial2020.Scripts.States
{
    public class GameState : State
    {
        protected Player Player => GameController.Instance.Player;
    }
}