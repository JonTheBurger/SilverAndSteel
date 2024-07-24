using Godot;

namespace Game;

[Icon("res://assets/img/icons/state.png")]
[Tool]
public partial class SkeletonIdleState : State<Skeleton>
{
    [Export]
    public State<Skeleton>? OnPlayerInRange { get; set; }

    [Export]
    public State<Skeleton>? OnPlayerDetected { get; set; }

    [Export]
    public State<Skeleton>? OnHpZero { get; set; }

    [Export]
    public StringName Animation { get; set; } = "idle";

    public override void OnEnter(State<Skeleton> previous)
    {
        AnimationPlayer?.Play(Animation);
    }

    public override void OnExit(State<Skeleton> next)
    {
        AnimationPlayer?.Stop();
    }

    public override void ProcessPhysics(double delta)
    {
        if (Target == null) { return; }

        if (Target.Stats.Hp <= 0)
        {
            Next = OnHpZero;
        }
        else if (Target.IsPlayerInRange)
        {
            Next = OnPlayerInRange;
        }
        else if (Target.IsPlayerDetected)
        {
            Next = OnPlayerDetected;
        }
    }
}