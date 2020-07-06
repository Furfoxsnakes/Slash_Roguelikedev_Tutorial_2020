using GoRogue;
using Microsoft.Xna.Framework;
using SlashRoguelikedevTutorial2020.Scripts.Commands;

namespace SlashRoguelikedevTutorial2020.Scripts.InputActions
{
    public class BasicMoveAndAttackAction : InputAction
    {
        public BasicMoveAndAttackAction(Character character) : base(character)
        {
            
        }

        public override void Run(Direction dir)
        {
            var newPos = Character.Position + dir;
            if (DungeonMap.Map.WalkabilityView[newPos])
            {
                GameController.CommandManager.Add(new MoveCommand(Character, dir));
                return;
            }
            
            // player can't move to new position
            // get a defender is available
            var defender = DungeonMap.Map.GetEntity<Character>(newPos);
            if (defender == null)
                // bump a wall or generic object
                GameController.CommandManager.Add(new BumpCommand(Character, DungeonMap[newPos]));
            else
                // attack the defender
                GameController.CommandManager.Add(new AttackCommand(Character, defender));
        }
    }
}