using Godot;

namespace Game;

[Icon("res://assets/img/icons/state.png")]
public partial class SkeletonIdleHsm : Hsm<Skeleton>
{
    [Export]
    public Hsm<Skeleton>? OnPlayerInRange { get; set; }

    [Export]
    public Hsm<Skeleton>? OnPlayerDetected { get; set; }

    [Export]
    public StringName Animation { get; set; } = "idle";

    protected override void OnEnter()
    {
        Animator?.Play(Animation);
    }

    protected override void OnExit()
    {
        Animator?.Stop();
    }

    protected override void OnProcessPhysics(double delta)
    {
        if (Target == null) { return; }

        if (Target.IsPlayerInRange)
        {
            Next = OnPlayerInRange;
        }
        else if (Target.IsPlayerDetected)
        {
            Next = OnPlayerDetected;
        }
    }
}
