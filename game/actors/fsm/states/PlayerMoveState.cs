using Godot;
using System;

namespace Game;

public partial class PlayerMoveState : ActorState
{
    [Export]
    public ActorState? OnStop { get; set; }

    [Export]
    public string Animation { get; set; } = "run";

    public override void OnEnter(ActorState previous)
    {
        AnimationPlayer?.Play(Animation);
    }

    public override void OnExit(ActorState next)
    {
        AnimationPlayer?.Stop();
    }

    public override void ProcessPhysics(double delta)
    {
        if (Math.Abs(CharacterBody2D.Velocity.X) < 0.0001)
        {
            Machine.Next = OnStop;
        }
    }
}
