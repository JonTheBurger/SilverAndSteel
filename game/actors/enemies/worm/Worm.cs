using Godot;
using System;

namespace Game;

public partial class Worm : Actor
{
    [Export]
    public StatSheet StatSheet { get; set; }

    public override void _Ready()
    {
        base._Ready();
    }

    public void MoveTowardsPlayer()
    {
        if (_player == null) { return; }
        if (!IsPlayerDetected) { return;}

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
}
