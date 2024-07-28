using Godot;

namespace Game;

[Icon("res://assets/img/icons/state.png")]
public partial class SkeletonAttackHsm : Hsm<Skeleton>
{
    [Export]
    public StringName Animation { get; set; } = "attack";

    [Export]
    public Hsm<Skeleton>? OnAttackComplete { get; set; }

    protected override void OnEnter()
    {
        Target.Animation?.Play(Animation);
        Target.Velocity = Target.Velocity.WithX(0);
    }

    protected override void OnAnimationFinished(StringName animation)
    {
        Next = OnAttackComplete;
    }
}