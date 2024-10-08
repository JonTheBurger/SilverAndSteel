using System.Dynamic;

using Godot;

using static Game.Globals;

namespace Game;

[GlobalClass]
[Icon("res://assets/img/icons/actor.png")]
public partial class Actor : CharacterBody2D
{
    [Export]
    public float Gravity { get; set; } = ProjectSettings.GetSetting("physics/2d/default_gravity").As<float>();

    [Export]
    public PackedScene Magic { get; set; }

    public Stats Stats => _stats!;
    public AnimationPlayer Animation => _animation!;
    public AudioStreamPlayer2D Audio => _audio!;
    public Directional Directional => _directional!;
    public Sprite2D Sprite => _sprite!;
    public Area2D Hitbox => _hitbox!;
    public CollisionShape2D Hurtbox => _hurtbox!;

    public override void _Ready()
    {
        base._Ready();

        _stats = GetNode<Stats>("Stats");
        _animation = GetNode<AnimationPlayer>("Animation");
        _audio = GetNode<AudioStreamPlayer2D>("Audio");
        _directional = GetNode<Directional>("Directional");
        _sprite = GetNode<Sprite2D>("Directional/Sprite");
        _hitbox = GetNode<Area2D>("Directional/Hitbox");
        _hurtbox = GetNode<CollisionShape2D>("Hurtbox");

        Hitbox.BodyEntered += OnAttackHit;
    }

    public void Move(Vector2 direction)
    {
        var velocity = Velocity;
        if (direction != Vector2.Zero)
        {
            velocity.X = direction.X * Stats.Speed;
        }
        else
        {
            velocity.X = Mathf.MoveToward(Velocity.X, 0, Stats.Speed);
        }
        Velocity = velocity;
    }

    public override void _PhysicsProcess(double delta)
    {
        Velocity = Velocity.WithY(Velocity.Y + (float)delta * Gravity);
        if (JustTurnedAround()) { Directional.Flip(); }
        MoveAndSlide();
    }

    private void OnAttackHit(Node2D node)
    {
        if (node == this) { return; }
        if (node is Actor actor)
        {
            int change = actor.Stats.ApplyDamage(Stats);
            Global.EventBus.OnHpChanged(actor, change);
        }
    }

    private bool JustTurnedAround()
    {
        bool turnedAround = false;

        if ((Velocity.X > 0) && (Directional.Facing == Direction.Left))
        {
            turnedAround = true;
        }
        else if ((Velocity.X < 0) && (Directional.Facing == Direction.Right))
        {
            turnedAround = true;
        }

        return turnedAround;
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        if (disposing)
        {
            if (_stats != null) { _stats.Dispose(); _stats = null; }
            if (_animation != null) { _animation.Dispose(); _animation = null; }
            if (_audio != null) { _audio.Dispose(); _audio = null; }
            if (_directional != null) { _directional.Dispose(); _directional = null; }
            if (_sprite != null) { _sprite.Dispose(); _sprite = null; }
            if (_hitbox != null) { _hitbox.Dispose(); _hitbox = null; }
            if (_hurtbox != null) { _hurtbox.Dispose(); _hurtbox = null; }
        }
    }

    private Stats? _stats;
    private AnimationPlayer? _animation;
    private AudioStreamPlayer2D? _audio;
    private Directional? _directional;
    private Sprite2D? _sprite;
    private Area2D? _hitbox;
    private CollisionShape2D? _hurtbox;
}