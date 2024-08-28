using Godot;
using System;

namespace Game;

public partial class Phantom : Actor
{
    private Hsm<CharacterBody2D>? _fsm;

    public override void _Ready()
    {
        base._Ready();

        _fsm = GetNode<Hsm<CharacterBody2D>>("StateMachine");
        _fsm.Start(this);
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        _fsm.ProcessPhysics(delta);
    }
}
