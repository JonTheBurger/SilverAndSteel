using Godot;

namespace Game;

[Icon("res://assets/img/icons/state.png")]
[Tool]
public partial class SkeletonIdleState : State<Skeleton>
{
    [Export]
    public State<Skeleton>? OnPlayerInRange { get; set; }

    [Export]
    public State<Skeleton>? OnPlayerOutOfRange { get; set; }

    [Export]
    public State<Skeleton>? OnHpZero { get; set; }

    [Export]
    public StringName Animation { get; set; } = "idle";

    public override void OnEnter(State<Skeleton> previous)
    {
        Target?.Animation?.Play(Animation);
    }

    public override void OnExit(State<Skeleton> next)
    {
        Target?.Animation?.Stop();
    }

    public override void ProcessPhysics(double delta)
    {
        if (Target == null) { return; }
        if (Target.Stats.Hp <= 0)
        {
            Next = OnHpZero;
        }
        else if (Target.IsPlayerOutOfRange())
        {
            Next = OnPlayerOutOfRange;
        }
        else if (Target.IsPlayerInRange())
        {
            Next = OnPlayerInRange;
        }
    }
}