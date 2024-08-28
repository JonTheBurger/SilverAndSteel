using Godot;

namespace Game;

[Icon("res://assets/img/icons/state.png")]
public partial class WalkState : Hsm<CharacterBody2D>
{
    [Export]
    public StringName Animation { get; set; } = "walk";

    [Export]
    public Thoughts? Thoughts { get; set; }

    [Export]
    public Directional? Directional { get; set; }

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
        if ((Target == null) || (Thoughts == null)) { return; }

        if (Thoughts.DetectedPlayer != null)
        {
            Target.SlideToward(Thoughts.DetectedPlayer, Stats?.Speed ?? 0);
        }

        if ((Next == null) && Thoughts.IsPlayerInRange)
        {
            Next = OnPlayerInRange;
        }
        else if ((Next == null) && !Thoughts.IsPlayerDetected)
        {
            Next = OnPlayerUndetected;
        }
    }
}