using Godot;

namespace Game;

[Icon("res://assets/img/icons/state.png")]
public partial class SkeletonDieHsm : Hsm<Skeleton>
{
    [Export]
    public StringName Animation { get; set; } = "die";

    public override void OnEnter(Hsm<Skeleton> previous)
    {
        base.OnEnter(previous);

        AnimationPlayer?.Play(Animation);
        Target.Velocity = Target.Velocity.WithX(0);
    }

    public override void OnExit(Hsm<Skeleton> next)
    {
        AnimationPlayer?.Stop();

        base.OnExit(next);
    }

    public override void OnAnimationFinished(StringName animation)
    {
        Target?.QueueFree();
    }
}