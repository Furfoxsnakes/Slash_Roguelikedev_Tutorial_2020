using GoRogue;
using GoRogue.GameFramework;

namespace SlashRoguelikedevTutorial2020.Scripts.Commands
{
    public class MoveCommand : Command
    {
        private Character _character;
        private Direction _dir;

        public MoveCommand(Character character, Direction direction)
        {
            _character = character;
            _dir = direction;
        }

        public override bool Execute()
        {
            return _character.MoveIn(_dir);
        }
    }
}