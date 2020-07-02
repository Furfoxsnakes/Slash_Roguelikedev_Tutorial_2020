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

        public static Vector2 ToVector2(this Coord c)
        {
            return new Vector2(c.X, c.Y);
        }
    }
}