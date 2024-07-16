using System;

using Godot;

namespace Game;

public partial class PlayerRunState : ActorState
{
    [Export]
    public ActorState? OnStop { get; set; }

    [Export]
    public StringName Animation { get; set; } = "run";

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
        if (Math.Abs(Actor.Velocity.X) < 0.0001)
        {
            Next = OnStop;
        }
    }
}