namespace SlashRoguelikedevTutorial2020.Scripts.Commands
{
    public struct CommandResult
    {
        public readonly bool IsSuccess;

        public static readonly CommandResult Success = new CommandResult(true);
        public static readonly CommandResult Failure = new CommandResult(false);
        
        public CommandResult(bool success)
        {
            IsSuccess = success;
        }
    }
}