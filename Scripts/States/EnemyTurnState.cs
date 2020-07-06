namespace SlashRoguelikedevTutorial2020.Scripts.States
{
    public class EnemyTurnState : GameState
    {
        public override void Enter()
        {
            base.Enter();
            foreach (var monster in GameController.DungeonMap.Monsters)
            {
                monster.Behaviour.Act(monster);
            }
            GameController.CommandManager.Run();
        }

        protected override void OnCommandManagerFinished()
        {
            GameController.StateMachine.ChangeState("PlayerTurn");
        }
    }
}