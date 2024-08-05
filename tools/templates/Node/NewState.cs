using Godot;

namespace Game;

[Icon("res://assets/img/icons/state.png")]
public partial class _CLASS_ : Hsm<Node>
{
    // States often play animations OnEnter and stop them OnExit.
    [Export]
    public StringName Animation { get; set; } = "idle";

    // Assign this to Next to notify the state machine to transition.
    [Export]
    public Hsm<Node>? OnSomeEvent { get; set; }

    // Called once when the state machine is about to start after the parent Init().
    protected override void OnInit()
    {
    }

    // Called when the state is about to become the current state.
    protected override void OnEnter()
    {
        // Animator?.Play(Animation);
    }

    // Called when the state is about to become the pervious state.
    protected override void OnExit()
    {
        // Animator?.Stop();
    }

    // Called when an animation completes while this state is active.
    protected override void OnAnimationFinished(StringName animation)
    {
    }

    // Called when user input is detected and this state is active.
    protected override void OnProcessInput(InputEvent input)
    {
    }

    // Called every graphics frame while this state is active. 'delta' is in seconds.
    protected override void OnProcess(double delta)
    {
    }

    // Called every physics frame while this state is active. 'delta' is in seconds.
    protected override void OnProcessPhysics(double delta)
    {
    }
}
