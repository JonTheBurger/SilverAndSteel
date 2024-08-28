using Godot;

using static Game.Globals;

namespace Game;

public partial class Bolt : Node2D
{
    [Export]
    public float Speed = 100.0f;

    public Node2D? Source;

    public override void _Ready()
    {
        _area2d = GetNode<Area2D>("Area2D");
        _area2d.BodyEntered += OnHit;
        _animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        _animatedSprite?.Play("attack");
        _animatedSprite.AnimationFinished += QueueFree;
    }

    public override void _PhysicsProcess(double delta)
    {
        var x = Position;
        x.X += (float)delta * Speed;
        this.Position = x;
    }

    private void OnHit(Node2D node)
    {
        if (node == Source) { return; }
        if (node is Actor actor)
        {
            int change = actor.Stats.ApplyDamage(new Stats());
            Global.EventBus.OnHpChanged(actor, change);
            _blowUp();
        }
        else if (node is CollisionObject2D collider && (collider.CollisionLayer & (uint)CollisionLayers.Ground) != 0)
        {
            _blowUp();
        }
    }

    private void _blowUp()
    {
        _animatedSprite?.Play("on_hit");
    }

    private Area2D? _area2d;
    private AnimatedSprite2D? _animatedSprite;
}
