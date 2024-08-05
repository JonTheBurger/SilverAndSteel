using Godot;

namespace Game;

[Icon("res://assets/img/icons/state.png")]
public partial class IdleState : Hsm<CharacterBody2D>
{
    [Export]
    public StringName Animation { get; set; } = "idle";

    [Export]
    public Hsm<CharacterBody2D>? OnFall { get; set; }

    [Export]
    public Area2D? AttackRange { get; set; }

    [Export]
    public Hsm<CharacterBody2D>? OnPlayerInRange { get; set; }

    private bool _isPlayerInRange = false;

    [Export]
    public Area2D? DetectionRadius { get; set; }

    [Export]
    public Hsm<CharacterBody2D>? OnPlayerDetected { get; set; }

    private bool _isPlayerDetected = false;

    public override void _Ready()
    {
        if (AttackRange != null)
        {
            AttackRange.BodyEntered += OnAttackRangeEnter;
            AttackRange.BodyExited += OnAttackRangeExit;
        }
        if (DetectionRadius != null)
        {
            DetectionRadius.BodyEntered += OnDetectionRadiusEnter;
            DetectionRadius.BodyExited += OnDetectionRadiusExit;
        }
    }

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
        else if (_isPlayerInRange)
        {
            Next = OnPlayerInRange;
        }
        else if (_isPlayerDetected)
        {
            Next = OnPlayerDetected;
        }
    }

    private void OnAttackRangeEnter(Node2D body)
    {
        if (body.IsInGroup(Groups.PLAYERS)) { _isPlayerInRange = true; }
    }

    private void OnAttackRangeExit(Node2D body)
    {
        if (body.IsInGroup(Groups.PLAYERS)) { _isPlayerInRange = false; }
    }

    private void OnDetectionRadiusEnter(Node2D body)
    {
        if (body.IsInGroup(Groups.PLAYERS)) { _isPlayerDetected = true; }
    }

    private void OnDetectionRadiusExit(Node2D body)
    {
        if (body.IsInGroup(Groups.PLAYERS)) { _isPlayerDetected = false; }
    }
}