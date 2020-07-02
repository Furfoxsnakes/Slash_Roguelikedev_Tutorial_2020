using System.Collections.Generic;
using System.Linq;
using Godot;
using GoRogue;
using SlashRoguelikedevTutorial2020.Scripts.Extensions;

namespace SlashRoguelikedevTutorial2020.Characters
{
    public class Player : Character
    {
        /* handled by godot input mapping and Input Manager
        public Dictionary<string, Direction> InputMapping = new Dictionary<string, Direction>()
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
        */
        
        public Camera2D Camera;

        public override void _Ready()
        {
            base._Ready();
            Camera = GetNode<Camera2D>("Camera");
            SetProcessInput(false);
        }

        /* now all handled by Input Manager
        /* processing of input is turned off anyway
        /* just keeping this around just in case
        public override void _Input(InputEvent @event)
        {
            
            
            // don't allow any further actions until the character has moved from tweening
            if (Tween.IsActive()) return;

            // keyboard movement
            if (@event is InputEventKey eventKey)
            {
                if (!eventKey.IsPressed()) return;

                var keyString = OS.GetScancodeString(eventKey.Scancode);

                var moveDir = Direction.NONE;

                foreach (var action in InputMapping.Keys.Where(action => action == keyString))
                {
                    moveDir = InputMapping[action];
                    break;
                }

                if (moveDir == Direction.NONE) return;

                if (MoveIn(moveDir)) return;
                
                var gameObject = Map.GetObjectAtPos(Position + moveDir);
                BumpObject(Map.GetObjectAtPos(Position + moveDir));
                if (gameObject is Character character)
                    //character.Kill();
                    character.TakeDamage();
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

                GameController.DungeonMap.PathingTileMap.Clear();
                for (var i = 0; i < path.Length; ++i)
                {
                    var pathDir = Direction.GetDirection(path.GetStepWithStart(i), path.GetStepWithStart(i + 1));

                    var spriteIndex = 0;
                    switch (pathDir.ToString())
                    {
                        case "UP":
                            spriteIndex = 0;
                            break;
                        case "RIGHT":
                            spriteIndex = 1;
                            break;
                        case "DOWN":
                            spriteIndex = 2;
                            break;
                        case "LEFT":
                            spriteIndex = 3;
                            break;
                        case "UP_RIGHT":
                            spriteIndex = 4;
                            break;
                        case "DOWN_RIGHT":
                            spriteIndex = 5;
                            break;
                        case "DOWN_LEFT":
                            spriteIndex = 6;
                            break;
                        case "UP_LEFT":
                            spriteIndex = 7;
                            break;
                    }

                    GameController.DungeonMap.PathingTileMap.SetCellv(path.GetStep(i).ToVector2(), spriteIndex);
                }
                var moveDir = Direction.GetDirection(Position, path.GetStep(0));
                
                var gameObject = GameController.DungeonMap[gridPos];
                if (!(MoveIn(moveDir)))
                {
                    if (gameObject is Character character)
                    {
                        if (character is Player) return;
                        BumpObject(gameObject);
                        //character.Kill();
                        character.TakeDamage();
                    }
                }
                
            }
        }
        */

    }
}