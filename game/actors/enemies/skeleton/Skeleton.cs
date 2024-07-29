using Godot;
using static Godot.Mathf;

using System.Linq;

namespace Game;

public partial class Skeleton : Actor
{
    public Area2D AggressionRange => _aggressionRange!;
    public Area2D DetectionRadius => _detectionRadius!;
    public bool IsPlayerInRange => _isPlayerInRange;
    public bool IsPlayerDetected => _isPlayerDetected;

    private Player? _player;
    private SkeletonHsm? _fsm;
    private Area2D? _aggressionRange;
    private Area2D? _detectionRadius;
    private bool _isPlayerInRange = false;
    private bool _isPlayerDetected = false;

    public override void _Ready()
    {
        base._Ready();

        _player = GetTree().GetNodesInGroup(Groups.PLAYERS).OfType<Player>().FirstOrDefault();
        _aggressionRange = GetNode<Area2D>("Directional/AggressionRange");
        _detectionRadius = GetNode<Area2D>("Directional/DetectionRadius");
        AggressionRange.BodyEntered += OnAggressionRangeEnter;
        AggressionRange.BodyExited += OnAggressionRangeExit;
        DetectionRadius.BodyEntered += OnDetectionRadiusEnter;
        DetectionRadius.BodyExited += OnDetectionRadiusExit;

        _fsm = GetNode<SkeletonHsm>("SkeletonHsm");
        _fsm.Start(this, Animation);
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        _fsm.ProcessPhysics(delta);

        if (_player != null)
        {
            if ((_player.GlobalPosition.X < GlobalPosition.X) && (Directional.Facing == Direction.Right))
            {
                Directional.Flip();
            }
            else if ((_player.GlobalPosition.X > GlobalPosition.X) && (Directional.Facing == Direction.Left))
            {
                Directional.Flip();
            }
        }
    }

    public void Stop()
    {
        Velocity = Velocity.WithX(0);
    }

    public void MoveTowardsPlayer()
    {
        if (_player == null) { return; }
        if (!_isPlayerDetected) { return;}

        var direction = (_player.GlobalPosition - GlobalPosition).Normalized();
        var velocity = Velocity;
        if (direction != Vector2.Zero)
        {
            velocity.X = direction.X * Stats.Speed;
        }
        else
        {
            velocity.X = MoveToward(Velocity.X, 0, Stats.Speed);
        }
        Velocity = velocity;
    }

    public void OnHpChanged(int hp)
    {
        if (Stats.Hp <= 0)
        {
            QueueFree();
        }
    }

    private void OnDetectionRadiusEnter(Node2D body)
    {
        if (body == _player)
        {
            _isPlayerDetected = true;
        }
    }

    private void OnDetectionRadiusExit(Node2D body)
    {
        if (body == _player)
        {
            _isPlayerDetected = false;
        }
    }

    private void OnAggressionRangeEnter(Node2D body)
    {
        if (body == _player) { _isPlayerInRange = true; }
    }

    private void OnAggressionRangeExit(Node2D body)
    {
        if (body == _player) { _isPlayerInRange = false; }
    }
}