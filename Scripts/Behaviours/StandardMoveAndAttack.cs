using Godot;
using GoRogue;
using SlashRoguelikedevTutorial2020.Scripts.InputActions;
using SlashRoguelikedevTutorial2020.Scripts.Interfaces;

namespace SlashRoguelikedevTutorial2020.Scripts.Behaviours
{
    public class StandardMoveAndAttack : IBehaviour
    {
        public void Act(Character character)
        {
            var dungeonMap = GameController.DungeonMap;
            var player = GameController.Instance.Player;
            var fov = new FOV(dungeonMap.Map.TransparencyView);

            fov.Calculate(character.Position, character.FOVRadius, Radius.DIAMOND);
            if (fov[player.Position] == 0) return;

            var path = dungeonMap.Map.AStar.ShortestPath(character.Position, player.Position);

            if (path == null) return;

            var inputAction = new BasicMoveAndAttackAction(character);
            var moveDir = Direction.GetDirection(character.Position, path.GetStep(0));
            inputAction.Run(moveDir);
        }
    }
}