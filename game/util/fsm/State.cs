using Godot;

namespace Game;

[Icon("res://assets/img/icons/state.png")]
public partial class State<T> : Node where T : Node
{
    /// <summary>
    /// WARNING: Fsm _OnReady fills this in - it will only be null in the constructor
    /// (i.e. when initializing other properties) or _OnReady().
    /// </summary>
    public T Target
    {
        get => _target ??= GetParent<Fsm<T>>().Target!;
        set => _target = value;
    }
    private T? _target;

    /// <summary>
    /// WARNING: Fsm _OnReady fills this in - it will only be null in the constructor
    /// (i.e. when initializing other properties) or _OnReady().
    /// </summary>
    public AnimationPlayer? AnimationPlayer
    {
        get => _animationPlayer ??= GetParent<Fsm<T>>().AnimationPlayer;
        set => _animationPlayer = value;
    }
    private AnimationPlayer? _animationPlayer;

    public State<T>? Next { get; set; } = null;

    /// <summary>
    /// Called when the state is about to become the current state.
    /// </summary>
    /// <param name="previous">Last active state.</param>
    public virtual void OnEnter(State<T> previous)
    {
    }

    /// <summary>
    /// Called when the state is about to become the pervious state.
    /// </summary>
    /// <param name="next">Next active state.</param>
    public virtual void OnExit(State<T> next)
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
    public static State<T> NOP => s_null ??= new State<T>();
    private static State<T>? s_null = null;
}