using System;
using System.Linq;

using Godot;

namespace Game;

[Icon("res://assets/img/icons/state.png")]
public partial class Hsm<T> : Node where T : Node
{
    [Export]
    bool LogTransitions { get; set; } = false;

    public void Start(T target, AnimationPlayer? animator)
    {
        Init(target, animator);
        if (animator != null)
        {
            animator.AnimationFinished += AnimationFinished;
        }
        Enter();
    }

    public void Stop()
    {
        Current?.Stop();
        Current = null;
        Exit();
    }

    public void Init(T target, AnimationPlayer? animator)
    {
        Target = target;
        AnimationPlayer = animator;

        var children = GetChildren().OfType<Hsm<T>>();
        foreach (var child in children)
        {
            child.Init(target, animator);
        }
    }

    /// <summary>
    /// WARNING: Hsm _OnReady fills this in - it will only be null in the constructor
    /// (i.e. when initializing other properties) or _OnReady().
    /// </summary>
    public T Target
    {
        // get => _target ??= GetParentOrNull<Hsm<T>>()?.Target!;
        get => _target;
        set => _target = value;
    }
    private T? _target;

    public AnimationPlayer? AnimationPlayer
    {
        // get => _animationPlayer ??= GetParentOrNull<Hsm<T>>()?.AnimationPlayer;
        get => _animationPlayer;
        set => _animationPlayer = value;
    }
    private AnimationPlayer? _animationPlayer;

    [Export]
    public Hsm<T>? Initial { get; set; }

    private Hsm<T>? _previous;
    public Hsm<T>? Current { get; set; }
    public Hsm<T>? Next { get; set; }

    public void Enter()
    {
        OnEnter();
        Current = Initial;
        Current?.Enter();
    }
    public void Exit()
    {
        Current?.Exit();
        OnExit();
    }
    public void AnimationFinished(StringName animation)
    {
        OnAnimationFinished(animation);
        Current?.AnimationFinished(animation);
    }
    public void ProcessInput(InputEvent input)
    {
        OnProcessInput(input);
        Current?.ProcessInput(input);
    }
    public void Process(double delta)
    {
        OnProcess(delta);
        Current?.Process(delta);
    }
    public void ProcessPhysics(double delta)
    {
        OnProcessPhysics(delta);
        Current?.ProcessPhysics(delta);
        ProcessTransition();
    }
    public void ProcessTransition()
    {
        if ((Current != null) && (Current.Next != null))
        {
            if (LogTransitions) { GD.Print($"{Name}: {Current.Name} -> {Current.Next.Name}"); }

            _previous = Current;
            Current.Exit();
            Current = Current.Next;
            Current.Next = null;
            Current.OnEnter();
        }
    }


    protected virtual void OnEnter()
    {
    }
    protected virtual void OnExit()
    {
    }
    protected virtual void OnAnimationFinished(StringName animation)
    {
    }
    protected virtual void OnProcessInput(InputEvent input)
    {
    }
    protected virtual void OnProcess(double delta)
    {
    }
    protected virtual void OnProcessPhysics(double delta)
    {
    }
    protected virtual void OnProcessTransition(Hsm<T>? next)
    {
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