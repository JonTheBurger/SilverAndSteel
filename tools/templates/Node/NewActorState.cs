using Godot;

namespace Game;

public partial class _CLASS_ : ActorState
{
    // Assign this to Next to notify the state machine to transition
    [Export]
    public ActorState? OnSomeEvent { get; set; }

    // Consider using this.AnimationPlayer?.Play(Animation);
    [Export]
    public StringName Animation { get; set; } = "";

    // Called when the state is about to become the current state.
    public override void OnEnter(ActorState previous)
    {
    }

    // Called when the state is about to become the pervious state
    public override void OnExit(ActorState next)
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
