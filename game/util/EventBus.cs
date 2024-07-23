using System;

using Godot;

namespace Game;

[Icon("res://assets/img/icons/connection.png")]
public partial class EventBus : Node
{
    [Flags]
    public enum Kinds
    {
        None = 0,
        HpChanged = 1 << 0,
    }

    [Export]
    public Kinds Log { get; set; }

    public Logger Logger => _logger ??= GetNode<Logger>("Logger");
    private Logger? _logger;

    [Signal]  // TODO: Use Actor when implemented
    public delegate void HealthChangedEventHandler(CharacterBody2D actor, int diff);
    public void OnHpChanged(CharacterBody2D actor, int diff)
    {
        if ((Log & Kinds.HpChanged) != 0)
        {
            Logger.Trace($"{actor.Name} HP {diff}");
        }
        EmitSignal(SignalName.HealthChanged, actor, diff);
    }
}