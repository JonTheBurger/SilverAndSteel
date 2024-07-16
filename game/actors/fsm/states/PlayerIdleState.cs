using System;

using Godot;

namespace Game;

public partial class PlayerIdleState : ActorState
{
    [Export]
    public ActorState? OnMove { get; set; }

    [Export]
    public ActorState? OnAttack { get; set; }

    [Export]
    public ActorState? OnJump { get; set; }

    [Export]
    public ActorState? OnFall { get; set; }

    [Export]
    public StringName Animation { get; set; } = "idle";

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
        if (!Actor.IsOnFloor())
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
            if (Actor.IsOnFloor())
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