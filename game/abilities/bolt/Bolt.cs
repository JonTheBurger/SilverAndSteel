using Godot;

namespace Game;

[Icon("res://assets/img/icons/fire.png")]
public partial class Bolt : Node2D
{
    [Export]
    public float Speed = 100.0f;

    [Export]
    public int Damage = 10;

    public Node2D? Caster;

    public override void _Ready()
    {
        _area2d = GetNode<Area2D>("Area2D");
        _area2d.BodyEntered += OnHit;
        _animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        _animatedSprite?.Play("attack");
        _animatedSprite.AnimationFinished += QueueFree;
        _direction = GetNode<Directional>("Directional");
    }

    public void CastFrom(Actor actor)
    {
        actor.GetParent().AddChild(this);
        Caster = actor;
        GlobalPosition = actor.GlobalPosition;
        _direction.Face(actor.Directional.Facing);
    }

    public override void _PhysicsProcess(double delta)
    {
        var x = Position;
        x.X += (float)delta * Speed * (int)_direction.Facing;
        Position = x;
    }

    private void OnHit(Node2D node)
    {
        if (node == Caster) { return; }
        if (node is Actor actor)
        {
            actor.Stats.ApplyDamage(Damage, actor);
        }
        Speed = 0;
        BlowUp();
    }

    private void BlowUp()
    {
        _animatedSprite?.Play("on_hit");
    }

    private Area2D? _area2d;
    private AnimatedSprite2D? _animatedSprite;
    private Directional? _direction;
}
