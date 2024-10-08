using Godot;

namespace Game;

[Icon("res://assets/img/icons/state.png")]
public partial class SkeletonDieHsm : Hsm<Skeleton>
{
    [Export]
    public StringName Animation { get; set; } = "die";

    protected override void OnEnter()
    {
        Animator?.Play(Animation);
        Target.CollisionLayer = (uint)CollisionLayers.None;
        Target.CollisionMask = (uint)CollisionLayers.Ground;
        Target.Velocity = Target.Velocity.WithX(0);
    }

    protected override void OnExit()
    {
        Animator?.Stop();
    }

    protected override void OnAnimationFinished(StringName animation)
    {
        Target?.QueueFree();
    }
}