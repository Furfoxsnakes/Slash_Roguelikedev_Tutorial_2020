using System;
using System.Collections.Generic;
using Godot;
using GoRogue;

namespace SlashRoguelikedevTutorial2020.Characters
{
    public class Player : Character
    {
        private Dictionary<string, Direction> _inputMapping = new Dictionary<string, Direction>()
        {
            {"Kp 1", Direction.DOWN_LEFT},
            {"Kp 2", Direction.DOWN},
            {"Kp 3", Direction.DOWN_RIGHT},
            {"Kp 4", Direction.LEFT},
            {"Kp 6", Direction.RIGHT},
            {"Kp 7", Direction.UP_LEFT},
            {"Kp 8", Direction.UP},
            {"Kp 9", Direction.UP_RIGHT},
        };
        
        public override void _Input(InputEvent @event)
        {
            if (!(@event is InputEventKey eventKey) || Tween.IsActive()) return;

            if (!eventKey.IsPressed()) return;
            
            var keyString = OS.GetScancodeString(eventKey.Scancode);

            var moveDir = Direction.NONE;

            foreach (var action in _inputMapping.Keys)
            {
                if (action == keyString)
                {
                    moveDir = _inputMapping[action];
                    break;
                }
            }

            if (moveDir != Direction.NONE)
                if (!MoveIn(moveDir))
                {
                    var gameObject = Map.GetObjectAtPos(Position + moveDir);
                    BumpObject(Map.GetObjectAtPos(Position + moveDir));
                    if (gameObject is Character character)
                        character.Kill();
                }
        }
    }
}