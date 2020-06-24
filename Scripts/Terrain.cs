using GoRogue;
using GoRogue.GameFramework;

namespace SlashRoguelikedevTutorial2020.Scripts
{
    public class Terrain : GameObject
    {
        public Terrain(Coord position, bool isWalkable, bool isTransparent) : base(position, 0, null, true, isWalkable, isTransparent)
        {
            
        }
    }
}