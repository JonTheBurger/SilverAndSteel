using Godot;

namespace Game;

[Icon("res://assets/img/icons/state.png")]
public partial class SkeletonWalkHsm : Hsm<Skeleton>
{
    [Export]
    public StringName Animation { get; set; } = "walk";

    [Export]
    public Hsm<Skeleton>? OnPlayerInRange { get; set; }

    [Export]
    public Hsm<Skeleton>? OnPlayerUndetected { get; set; }

    protected override void OnEnter()
    {
        AnimationPlayer?.Play(Animation);
    }

    protected override void OnExit()
    {
        AnimationPlayer?.Stop();
        Target.Stop();
    }

    protected override void OnProcessPhysics(double delta)
    {
        if (Target == null) { return; }

        Target.MoveTowardsPlayer();

        if (Target.IsPlayerInRange)
        {
            Next = OnPlayerInRange;
        }
        else if (!Target.IsPlayerDetected)
        {
            Next = OnPlayerUndetected;
        }
    }
}