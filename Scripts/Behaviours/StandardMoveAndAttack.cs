using GoRogue;
using SlashRoguelikedevTutorial2020.Scripts.Commands;
using SlashRoguelikedevTutorial2020.Scripts.Interfaces;

namespace SlashRoguelikedevTutorial2020.Scripts.Behaviours
{
    public class StandardMoveAndAttack : IBehaviour
    {
        public void Act(Monster monster)
        {
            var dungeonMap = GameController.DungeonMap;
            var player = GameController.Instance.Player;
            var fov = new FOV(dungeonMap.Map.TransparencyView);

            if (!monster.TurnsAlerted.HasValue)
            {
                fov.Calculate(monster.Position, monster.FOVRadius, Radius.DIAMOND);
                if (fov[player.Position] > 0)
                    monster.TurnsAlerted = 1;
            }

            if (!monster.TurnsAlerted.HasValue) return;

            var path = dungeonMap.Map.AStar.ShortestPath(monster.Position, player.Position);
                
            if (path == null) return;

            //var inputAction = new BasicMoveAndAttackAction(monster);
            var moveDir = Direction.GetDirection(monster.Position, path.GetStep(0));
            //inputAction.Run(moveDir);
            
            monster.TurnsAlerted++;

            if (monster.TurnsAlerted > 15)
                monster.TurnsAlerted = null;
            
            var moveCommand = new MoveCommand(monster, moveDir);
            //moveCommand.OnSuccessMethod = OnMoveSuccess;
            GameController.CommandManager.PushAndRun(moveCommand);

        }
    }
}