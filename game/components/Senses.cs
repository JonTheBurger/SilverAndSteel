using Godot;

namespace Game;

/// <summary>
/// <code>
/// #region Senses
/// public Senses? Senses { get; private set; }
/// public bool IsPlayerDetected => Senses?.IsPlayerDetected ?? false;
/// public bool IsPlayerInRange => Senses?.IsPlayerInRange ?? false;
/// public Player? DetectedPlayer => Senses?.DetectedPlayer;
/// #endregion
/// </code>
/// </summary>
[GlobalClass]
[Icon("res://assets/img/icons/lightbulb.png")]
public partial class Senses : Node2D
{
    [Export]
    public Area2D? DetectionRadius { get; set; }
    [Export]
    public Area2D? AttackRange { get; set; }

    public Player? DetectedPlayer { get; private set; } = null;
    public bool IsPlayerDetected => DetectedPlayer != null;
    public bool IsPlayerInRange { get; private set; }

    public override void _Ready()
    {
        if (DetectionRadius != null)
        {
            DetectionRadius.BodyEntered += OnDetectionRadiusEnter;
            DetectionRadius.BodyExited += OnDetectionRadiusExit;
        }
        if (AttackRange != null)
        {
            AttackRange.BodyEntered += OnAttackRangeEnter;
            AttackRange.BodyExited += OnAttackRangeExit;
        }
    }

    private void OnDetectionRadiusEnter(Node2D body)
    {
        if (body.IsInGroup(Groups.PLAYERS)) { DetectedPlayer = body as Player; }
    }

    private void OnDetectionRadiusExit(Node2D body)
    {
        if (body.IsInGroup(Groups.PLAYERS)) { DetectedPlayer = null; }
    }

    private void OnAttackRangeEnter(Node2D body)
    {
        if (body.IsInGroup(Groups.PLAYERS)) { IsPlayerInRange = true; }
    }

    private void OnAttackRangeExit(Node2D body)
    {
        if (body.IsInGroup(Groups.PLAYERS)) { IsPlayerInRange = false; }
    }
}