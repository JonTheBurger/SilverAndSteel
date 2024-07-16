using System;

using Godot;

namespace Game;

public partial class PlayerIdleState : State<Player>
{
    [Export]
    public State<Player>? OnMove { get; set; }

    [Export]
    public State<Player>? OnAttack { get; set; }

    [Export]
    public State<Player>? OnJump { get; set; }

    [Export]
    public State<Player>? OnFall { get; set; }

    [Export]
    public StringName Animation { get; set; } = "idle";

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
        if (!Target.IsOnFloor())
        {
            Next = OnFall;
        }
    }

    public override void ProcessInput(InputEvent input)
    {
        if (Input.IsActionJustPressed(Actions.ATTACK))
        {
            Next = OnAttack;
        }
        else if (Input.IsActionJustPressed(Actions.JUMP))
        {
            if (Target.IsOnFloor())
            {
                Next = OnJump;
            }
        }
        else if (Input.IsActionJustPressed(Actions.LEFT))
        {
            Next = OnMove;
        }
        else if (Input.IsActionJustPressed(Actions.RIGHT))
        {
            Next = OnMove;
        }
    }
}