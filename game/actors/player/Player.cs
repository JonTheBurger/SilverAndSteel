using Godot;

namespace Game;

public partial class Player : CharacterBody2D
{
    [Export]
    public float Speed { get; set; } = 200.0f;

    [Export]
    public float JumpVelocity { get; set; } = -150.0f;

    [Export]
    public Sprite2D Sprite2D
    {
        get => _sprite2D ??= GetNode<Sprite2D>("Sprite2D");
        set => _sprite2D = value;
    }
    private Sprite2D? _sprite2D;

    [Export]
    public AnimationPlayer AnimationPlayer
    {
        get => _animationPlayer ??= GetNode<AnimationPlayer>("AnimationPlayer");
        set => _animationPlayer = value;
    }
    private AnimationPlayer? _animationPlayer;

    [Export]
    public ActorFsm ActorFsm
    {
        get => _actorFsm ??= GetNode<ActorFsm>("ActorFsm");
        set => _actorFsm = value;
    }
    private ActorFsm? _actorFsm;

    [Signal]
    public delegate void TurnedAroundEventHandler(bool isFacingRight);

    private bool _isFacingRight = true;

    public override void _PhysicsProcess(double delta)
    {
        // Vector2 direction = Input.GetVector("left", "right", "up", "down");
        // if (direction != Vector2.Zero)
        // {
        //     velocity.X = direction.X * Speed;
        // }
        // else
        // {
        //     velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
        // }

        if (JustTurnedAround()) { TurnAround(); }
        MoveAndSlide();
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
            if (_actorFsm != null) { _actorFsm.Dispose(); _actorFsm = null; }
        }
    }
}