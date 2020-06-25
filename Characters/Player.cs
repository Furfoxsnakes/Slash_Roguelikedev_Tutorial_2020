using System;
using Godot;
using GoRogue;

namespace SlashRoguelikedevTutorial2020.Characters
{
    public class Player : Character
    {
        public override void _Input(InputEvent @event)
        {
            if (!(@event is InputEventKey) || Tween.IsActive()) return;

            Vector2 movement;
            movement.x = Convert.ToInt32(Input.IsActionPressed("MoveEast")) -
                         Convert.ToInt32(Input.IsActionPressed("MoveWest"));
            movement.y = Convert.ToInt32(Input.IsActionPressed("MoveSouth")) -
                         Convert.ToInt32(Input.IsActionPressed("MoveNorth"));

            var coord = new Coord((int)movement.x, (int)movement.y);
            
            if (movement != Vector2.Zero)
            {
                MoveIn(Direction.GetDirection(Position, Position + coord));
            }
        }

        public override void _Ready()
        {
            base._Ready();
            GetNode<Camera2D>("Camera").Current = true;
        }
    }
}