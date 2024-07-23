using Godot;
using static Godot.Mathf;

using System.Linq;

namespace Game;

public partial class Skeleton : Actor
{
    private Player? _player;
    private SkeletonFsm? _fsm;

    public override void _Ready()
    {
        base._Ready();

        _fsm = GetNode<SkeletonFsm>("Fsm");
        _fsm.Target = this;
        Animation.AnimationFinished += _fsm.OnAnimationFinished;

        _player = GetTree().GetNodesInGroup(Groups.PLAYERS).OfType<Player>().FirstOrDefault();
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);

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

    public bool IsPlayerInRange()
    {
        return (_player != null) && (Abs(_player.GlobalPosition.X - GlobalPosition.X) <= 25);
    }

    public bool IsPlayerOutOfRange()
    {
        return (_player != null) && (Abs(_player.GlobalPosition.X - GlobalPosition.X) > 25);
    }

    public void MoveTowardsPlayer()
    {
        if (_player == null) { return; }

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
}