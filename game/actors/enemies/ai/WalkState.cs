using Godot;

namespace Game;

[Icon("res://assets/img/icons/state.png")]
public partial class WalkState : Hsm<CharacterBody2D>
{
    [Export]
    public StringName Animation { get; set; } = "walk";

    [Export]
    public Beliefs? Beliefs { get; set; }

    [Export]
    public Stats? Stats { get; set; }

    [Export]
    public Hsm<CharacterBody2D>? OnPlayerInRange { get; set; }

    [Export]
    public Hsm<CharacterBody2D>? OnPlayerUndetected { get; set; }

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
        if ((Target == null) || (Beliefs == null)) { return; }

        if (Beliefs.DetectedPlayer != null)
        {
            Target.SlideToward(Beliefs.DetectedPlayer, Stats?.Speed ?? 0);
        }

        if ((Next == null) && Beliefs.IsPlayerInRange)
        {
            Next = OnPlayerInRange;
        }
        else if ((Next == null) && !Beliefs.IsPlayerDetected)
        {
            Next = OnPlayerUndetected;
        }
    }
}