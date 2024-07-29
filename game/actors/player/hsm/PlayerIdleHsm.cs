using Godot;

namespace Game;

[Icon("res://assets/img/icons/state.png")]
public partial class PlayerIdleHsm : Hsm<Player>
{
    [Export]
    public Hsm<Player>? OnMove { get; set; }

    [Export]
    public Hsm<Player>? OnAttack { get; set; }

    [Export]
    public Hsm<Player>? OnJump { get; set; }

    [Export]
    public Hsm<Player>? OnFall { get; set; }

    [Export]
    public StringName Animation { get; set; } = "idle";

    protected override void OnEnter()
    {
        Target.Animation?.Play(Animation);
        Target.Velocity = Target.Velocity.WithX(0);
    }

    protected override void OnExit()
    {
        Target.Animation?.Stop();
    }

    protected override void OnProcessPhysics(double delta)
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

    protected override void OnProcessInput(InputEvent input)
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