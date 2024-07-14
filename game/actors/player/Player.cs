using Godot;

namespace Game;

public partial class Player : CharacterBody2D
{
    #region Properties
    [Export]
    public float Speed { get; set; } = 200.0f;

    [Export]
    public float JumpVelocity { get; set; } = -150.0f;
    #endregion

    #region Nodes
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
    #endregion

    #region Signals
    [Signal]
    public delegate void TurnedAroundEventHandler(bool isFacingRight);
    #endregion

    #region Private
    // Get the gravity from the project settings to be synced with RigidBody nodes.
    private float _gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();
    private Vector2 direction = Vector2.Zero;
    private bool _isFacingRight = true;
    #endregion

    public override void _Ready()
    {
    }

    public override void _PhysicsProcess(double delta)
    {
        Vector2 velocity = Velocity;

        // Add the gravity.
        if (!IsOnFloor())
            velocity.Y += _gravity * (float)delta;

        // Handle Jump.
        if (Input.IsActionJustPressed("jump") && IsOnFloor())
        {
            velocity = Jump(velocity);
        }

        // Get the input direction and handle the movement/deceleration.
        // As good practice, you should replace UI actions with custom gameplay actions.
        direction = Input.GetVector("left", "right", "up", "down");
        if (direction != Vector2.Zero)
        {
            velocity.X = direction.X * Speed;
        }
        else
        {
            velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
        }

        Velocity = velocity;
        if (JustTurnedAround()) { TurnAround(); }
        MoveAndSlide();
    }

    private Vector2 Jump(Vector2 velocity)
    {
        velocity.Y = JumpVelocity;
        return velocity;
    }

    private bool JustTurnedAround()
    {
        bool turnedAround = false;
        if ((direction.X > 0) && !_isFacingRight)
        {
            turnedAround = true;
        }
        else if ((direction.X < 0) && _isFacingRight)
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