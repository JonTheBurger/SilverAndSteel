using System;

using Godot;

namespace Game;

public partial class PlayerRunState : State<Player>
{
    [Export]
    public State<Player>? OnStop { get; set; }

    [Export]
    public StringName Animation { get; set; } = "run";

    public override void OnEnter(State<Player> previous)
    {
        Target.AnimationPlayer?.Play(Animation);
    }

    public override void OnExit(State<Player> next)
    {
        Target.AnimationPlayer?.Stop();
    }

    public override void ProcessPhysics(double delta)
    {
        if (Math.Abs(Target.Velocity.X) < 0.0001)
        {
            Next = OnStop;
        }
    }
}