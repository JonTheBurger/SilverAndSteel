using Godot;
namespace Game;

[Icon("res://assets/img/icons/state.png")]
public partial class IdleState : Hsm<CharacterBody2D>
{
    [Export]
    public StringName Animation { get; set; } = "idle";

    [Export]
    public Thoughts? Thoughts { get; set; }

    [Export]
    public Hsm<CharacterBody2D>? OnFall { get; set; }

    [Export]
    public Hsm<CharacterBody2D>? OnPlayerInRange { get; set; }

    [Export]
    public Hsm<CharacterBody2D>? OnPlayerDetected { get; set; }

    private bool _isPlayerDetected = false;

    protected override void OnEnter()
    {
        Animator?.Play(Animation);
    }

    protected override void OnExit()
    {
        Animator?.Stop();
    }

    protected override void OnProcessPhysics(double delta)
    {
        if (Target == null) { return; }

        if ((OnFall != null) && !Target.IsOnFloor())
        {
            Next = OnFall;
        }
        else if (Thoughts?.IsPlayerInRange ?? false)
        {
            Next = OnPlayerInRange;
        }
        else if (Thoughts?.IsPlayerDetected ?? false)
        {
            Next = OnPlayerDetected;
        }
    }
}