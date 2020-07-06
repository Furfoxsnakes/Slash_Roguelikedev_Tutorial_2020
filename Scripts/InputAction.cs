﻿using GoRogue;
using GoRogue.GameFramework;

namespace SlashRoguelikedevTutorial2020.Scripts
{
    public class InputAction
    {
        protected Character Character;
        protected DungeonMap DungeonMap => GameController.DungeonMap;
        
        public InputAction(Character character)
        {
            Character = character;
        }

        public virtual void Run(Direction dir)
        {
            
        }
    }
}