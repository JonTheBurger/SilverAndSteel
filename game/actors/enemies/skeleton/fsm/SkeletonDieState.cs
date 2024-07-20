using Godot;

namespace Game;

[Icon("res://assets/img/icons/state.png")]
public partial class SkeletonDieState : State<Skeleton>
{
    [Export]
    public StringName Animation { get; set; } = "die";

    public override void OnEnter(State<Skeleton> previous)
    {
        Target.AnimationPlayer?.Play(Animation);
    }

    public override void OnExit(State<Skeleton> next)
    {
        Target.AnimationPlayer?.Stop();
    }

    public override void OnAnimationFinished(StringName animation)
    {
        Target.QueueFree();
    }
}