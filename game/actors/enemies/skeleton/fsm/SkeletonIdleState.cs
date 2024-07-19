using Godot;

namespace Game;

[Icon("res://assets/img/icons/state.png")]
public partial class SkeletonIdleState : State<Skeleton>
{
    [Export]
    public State<Skeleton>? OnMove { get; set; }

    [Export]
    public StringName Animation { get; set; } = "idle";

    public override void OnEnter(State<Skeleton> previous)
    {
        Target.AnimationPlayer?.Play(Animation);
    }

    public override void OnExit(State<Skeleton> next)
    {
        Target.AnimationPlayer?.Stop();
    }
}
