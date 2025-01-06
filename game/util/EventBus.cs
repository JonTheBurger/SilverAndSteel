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
        HpChanged = 1 << 1,
        ScoreChanged = 1 << 2,
    }

    [Export]
    public Kinds Log { get; set; } = 0;

    public Logger Logger => _logger ??= GetNode<Logger>("Logger");
    private Logger? _logger;

    [Signal]
    public delegate void HealthChangedEventHandler(Actor actor, int diff);
    public void OnHpChanged(Actor actor, int diff)
    {
        if ((Log & Kinds.HpChanged) != 0)
        {
            Logger.Trace($"{actor.Name} HP {diff}");
        }
        EmitSignal(SignalName.HealthChanged, actor, diff);
    }

    [Signal]
    public delegate void ScoreChangedEventHandler(Player player);
    public void OnScoreChanged(Player player)
    {
        if ((Log & Kinds.ScoreChanged) != 0)
        {
            Logger.Trace($"{player.Name} Score {player.Score}");
        }
        EmitSignal(SignalName.ScoreChanged, player);
    }
}