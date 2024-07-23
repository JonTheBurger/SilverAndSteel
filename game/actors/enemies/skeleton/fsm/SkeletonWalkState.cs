using Godot;

namespace Game;

[Icon("res://assets/img/icons/state.png")]
[Tool]
public partial class SkeletonWalkState : State<Skeleton>
{
    [Export]
    public State<Skeleton>? OnPlayerInRange { get; set; }

    [Export]
    public State<Skeleton>? OnHpZero { get; set; }

    [Export]
    public StringName Animation { get; set; } = "walk";

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

        // TODO: Check that the floor is still there before moving
        Target.MoveTowardsPlayer();

        if (Target.Stats.Hp <= 0)
        {
            Next = OnHpZero;
        }
        else if (Target.IsPlayerInRange)
        {
            Next = OnPlayerInRange;
        }
    }
}