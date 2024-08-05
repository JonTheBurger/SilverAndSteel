using System;
using System.Linq;

using Godot;

namespace Game;

[Icon("res://assets/img/icons/state.png")]
[Tool]
public partial class Hsm<T> : Node where T : Node
{
    [Export]
    public Hsm<T>? Initial { get; set; }

    [Export]
    public AnimationPlayer? Animator { get; set; }

    [Export]
    bool LogTransitions { get; set; } = false;

    /// <summary>
    /// WARNING: Hsm _OnReady fills this in - it will only be null in the constructor
    /// (i.e. when initializing other properties) or _OnReady().
    /// </summary>
    public T Target { get; set; }

    private Hsm<T>? _previous;
    public Hsm<T>? Current { get; set; }
    public Hsm<T>? Next { get; set; }

    public void Start(T target)
    {
        Init(target, Animator);
        if (Animator != null)
        {
            Animator.AnimationFinished += AnimationFinished;
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
        Animator = animator;
        OnInit();

        var children = GetChildren().OfType<Hsm<T>>();
        foreach (var child in children)
        {
            child.Init(target, animator);
        }
    }

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

    protected virtual void OnInit()
    {
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

    public override string[] _GetConfigurationWarnings()
    {
        if (Animator == null)
        {
            return new string[] {
                "No AnimationPlayer was set!" +
                " States will not be able to react to animations finishing!"
            };
        }
        return Array.Empty<string>();
    }
}