using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using GoRogue;
using SlashRoguelikedevTutorial2020.Scripts;
using SlashRoguelikedevTutorial2020.Scripts.Extensions;
using SlashRoguelikedevTutorial2020.Scripts.InputActions;

namespace SlashRoguelikedevTutorial2020.Characters
{
    public class Player : Character
    {
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

        public Camera2D Camera;
        public InputAction InputAction;

        public override void _Ready()
        {
            base._Ready();
            Camera = GetNode<Camera2D>("Camera");
            SetProcessInput(false);
            InputAction = new BasicMoveAndAttackAction(this);
        }
    }
}