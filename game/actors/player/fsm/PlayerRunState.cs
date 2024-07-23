using System;

using Godot;

namespace Game;

[Icon("res://assets/img/icons/state.png")]
public partial class PlayerRunState : State<Player>
{
    [Export]
    public State<Player>? OnStop { get; set; }

    [Export]
    public State<Player>? OnJump { get; set; }

    [Export]
    public State<Player>? OnAttack { get; set; }

    [Export]
    public State<Player>? OnFall { get; set; }

    [Export]
    public StringName Animation { get; set; } = "run";

    public override void OnEnter(State<Player> previous)
    {
        Target.Animation?.Play(Animation);
    }

    public override void OnExit(State<Player> next)
    {
        Target.Animation?.Stop();
    }

    public override void ProcessPhysics(double delta)
    {
        Target.Move();
        if (!Target.IsOnFloor())
        {
            Next = OnFall;
        }
        else if (Input.IsActionJustPressed(Actions.JUMP))
        {
            Next = OnJump;
        }
        else if (Input.IsActionJustPressed(Actions.ATTACK))
        {
            Next = OnAttack;
        }
        else if (Math.Abs(Target.Velocity.X) < 0.0001)
        {
            Next = OnStop;
        }
    }
}