using System;
using System.Linq;

using Godot;

namespace Game;

[Icon("res://assets/img/icons/fsm.png")]
public partial class Fsm<T> : Node2D where T : Node
{
    public T? Target { get; set; }

    [Export]
    public AnimationPlayer? AnimationPlayer { get; set; }

    [Export]
    private State<T> Current { get; set; } = State<T>.NOP;

    public Label? Label
    {
        get => _label ??= GetNode("Label") as Label;
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
            if (Label != null)
            {
                Label.Visible = value;
            }
        }
    }
    bool _showDebugLabel = false;

    [Export]
    bool LogTransitions { get; set; } = false;

    private State<T> _previous = State<T>.NOP;

    public override void _EnterTree()
    {
        Target ??= GetParent() as T;
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        if (Target != null)
        {
            foreach (var state in GetChildren().OfType<State<T>>())
            {
                state.Target = Target;
                state.AnimationPlayer = AnimationPlayer;
            }
        }
        else if (!Engine.IsEditorHint())
        {
            // In the editor, 'GetParent() as T' only works if T is a [Tool]
            GD.PushWarning(
                $"Fsm {Name}.Target is null!" +
                $" Make a {typeof(T).FullName} its parent," +
                $" or assign a {typeof(T).FullName} to 'Fsm.Target'" +
                "in a parent node's _EnterTree() method!");
        }

        // TODO: We just assume node `"Label"` exists.
        // Label.Text = Current.Name;
        // Label.Visible = ShowDebugLabel;

        ProcessTransition(Current);
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
        ProcessTransition(Current.Next);
    }

    // Checks if a transition is necessary, running the OnExit/OnEnter functions as needed
    private void ProcessTransition(State<T>? next)
    {
        if (next != null)
        {
            if (LogTransitions) { GD.Print($"{Name}: {Current.Name} -> {next.Name}"); }
            if (ShowDebugLabel && Label != null) { Label.Text = next.Name; }

            Current.OnExit(next);
            next.Next = null;
            next.OnEnter(Current);

            _previous = Current;
            Current.Next = null;
            Current = next;
        }
    }

    // Notifies only the active state that an animation has completed
    public void OnAnimationFinished(StringName animation)
    {
        Current.OnAnimationFinished(animation);
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