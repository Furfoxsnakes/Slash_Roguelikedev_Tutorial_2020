using Godot;

namespace SlashRoguelikedevTutorial2020.Scripts.States
{
    public class EnemyTurnState : GameState
    {
        public override void Enter()
        {
            foreach (var monster in GameController.DungeonMap.Monsters)
            {
                monster.Behaviour.Act(monster);
            }
            
            GameController.CommandManager.Run();

            GameController.StateMachine.ChangeState("PlayerTurn");
        }
    }
}