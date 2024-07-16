using Godot;

namespace Game;

[Icon("res://assets/img/icons/state.png")]
public partial class PlayerJumpState : State<Player>
{
    [Export]
    public State<Player>? OnLand { get; set; }

    [Export]
    public StringName Animation { get; set; } = "jump";

    private float _gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

    public override void OnEnter(State<Player> previous)
    {
        Target.AnimationPlayer?.Play(Animation);
        var velocity = Target.Velocity;
        velocity.Y = -150.0f;
        Target.Velocity = velocity;
    }

    public override void OnExit(State<Player> next)
    {
        Target.AnimationPlayer?.Stop();
    }

    public override void ProcessPhysics(double delta)
    {
        Target.Move();
        Vector2 velocity = Target.Velocity;
        velocity.Y += _gravity * (float)delta;
        Target.Velocity = velocity;
        if (Target.IsOnFloor())
        {
            Next = OnLand;
        }
    }
}