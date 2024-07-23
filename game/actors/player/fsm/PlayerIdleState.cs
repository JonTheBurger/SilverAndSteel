using Godot;

namespace Game;

[Icon("res://assets/img/icons/state.png")]
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
        Target.Animation?.Play(Animation);
    }

    public override void OnExit(State<Player> next)
    {
        Target.Animation?.Stop();
    }

    public override void ProcessPhysics(double delta)
    {
        if (Input.IsActionPressed(Actions.LEFT) && !Input.IsActionPressed(Actions.RIGHT))
        {
            Next = OnMove;
        }
        else if (Input.IsActionPressed(Actions.RIGHT) && !Input.IsActionPressed(Actions.LEFT))
        {
            Next = OnMove;
        }

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
    }
}