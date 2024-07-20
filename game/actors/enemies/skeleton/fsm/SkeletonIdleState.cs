using Godot;

namespace Game;

[Icon("res://assets/img/icons/state.png")]
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
        Target.AnimationPlayer?.Play(Animation);
    }

    public override void OnExit(State<Skeleton> next)
    {
        Target.AnimationPlayer?.Stop();
    }

    public override void ProcessPhysics(double delta)
    {
        if (Target.Stats.Hp <= 0)
        {
            Next = OnHpZero;
        }
        else if (Target.IsPlayerInRange())
        {
            // TODO: Start timer before attacking
            Next = OnPlayerInRange;
        }
        else if (Target.IsPlayerOutOfRange())
        {
            Next = OnPlayerOutOfRange;
        }
    }
}