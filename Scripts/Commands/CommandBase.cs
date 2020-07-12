using System;
using System.Runtime.InteropServices;
using SlashRoguelikedevTutorial2020.Scripts.Commands;

namespace SlashRoguelikedevTutorial2020.Scripts
{
    public abstract class CommandBase
    {
        public Func<CommandBase, bool> OnSuccessMethod;
        public Func<CommandBase, bool> OnFailureMethod;

        public bool IsFinished { get; private set; }
        public CommandResult Result { get; private set; } = CommandResult.Failure;

        public abstract void Execute();

        public void Finish(CommandResult result)
        {
            Result = result;
            IsFinished = true;

            if (Result.IsSuccess && (OnSuccessMethod?.Invoke(this) ?? true))
                OnSuccessResult();
            else if (OnFailureMethod?.Invoke(this) ?? true)
                OnFailureResult();
        }

        public virtual void OnSuccessResult() {}
        public virtual void OnFailureResult() {}
    }
}