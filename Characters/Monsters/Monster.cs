using SlashRoguelikedevTutorial2020.Scripts.Behaviours;
using SlashRoguelikedevTutorial2020.Scripts.Interfaces;

public class Monster : Character
{
    public IBehaviour Behaviour;
    public int? TurnsAlerted { get; set; }

    public override void _Ready()
    {
        base._Ready();
        Behaviour = new StandardMoveAndAttack();
    }
}
