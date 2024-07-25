using System;

using Godot;

namespace Game;

[Icon("res://assets/img/icons/state.png")]
public partial class Hsm<T> : Node where T : Node
{
    [Export]
    bool LogTransitions { get; set; } = false;

    public void HookUp(T target, AnimationPlayer player)
    {
        Target = target;
        AnimationPlayer = player;
        player.AnimationFinished += OnAnimationFinished;
    }

    /// <summary>
    /// WARNING: Hsm _OnReady fills this in - it will only be null in the constructor
    /// (i.e. when initializing other properties) or _OnReady().
    /// </summary>
    public T Target
    {
        get => _target ??= GetParent<Hsm<T>>().Target!;
        set => _target = value;
    }
    private T? _target;

    public AnimationPlayer? AnimationPlayer
    {
        // get => _animationPlayer ??= GetParent<Hsm<T>>().AnimationPlayer;
        get => _animationPlayer;
        set => _animationPlayer = value;
    }
    private AnimationPlayer? _animationPlayer;

    [Export]
    public Hsm<T>? Initial { get; set; }

    private Hsm<T>? _previous;
    private Hsm<T>? Current { get; set; }
    public Hsm<T>? Next { get; set; } = null;

    /// <summary>
    /// Called when the state is about to become the current state.
    /// </summary>
    /// <param name="previous">Last active state.</param>
    public virtual void OnEnter(Hsm<T> previous)
    {
        Current = Initial;
        Current?.OnEnter(previous);
    }

    /// <summary>
    /// Called when the state is about to become the pervious state.
    /// </summary>
    /// <param name="next">Next active state.</param>
    public virtual void OnExit(Hsm<T> next)
    {
        Current?.OnExit(next);
    }

    /// <summary>
    /// Called when an animation completes while this state is active.
    /// </summary>
    /// <param name="animation">Name of the completed animation.</param>
    public virtual void OnAnimationFinished(StringName animation)
    {
        Current?.OnAnimationFinished(animation);
    }

    /// <summary>
    /// Called every graphics frame while this state is active.
    /// </summary>
    /// <param name="delta">Elapsed time since the previous frame in seconds.</param>
    public virtual void Process(double delta)
    {
        Current?.Process(delta);
    }

    /// <summary>
    /// Called every physics frame while this state is active.
    /// </summary>
    /// <param name="delta">
    /// Elapsed time since the previous frame in seconds; should be constant
    /// </param>
    public virtual void ProcessPhysics(double delta)
    {
        Current?.ProcessPhysics(delta);
        ProcessTransition(Current?.Next);
    }

    /// <summary>
    /// Called when user input is detected and this state is active.
    /// </summary>
    /// <param name="input">User input.</param>
    public virtual void ProcessInput(InputEvent input)
    {
        Current?.ProcessInput(input);
    }

    // Checks if a transition is necessary, running the OnExit/OnEnter functions as needed
    private void ProcessTransition(Hsm<T>? next)
    {
        if (next != null)
        {
            if (LogTransitions) { GD.Print($"{Name}: {Current?.Name} -> {next.Name}"); }

            Current?.OnExit(next);
            next.Next = null;
            next.OnEnter(Current);

            _previous = Current;
            Current.Next = null;
            Current = next;
        }
    }

    public override string[] _GetConfigurationWarnings()
    {
        if (AnimationPlayer == null)
        {
            return new string[] {
                "No AnimationPlayer was set!" +
                " States will not be able to react to animations finishing!"
            };
        }
        return Array.Empty<string>();
    }
}