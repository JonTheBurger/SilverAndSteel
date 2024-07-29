using Godot;

namespace Game;

[Icon("res://assets/img/icons/state.png")]
public partial class SkeletonDieHsm : Hsm<Skeleton>
{
    [Export]
    public StringName Animation { get; set; } = "die";

    protected override void OnEnter()
    {
        AnimationPlayer?.Play(Animation);
        Target.Hitbox.Disabled = true;
        Target.Gravity = 0;
        Target.Velocity = Target.Velocity.WithX(0);
    }

    protected override void OnExit()
    {
        AnimationPlayer?.Stop();
    }

    protected override void OnAnimationFinished(StringName animation)
    {
        Target?.QueueFree();
    }
}