using Godot;

namespace Game;

[Icon("res://icon.svg")]
public partial class ActorState : Node
{
    public CharacterBody2D CharacterBody2D
    {
        get => _characterBody2D ??= GetParent<ActorFsm>().CharacterBody2D;
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
    /// Parent node, which must always be an instance of <see cref="ActorFsm"/>.
    /// </summary>
    public ActorFsm Machine => _machine ??= GetParent<ActorFsm>();
    private ActorFsm? _machine;

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
