﻿using System.Collections.Generic;

namespace SlashRoguelikedevTutorial2020.Scripts
{
    public class CommandManager
    {
        public delegate void Finished();
        public event Finished OnFinished;
        
        private Queue<Command> _commands;

        public CommandManager()
        {
            _commands = new Queue<Command>();
        }

        public void Run()
        {
            //if (_commands.Count <= 0) return;
            
            while (_commands.Count > 0)
                _commands.Dequeue().Execute();
            
            OnFinished?.Invoke();
        }

        public Command Next()
        {
            if (_commands.Count <= 0) return null;

            return _commands.Dequeue();
        }

        public void Add(Command command)
        {
            _commands.Enqueue(command);
        }
    }
    
}