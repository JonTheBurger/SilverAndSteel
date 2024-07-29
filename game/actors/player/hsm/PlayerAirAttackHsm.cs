using Godot;

namespace Game;

[Icon("res://assets/img/icons/state.png")]
public partial class PlayerAirAttackHsm : Hsm<Player>
{
    [Export]
    public StringName Animation { get; set; } = "air_attack";

    [Export]
    public Hsm<Player>? OnAttackComplete { get; set; }

    [Export]
    public Hsm<Player>? OnLand { get; set; }

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
        Target.Move();
        if (Target.IsOnFloor())
        {
            Next = OnLand;
        }
    }

    protected override void OnAnimationFinished(StringName animation)
    {
        Next = OnAttackComplete;
    }
}
