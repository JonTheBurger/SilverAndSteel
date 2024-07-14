using Godot;
using System;
using System.Diagnostics;
using System.Linq;

namespace Game;

[Icon("res://icon.svg")]
public partial class ActorFsm : Node2D
{
    [Export]
    public CharacterBody2D CharacterBody2D
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

    /// <summary>
    /// Set this to transition on next _PhysicsProcess
    /// </summary>
    public ActorState? Next { get; set; } = null;

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

    ActorState _previous = ActorState.NOP;
    ActorState[] _states = Array.Empty<ActorState>();

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _states = GetChildren().OfType<ActorState>().ToArray();
        foreach (var state in _states)
        {
            state.CharacterBody2D = CharacterBody2D;
            state.AnimationPlayer = AnimationPlayer;
        }
        Label.Text = $"State: {Current.Name}";
        Label.Visible = ShowDebugLabel;
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
        if (Next != null)
        {
            Transition();
        }
    }

    // Called when user input is detected; we only want States to process when they are active
    public override void _Input(InputEvent @event)
    {
        Current.ProcessInput(@event);
    }

    private void Transition()
    {
        Debug.Assert(Next != null);
        Debug.Assert(ReferenceEquals(Next.Machine, this));
        if (LogTransitions) { GD.Print($"{Name}: {Current.Name} -> {Next.Name}"); }

        Current.OnExit(Next);
        Next.OnEnter(Current);

        _previous = Current;
        Current = Next;
        Next = null;

        if (ShowDebugLabel) { Label.Text = $"State: {Current.Name}"; }
    }
}