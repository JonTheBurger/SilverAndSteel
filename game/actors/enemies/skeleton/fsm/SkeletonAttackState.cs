using Godot;

namespace Game;

[Icon("res://assets/img/icons/state.png")]
public partial class SkeletonAttackState : State<Skeleton>
{
    [Export]
    public State<Skeleton>? OnAttackFinished { get; set; }

    [Export]
    public State<Skeleton>? OnHpZero { get; set; }

    [Export]
    public StringName Animation { get; set; } = "attack";

    public Timer AttackDelayTimer => _attackDelayTimer ??= this.GetNodeOrCreate<Timer>("AttackDelayTimer");
    private Timer? _attackDelayTimer;

    public override void OnEnter(State<Skeleton> previous)
    {
        Target.AnimationPlayer?.Play(Animation);
        Target.Velocity = Target.Velocity.WithX(0);
    }

    public override void OnExit(State<Skeleton> next)
    {
        Target.AnimationPlayer?.Stop();
    }

    public override void ProcessPhysics(double delta)
    {
        if (Target.Stats.Hp <= 0)
        {
            Next = OnHpZero;
        }
    }

    public override void OnAnimationFinished(StringName animation)
    {
        Next = OnAttackFinished;
    }
}