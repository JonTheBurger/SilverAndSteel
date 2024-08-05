using Godot;
using Godot.Collections;

namespace Game;

public partial class Beliefs : Node2D
{
    [Export]
    public Area2D? AttackRange { get; set; }

    [Export]
    public Area2D? DetectionRadius { get; set; }

    public Player? DetectedPlayer { get; private set; } = null;
    public bool IsPlayerDetected => DetectedPlayer != null;
    public bool IsPlayerInRange { get; private set; }

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

    private void OnAttackRangeEnter(Node2D body)
    {
        if (body.IsInGroup(Groups.PLAYERS)) { IsPlayerInRange = true; }
    }

    private void OnAttackRangeExit(Node2D body)
    {
        if (body.IsInGroup(Groups.PLAYERS)) { IsPlayerInRange = false; }
    }

    private void OnDetectionRadiusEnter(Node2D body)
    {
        if (body.IsInGroup(Groups.PLAYERS)) { DetectedPlayer = body as Player; }
    }

    private void OnDetectionRadiusExit(Node2D body)
    {
        if (body.IsInGroup(Groups.PLAYERS)) { DetectedPlayer = null; }
    }
}