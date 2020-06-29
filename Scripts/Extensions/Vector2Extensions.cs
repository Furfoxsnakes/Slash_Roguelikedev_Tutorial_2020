using Godot;
using GoRogue;

namespace SlashRoguelikedevTutorial2020.Scripts.Extensions
{
    public static class Vector2Extensions
    {
        public static Coord ToCoord(this Vector2 v)
        {
            return new Coord((int)v.x, (int)v.y);
        }
    }
}