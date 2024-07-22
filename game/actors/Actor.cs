using Godot;

using static Game.Globals;

namespace Game;

[GlobalClass]
public partial class Actor : CharacterBody2D
{
    public Stats Stats => _stats!;
    public Sprite2D Sprite => _sprite!;
    public AnimationPlayer Animation => _animation!;
    public AudioStreamPlayer2D Audio => _audio!;
    public CollisionShape2D Hurtbox => _hurtbox!;
    public Area2D AttackArea => _attackArea!;
    public CollisionShape2D Hitbox => _hitbox!;

    [Signal]
    public delegate void TurnedAroundEventHandler(bool isFacingRight);

    public override void _Ready()
    {
        base._Ready();
        _stats = GetNode<Stats>("Stats");
        _sprite = GetNode<Sprite2D>("Sprite");
        _animation = GetNode<AnimationPlayer>("Animation");
        _audio = GetNode<AudioStreamPlayer2D>("Audio");
        _hurtbox = GetNode<CollisionShape2D>("Hurtbox");
        _attackArea = GetNode<Area2D>("AttackArea");
        _hitbox = GetNode<CollisionShape2D>("AttackArea/Hitbox");

        AttackArea.BodyEntered += OnAttackHit;
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
        if (JustTurnedAround()) { TurnAround(); }
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

    private void TurnAround()
    {
        _isFacingRight = !_isFacingRight;
        Scale = Scale.WithXFlipped();
        EmitSignal(SignalName.TurnedAround, _isFacingRight);
    }

    private bool JustTurnedAround()
    {
        bool turnedAround = false;

        if ((Velocity.X > 0) && !_isFacingRight)
        {
            turnedAround = true;
        }
        else if ((Velocity.X < 0) && _isFacingRight)
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
            if (_sprite != null) { _sprite.Dispose(); _sprite = null; }
            if (_animation != null) { _animation.Dispose(); _animation = null; }
            if (_audio != null) { _audio.Dispose(); _audio = null; }
            if (_hurtbox != null) { _hurtbox.Dispose(); _hurtbox = null; }
            if (_attackArea != null) { _attackArea.Dispose(); _attackArea = null; }
            if (_hitbox != null) { _hitbox.Dispose(); _hitbox = null; }
        }
    }

    private bool _isFacingRight = true;
    private Stats? _stats;
    private Sprite2D? _sprite;
    private AnimationPlayer? _animation;
    private AudioStreamPlayer2D? _audio;
    private CollisionShape2D? _hurtbox;
    private Area2D? _attackArea;
    private CollisionShape2D? _hitbox;
}