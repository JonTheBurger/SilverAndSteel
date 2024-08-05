using Godot;

namespace Game;

[Icon("res://assets/img/icons/state.png")]
public partial class WalkState : Hsm<CharacterBody2D>
{
    [Export]
    public StringName Animation { get; set; } = "walk";

    [Export]
    public Hsm<Skeleton>? OnPlayerInRange { get; set; }

    [Export]
    public Hsm<Skeleton>? OnPlayerUndetected { get; set; }

    protected override void OnEnter()
    {
        Animator?.Play(Animation);
    }

    protected override void OnExit()
    {
        Animator?.Stop();
        Target.Velocity = Target.Velocity.WithX(0);
    }

    protected override void OnProcessPhysics(double delta)
    {
        if (Target == null) { return; }

        // Target.MoveTowardsPlayer();

        // if (Target.IsPlayerInRange)
        // {
        //     Next = OnPlayerInRange;
        // }
        // else if (!Target.IsPlayerDetected)
        // {
        //     Next = OnPlayerUndetected;
        // }
    }
}