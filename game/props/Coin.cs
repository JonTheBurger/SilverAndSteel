using Godot;
using static Game.Globals;

namespace Game;

public partial class Coin : CharacterBody2D
{
#nullable disable
    public AnimationPlayer Animation { get; private set; }
    public Area2D Area2D { get; private set; }
#nullable enable

    [Export]
    public float Gravity { get; set; } = ProjectSettings.GetSetting("physics/2d/default_gravity").As<float>();

    public override void _Ready()
    {
        Animation = GetNode<AnimationPlayer>("Animation");
        Area2D = GetNode<Area2D>("Area2D");

        Animation.Play("loop");
        Animation.AnimationFinished += (_) => QueueFree();
        Area2D.BodyEntered += (body) =>
        {
            if (body is Player player)
            {
                foreach (var child in Area2D.GetChildren())
                {
                    child.QueueFree();
                }
                Animation.Play("collect");
                Gravity *= -0.3f;
                player.Score += 100;
                Global.EventBus.OnScoreChanged(player);
            }
        };
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        Velocity = Velocity.WithY(Velocity.Y + (float)delta * Gravity);
        MoveAndSlide();
    }
}
