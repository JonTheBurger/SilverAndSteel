using Godot;

namespace Game;

[Icon("res://assets/img/icons/state.png")]
public partial class SkeletonWalkHsm : Hsm<Skeleton>
{
    [Export]
    public StringName Animation { get; set; } = "walk";

    [Export]
    public Hsm<Skeleton>? OnPlayerUndetected { get; set; }

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

        if (!Target.IsPlayerDetected)
        {
            Next = OnPlayerUndetected;
        }
    }
}