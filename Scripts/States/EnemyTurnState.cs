namespace SlashRoguelikedevTutorial2020.Scripts.States
{
    public class EnemyTurnState : GameState
    {
        public override async void Enter()
        {
            base.Enter();
            foreach (var monster in GameController.DungeonMap.Monsters)
            {
                // TODO: BUG FIX: Enemies appear to attack the last spot the enemy was in if moving to a new cell
                monster.Behaviour.Act(monster);
            }

            GameController.StateMachine.ChangeState("PlayerTurn");
        }
    }
}