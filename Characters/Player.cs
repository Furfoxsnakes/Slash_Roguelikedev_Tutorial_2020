using System;
using Godot;

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

            if (movement != Vector2.Zero)
                MoveBy(movement);
        }
    }
}