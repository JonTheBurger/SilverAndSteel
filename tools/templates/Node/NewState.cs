using Godot;

namespace Game;

[Icon("res://assets/img/icons/state.png")]
public partial class _CLASS_ : State<Node>
{
    // Assign this to Next to notify the state machine to transition.
    [Export]
    public State<Node>? OnSomeEvent { get; set; }

    // States often play animations OnEnter and stop them OnExit.
    [Export]
    public StringName Animation { get; set; } = "idle";

    // Called when the state is about to become the current state.
    public override void OnEnter(State<Node> previous)
    {
        // Target.AnimationPlayer?.Play(Animation);
    }

    // Called when the state is about to become the pervious state.
    public override void OnExit(State<Node> next)
    {
        // Target.AnimationPlayer?.Stop();
    }

    // Called when an animation completes while this state is active.
    public override void OnAnimationFinished(StringName animation)
    {
    }

    // Called every graphics frame while this state is active. 'delta' is in seconds.
    public override void Process(double delta)
    {
    }

    // Called every physics frame while this state is active. 'delta' is in seconds.
    public override void ProcessPhysics(double delta)
    {
    }

    // Called when user input is detected and this state is active.
    public override void ProcessInput(InputEvent input)
    {
    }
}
