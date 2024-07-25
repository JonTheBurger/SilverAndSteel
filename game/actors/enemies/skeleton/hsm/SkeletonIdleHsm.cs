using Godot;

namespace Game;

[Icon("res://assets/img/icons/state.png")]
public partial class SkeletonIdleHsm : Hsm<Skeleton>
{
    [Export]
    public Hsm<Skeleton>? OnPlayerDetected { get; set; }

    [Export]
    public StringName Animation { get; set; } = "idle";

    public override void OnEnter(Hsm<Skeleton> previous)
    {
        base.OnEnter(previous);

        AnimationPlayer?.Play(Animation);
    }

    public override void OnExit(Hsm<Skeleton> next)
    {
        AnimationPlayer?.Stop();

        base.OnExit(next);
    }

    public override void ProcessPhysics(double delta)
    {
        base.ProcessPhysics(delta);

        if (Target == null) { return; }

        if (Target.IsPlayerDetected)
        {
            Next = OnPlayerDetected;
        }
    }
}