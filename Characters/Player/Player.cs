using System;
using System.Collections.Generic;
using Godot;
using GoRogue;
using SlashRoguelikedevTutorial2020.Scripts.Extensions;

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

        public Camera2D Camera;
        public Line2D Line;

        public override void _Ready()
        {
            base._Ready();
            Camera = GetNode<Camera2D>("Camera");
        }

        public override void _Input(InputEvent @event)
        {
            
            
            if (Tween.IsActive()) return;
            
            // keyboard movement
            if (@event is InputEventKey eventKey)
            {
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
            // mouse movement
            else if (@event is InputEventMouse mouseEvent)
            {
                if (mouseEvent.ButtonMask != 1) return;
        
                // TODO: Use Astar to find a path to the target
                // TODO: Visualize the path to the target
                var mousePos = Camera.GetGlobalMousePosition();
                var gridPos = GameController.DungeonMap.WorldToMap(mousePos).ToCoord();

                // don't allow player to click on unexplored tile
                if (!GameController.DungeonMap.Map.Explored[gridPos]) return;

                var path = GameController.DungeonMap.Map.AStar.ShortestPath(Position, gridPos);

                if (path == null || path.Length == 0) return;
                
                var moveDir = Direction.GetDirection(Position, path.GetStep(0));
                
                var gameObject = GameController.DungeonMap[gridPos];
                if (!(MoveIn(moveDir)))
                {
                    if (gameObject is Character character)
                    {
                        if (character is Player) return;
                        BumpObject(gameObject);
                        character.Kill();
                    }
                }
            }
        }
    }
}