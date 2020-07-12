using Godot;

namespace SlashRoguelikedevTutorial2020.Scripts
{
    public class State : Node
    {
        public StateMachine Parent;    // StateMachine should be the parent of each state
        public GameController Game;   // root owner should be gamecontroller node

        [Signal]
        public delegate void StateFinished();

        public event StateFinished OnStateFinished;

        public override void _Ready()
        {
            Parent = GetParent<StateMachine>();
            Game = Owner as GameController;
        }

        public virtual void Enter()
        {
            
        }

        public virtual void Exit()
        {
            
        }

        public virtual void Update(float delta)
        {
            
        }

        public virtual void HandleInput(InputEvent @event)
        {
            
        }
    }
}