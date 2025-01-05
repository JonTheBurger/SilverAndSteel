using Godot;
using System.Linq;
using static Godot.Mathf;

namespace Game;

public partial class Enemy : Actor
{
    public Player? Player { get; private set; }
    public Senses? Senses { get; private set; }
    public bool IsDying { get; set; } = false;
    public bool IsPlayerDetected => Senses?.IsPlayerDetected ?? false;
    public bool IsPlayerInRange => Senses?.IsPlayerInRange ?? false;

    public override void _Ready()
    {
        base._Ready();

        Player = GetTree().GetNodesInGroup(Groups.PLAYERS).OfType<Player>().FirstOrDefault();
        Senses = GetNode<Senses>("Senses");
        Animation.AnimationFinished += (StringName name) => {
            if (name == "die")
            {
                QueueFree();
            }
        };
    }

    void MoveTowardsPlayer()
    {
        if (Senses?.DetectedPlayer == null) { return; }
        if (!Senses.IsPlayerDetected) { return; }

        var direction = (Senses.DetectedPlayer.GlobalPosition - GlobalPosition).Normalized();
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

    public void StopMoving()
    {
        Velocity = Velocity.WithX(0);
    }
}
