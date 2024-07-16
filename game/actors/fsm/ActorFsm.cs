using System;
using System.Linq;

using Godot;

namespace Game;

[Icon("res://assets/img/icons/fsm.png")]
public partial class ActorFsm : Node2D
{
    [Export]
    public CharacterBody2D Actor
    {
        get => _characterBody2D ??= GetParent<CharacterBody2D>();
        set => _characterBody2D = value;
    }
    private CharacterBody2D? _characterBody2D;

    [Export]
    public AnimationPlayer AnimationPlayer
    {
        get => _animationPlayer ??= GetNode<AnimationPlayer>("../AnimationPlayer");
        set => _animationPlayer = value;
    }
    private AnimationPlayer? _animationPlayer;

    [Export]
    private ActorState Current { get; set; } = ActorState.NOP;

    public Label Label
    {
        get => _label ??= GetNode<Label>("Label");
        set => _label = value;
    }
    private Label? _label;

    [Export]
    bool ShowDebugLabel
    {
        get => _showDebugLabel;
        set
        {
            _showDebugLabel = value;
            Label.Visible = value;
        }
    }
    bool _showDebugLabel = false;

    [Export]
    bool LogTransitions { get; set; } = false;

    private ActorState _previous = ActorState.NOP;
    private ActorState[] _states = Array.Empty<ActorState>();

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _states = GetChildren().OfType<ActorState>().ToArray();
        foreach (var state in _states)
        {
            state.Actor = Actor;
            state.AnimationPlayer = AnimationPlayer;
        }
        if (AnimationPlayer != null) { AnimationPlayer.AnimationFinished += OnAnimationFinished; }
        Label.Text = Current.Name;
        Label.Visible = ShowDebugLabel;
    }

    // Called when user input is detected; we only want States to process when they are active
    public override void _Input(InputEvent @event)
    {
        Current.ProcessInput(@event);
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        Current.Process(delta);
    }

    // Physics processing means that the frame rate is synced to the physics, i.e. the delta variable should be constant. delta is in seconds.
    public override void _PhysicsProcess(double delta)
    {
        Current.ProcessPhysics(delta);
        ProcessTransition();
    }

    // Checks if a transition is necessary, running the OnExit/OnEnter functions as needed
    private void ProcessTransition()
    {
        ActorState? next = Current.Next;
        if (next != null)
        {
            if (LogTransitions) { GD.Print($"{Name}: {Current.Name} -> {next.Name}"); }
            if (ShowDebugLabel) { Label.Text = next.Name; }

            Current.OnExit(next);
            next.Next = null;
            next.OnEnter(Current);

            _previous = Current;
            Current.Next = null;
            Current = next;
        }
    }

    // Notifies only the active state that an animation has completed
    private void OnAnimationFinished(StringName animation)
    {
        Current.OnAnimationFinished(animation);
    }
}