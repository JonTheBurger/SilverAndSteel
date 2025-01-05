using Godot;

namespace Game;

[GlobalClass]
[Icon("res://assets/img/icons/state.png")]
public partial class PlayerJumpHsm : Hsm<Player>
{
    [Export]
    public Hsm<Player>? OnLand { get; set; }

    [Export]
    public Hsm<Player>? OnAttack { get; set; }

    [Export]
    public Hsm<Player>? OnCast { get; set; }

    [Export]
    public StringName Animation { get; set; } = "jump_air";

    protected override void OnEnter()
    {
        Target.Animation?.Play(Animation);
        Target.Velocity = Target.Velocity.WithY(-150.0f);
    }

    protected override void OnExit()
    {
        Target.Animation?.Stop();
    }

    protected override void OnProcessPhysics(double delta)
    {
        Target.Move();
        if (Target.IsOnFloor())
        {
            Next = OnLand;
        }
    }

    protected override void OnProcessInput(InputEvent input)
    {
        if (Input.IsActionJustPressed(Actions.ATTACK))
        {
            Next = OnAttack;
        }
        else if (Input.IsActionJustPressed(Actions.MAGIC))
        {
            Next = OnCast;
        }
    }
}