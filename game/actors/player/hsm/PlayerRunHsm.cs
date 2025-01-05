using System;

using Godot;

namespace Game;

[GlobalClass]
[Icon("res://assets/img/icons/state.png")]
public partial class PlayerRunHsm : Hsm<Player>
{
    [Export]
    public Hsm<Player>? OnStop { get; set; }

    [Export]
    public Hsm<Player>? OnJump { get; set; }

    [Export]
    public Hsm<Player>? OnAttack { get; set; }

    [Export]
    public Hsm<Player>? OnCast { get; set; }

    [Export]
    public Hsm<Player>? OnFall { get; set; }

    [Export]
    public StringName Animation { get; set; } = "run";

    protected override void OnEnter()
    {
        Target.Animation?.Play(Animation);
    }

    protected override void OnExit()
    {
        Target.Animation?.Stop();
    }

    protected override void OnProcessPhysics(double delta)
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
        else if (Input.IsActionJustPressed(Actions.MAGIC))
        {
            Next = OnCast;
        }
        else if (Math.Abs(Target.Velocity.X) < 0.0001)
        {
            Next = OnStop;
        }
    }
}