using Godot;

namespace Game;

public partial class PlayerJumpState : ActorState
{
    [Export]
    public ActorState? OnLand { get; set; }

    [Export]
    public StringName Animation { get; set; } = "jump";

    private float _gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

    public override void OnEnter(ActorState previous)
    {
        AnimationPlayer?.Play(Animation);
        var velocity = Actor.Velocity;
        velocity.Y = -150.0f;
        Actor.Velocity = velocity;
    }

    public override void OnExit(ActorState next)
    {
        AnimationPlayer?.Stop();
    }

    public override void ProcessPhysics(double delta)
    {
        Vector2 velocity = Actor.Velocity;
        velocity.Y += _gravity * (float)delta;
        Actor.Velocity = velocity;
        if (Actor.IsOnFloor())
        {
            Next = OnLand;
        }
    }
}