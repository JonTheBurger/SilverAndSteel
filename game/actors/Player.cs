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
        get => this.OnReady("Sprite2D", ref _sprite2D);
        set => _sprite2D = value;
    }
    private Sprite2D? _sprite2D;

    [Signal]
    public delegate void TurnedAroundEventHandler(bool isFacingRight);

    // Get the gravity from the project settings to be synced with RigidBody nodes.
    private float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();
    private bool isFacingRight = true;

    public override void _PhysicsProcess(double delta)
    {
        Vector2 velocity = Velocity;

        // Add the gravity.
        if (!IsOnFloor())
            velocity.Y += gravity * (float)delta;

        // Handle Jump.
        if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
            velocity.Y = JumpVelocity;

        // Get the input direction and handle the movement/deceleration.
        // As good practice, you should replace UI actions with custom gameplay actions.
        Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
        if (direction != Vector2.Zero)
        {
            velocity.X = direction.X * Speed;
            if (direction.X > 0)
            {
                if (!isFacingRight)
                {
                    isFacingRight = true;
                    EmitSignal(SignalName.TurnedAround, isFacingRight);
                }
            }
            else
            {
                if (isFacingRight)
                {
                    isFacingRight = false;
                    EmitSignal(SignalName.TurnedAround, isFacingRight);
                }
            }
        }
        else
        {
            velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
        }

        Velocity = velocity;
        MoveAndSlide();
    }
}
