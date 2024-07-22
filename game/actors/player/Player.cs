using Godot;
using static Game.Globals;

namespace Game;

public partial class Player : CharacterBody2D, IActor
{
    [Export]
    public float JumpVelocity { get; set; } = -150.0f;

    public Node2D Direction => GetNode<Node2D>("Directional");

    public Sprite2D Sprite2D
    {
        get => _sprite2D ??= GetNode<Sprite2D>("Directional/Sprite2D");
        set => _sprite2D = value;
    }
    private Sprite2D? _sprite2D;

    public AnimationPlayer AnimationPlayer
    {
        get => _animationPlayer ??= GetNode<AnimationPlayer>("Directional/AnimationPlayer");
        set => _animationPlayer = value;
    }
    private AnimationPlayer? _animationPlayer;

    public PlayerFsm PlayerFsm
    {
        get => _playerFsm ??= GetNode<PlayerFsm>("PlayerFsm");
        set => _playerFsm = value;
    }
    private PlayerFsm? _playerFsm;

    public Area2D Weapon
    {
        get => _weapon ??= GetNode<Area2D>("Directional/Weapon");
        set => _weapon = value;
    }
    private Area2D? _weapon;

    public CollisionShape2D Hitbox
    {
        get => _hitbox ??= GetNode<CollisionShape2D>("Directional/Weapon/Hitbox");
        set => _hitbox = value;
    }
    private CollisionShape2D? _hitbox;

    public AudioStreamPlayer2D AudioStreamPlayer2D => _audioStreamPlayer2D ??= this.GetNodeOrCreate<AudioStreamPlayer2D>("AudioStreamPlayer2D");
    private AudioStreamPlayer2D? _audioStreamPlayer2D;

    public AudioLibrary AudioLibrary => _audioLibrary ??= this.GetNodeOrCreate<AudioLibrary>("AudioLibrary");
    private AudioLibrary? _audioLibrary;

    public Stats Stats
    {
        get => _stats ??= GetNode<Stats>("Stats");
    }
    private Stats? _stats;

    [Signal]
    public delegate void TurnedAroundEventHandler(bool isFacingRight);

    private bool _isFacingRight = true;

    public override void _Ready()
    {
        Weapon.BodyEntered += OnWeaponHit;
        PlayerFsm.Target = this;
    }

    public void Move()
    {
        var velocity = Velocity;
        Vector2 direction = Input.GetVector("left", "right", "up", "down");
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

    private void OnWeaponHit(Node2D node)
    {
        if (node == this) { return; }
        if (node is IActor actor)
        {
            int change = actor.Stats.ApplyDamage(Stats);
            Global.EventBus.OnHpChanged((CharacterBody2D)actor, change);
        }
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

    private void TurnAround()
    {
        _isFacingRight = !_isFacingRight;
        Direction.Scale = Direction.Scale.WithXFlipped();
        EmitSignal(SignalName.TurnedAround, _isFacingRight);
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        if (disposing)
        {
            if (_sprite2D != null) { _sprite2D.Dispose(); _sprite2D = null; }
            if (_animationPlayer != null) { _animationPlayer.Dispose(); _animationPlayer = null; }
        }
    }
}