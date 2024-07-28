using Godot;

namespace Game;

[Icon("res://assets/img/icons/state.png")]
public partial class PlayerJumpHsm : Hsm<Player>
{
    [Export]
    public Hsm<Player>? OnLand { get; set; }

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
}