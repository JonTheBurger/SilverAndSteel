using Godot;

namespace Game;

public partial class PlayerIdleState : ActorState
{
    [Export]
    public ActorState? OnMove { get; set; }

    [Export]
    public string Animation { get; set; } = "idle";

    public override void OnEnter(ActorState previous)
    {
        AnimationPlayer?.Play(Animation);
    }

    public override void OnExit(ActorState next)
    {
        AnimationPlayer?.Stop();
    }

    public override void ProcessInput(InputEvent input)
    {
        if (Input.IsActionJustPressed("left"))
        {
            Machine.Next = OnMove;
        }
        else if (Input.IsActionJustPressed("right"))
        {
            Machine.Next = OnMove;
        }
    }
}
