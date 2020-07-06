using Godot;
using System;
using SlashRoguelikedevTutorial2020.Scripts.Behaviours;
using SlashRoguelikedevTutorial2020.Scripts.Interfaces;

public class Monster : Character
{
    public IBehaviour Behaviour;

    public override void _Ready()
    {
        base._Ready();
        Behaviour = new StandardMoveAndAttack();
    }
}
