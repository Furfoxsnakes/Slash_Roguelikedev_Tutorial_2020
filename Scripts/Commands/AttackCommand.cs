using Godot;
using GoRogue;

namespace SlashRoguelikedevTutorial2020.Scripts.Commands
{
    public class AttackCommand : BumpCommand
    {
        protected Character Defender;
        protected Character Attacker;
        
        public AttackCommand(Character attacker, Character defender) : base(attacker, defender.GlobalPosition)
        {
            Attacker = attacker;
            Defender = defender;
        }

        public override bool Execute()
        {
            Defender.TakeDamage();
            return base.Execute();
        }
    }
}