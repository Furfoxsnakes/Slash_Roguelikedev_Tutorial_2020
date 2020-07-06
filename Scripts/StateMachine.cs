using System.Collections.Generic;
using Godot;

namespace SlashRoguelikedevTutorial2020.Scripts
{
    public class StateMachine : Node
    {
        public GameController Game;
        public Dictionary<string, State> States = new Dictionary<string, State>();

        private State _currentState;
        private State _previousState;

        protected bool IsActive
        {
            get => _isActive;
            set => SetActive(value);
        }
        private bool _isActive;

        public override void _Ready()
        {
            SetPhysicsProcess(false);
            SetProcessInput(false);
            Game = Owner as GameController;
        }

        /// <summary>
        /// Initializes the state machine with a start state
        /// </summary>
        /// <param name="initialStateName">The states to start the state machine on</param>
        public void Start(string initialStateName)
        {
            SetActive(true);
            GetStateNodes();
            ChangeState(initialStateName);
        }

        /// <summary>
        /// Populates the States dictionary with all the state nodes added in the editor
        /// </summary>
        public void GetStateNodes()
        {
            foreach (State state in GetChildren())
            {
                if (States.ContainsKey(state.Name)) continue;

                States[state.Name] = state;
                States[state.Name].Game = (GameController) Owner;
            }
        }

        /// <summary>
        /// Change to a requested state
        /// </summary>
        /// <param name="stateName">Name of the state to be changed to</param>
        public void ChangeState(string stateName)
        {
            if (!IsActive || !States.ContainsKey(stateName)) return;

            if (_currentState != null)
            {
                _previousState = _currentState;
                _currentState.Exit();
            }

            _currentState = States[stateName];
            _currentState?.Enter();
        }

        public override void _Input(InputEvent @event)
        {
            _currentState?.HandleInput(@event);
        }

        public override void _PhysicsProcess(float delta)
        {
            _currentState.Update(delta);
        }

        private void SetActive(bool value)
        {
            _isActive = value;
            SetPhysicsProcess(value);
            SetProcessInput(value);

            if (!_isActive) _currentState = null;
        }
    }
}