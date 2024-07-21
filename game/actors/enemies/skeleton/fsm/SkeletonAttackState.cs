using Godot;

using System;

namespace Game;

[Icon("res://assets/img/icons/state.png")]
[Tool]
public partial class SkeletonAttackState : State<Skeleton>
{
    [Export]
    public State<Skeleton>? OnAttackFinished { get; set; }

    [Export]
    public State<Skeleton>? OnHpZero { get; set; }

    [Export]
    public StringName AttackAnimation { get; set; } = "attack";

    [Export]
    public StringName WindupAnimation { get; set; } = "idle";

    public Timer AttackDelayTimer => _attackDelayTimer ??= this.GetNodeOrCreate<Timer>("AttackDelayTimer");
    private Timer? _attackDelayTimer;
    bool _timerStarted;

    public override void _Ready()
    {
        AttackDelayTimer.OneShot = true;
        AttackDelayTimer.Timeout += () => Target?.AnimationPlayer?.Play(AttackAnimation);
    }

    public override void OnEnter(State<Skeleton> previous)
    {
        if (Target == null) { return; }
        Target.AnimationPlayer?.Play(WindupAnimation);
        AttackDelayTimer.Start(10 / Target.Stats.Speed);
        Target.Velocity = Target.Velocity.WithX(0);
    }

    public override void OnExit(State<Skeleton> next)
    {
        AttackDelayTimer.Stop();
        Target?.AnimationPlayer?.Stop();
    }

    public override void ProcessPhysics(double delta)
    {
        if (Target == null) { return; }
        if (Target.Stats.Hp <= 0)
        {
            Next = OnHpZero;
        }
    }

    public override void OnAnimationFinished(StringName animation)
    {
        if (animation == AttackAnimation)
        {
            Next = OnAttackFinished;
        }
    }

    public override string[] _GetConfigurationWarnings()
    {
        if (AttackAnimation == WindupAnimation)
        {
            return new string[] { "Attack Animation and Windup Animation must be different!" };
        }
        return Array.Empty<string>();
    }
}