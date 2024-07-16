using Godot;

namespace Game;

[Icon("res://assets/img/icons/state.png")]
public partial class ActorState : Node
{
    public CharacterBody2D Actor
    {
        get => _characterBody2D ??= GetParent<ActorFsm>().Actor;
        set => _characterBody2D = value;
    }
    private CharacterBody2D? _characterBody2D;

    public AnimationPlayer AnimationPlayer
    {
        get => _animationPlayer ??= GetParent<ActorFsm>().AnimationPlayer;
        set => _animationPlayer = value;
    }
    private AnimationPlayer? _animationPlayer;

    /// <summary>
    /// Set this to transition on next ActorFsm _PhysicsProcess
    /// </summary>
    public ActorState? Next { get; set; } = null;

    /// <summary>
    /// Called when the state is about to become the current state.
    /// </summary>
    /// <param name="previous">Last active state.</param>
    public virtual void OnEnter(ActorState previous)
    {
    }

    /// <summary>
    /// Called when the state is about to become the pervious state.
    /// </summary>
    /// <param name="next">Next active state.</param>
    public virtual void OnExit(ActorState next)
    {
    }

    /// <summary>
    /// Called when an animation completes while this state is active.
    /// </summary>
    /// <param name="animation">Name of the completed animation.</param>
    public virtual void OnAnimationFinished(StringName animation)
    {
    }

    /// <summary>
    /// Called every graphics frame while this state is active.
    /// </summary>
    /// <param name="delta">Elapsed time since the previous frame in seconds.</param>
    public virtual void Process(double delta)
    {
    }

    /// <summary>
    /// Called every physics frame while this state is active.
    /// </summary>
    /// <param name="delta">
    /// Elapsed time since the previous frame in seconds; should be constant
    /// </param>
    public virtual void ProcessPhysics(double delta)
    {
    }

    /// <summary>
    /// Called when user input is detected and this state is active.
    /// </summary>
    /// <param name="input">User input.</param>
    public virtual void ProcessInput(InputEvent input)
    {
    }

    /// <summary>
    /// A NO-OP state; used for "Null Object Pattern" to avoid null checks.
    /// </summary>
    public static ActorState NOP => s_null ??= new ActorState();
    private static ActorState? s_null = null;
}