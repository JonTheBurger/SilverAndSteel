using Godot;

namespace Game;

public partial class Player : CharacterBody2D, IActor
{
    [Export]
    public float Speed { get; set; } = 200.0f;

    [Export]
    public float JumpVelocity { get; set; } = -150.0f;

    public Sprite2D Sprite2D
    {
        get => _sprite2D ??= GetNode<Sprite2D>("Sprite2D");
        set => _sprite2D = value;
    }
    private Sprite2D? _sprite2D;

    public AnimationPlayer AnimationPlayer
    {
        get => _animationPlayer ??= GetNode<AnimationPlayer>("AnimationPlayer");
        set => _animationPlayer = value;
    }
    private AnimationPlayer? _animationPlayer;

    public PlayerFsm PlayerFsm
    {
        get => _playerFsm ??= GetNode<PlayerFsm>("PlayerFsm");
        set => _playerFsm = value;
    }
    private PlayerFsm? _playerFsm;

    public Area2D Sword
    {
        get => _sword ??= GetNode<Area2D>("Sword");
        set => _sword = value;
    }
    private Area2D? _sword;

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
        Sword.BodyEntered += OnSwordHit;
        PlayerFsm.Target = this;
    }

    public void Move()
    {
        var velocity = Velocity;
        Vector2 direction = Input.GetVector("left", "right", "up", "down");
        if (direction != Vector2.Zero)
        {
            velocity.X = direction.X * Speed;
        }
        else
        {
            velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
        }
        Velocity = velocity;
    }

    public override void _PhysicsProcess(double delta)
    {
        if (JustTurnedAround()) { TurnAround(); }
        MoveAndSlide();
    }

    private void OnSwordHit(Node2D node)
    {
        if (node == this) { return; }
        if (node is IActor actor)
        {
            actor.Stats.CalculateDamage(Stats);
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
        Sprite2D.FlipH = !Sprite2D.FlipH;
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