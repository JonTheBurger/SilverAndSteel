using Godot;

namespace Game;

[Icon("res://assets/img/icons/state.png")]
public partial class SkeletonIdleState : State<Skeleton>
{
    [Export]
    public State<Skeleton>? OnPlayerInRange { get; set; }

    [Export]
    public State<Skeleton>? OnPlayerOutOfRange { get; set; }

    [Export]
    public State<Skeleton>? OnHpZero { get; set; }

    [Export]
    public StringName Animation { get; set; } = "idle";

    public Timer AttackDelayTimer => _attackDelayTimer ??= this.GetNodeOrCreate<Timer>("AttackDelayTimer");
    private Timer? _attackDelayTimer;
    bool _timerStarted;

    public override void _Ready()
    {
        AttackDelayTimer.Timeout += () => 
        {
            GD.Print("Timer Done!");
            _timerStarted = true;
        };
    }

    public override void OnEnter(State<Skeleton> previous)
    {
        Target.AnimationPlayer?.Play(Animation);
    }

    public override void OnExit(State<Skeleton> next)
    {
        Target.AnimationPlayer?.Stop();
        AttackDelayTimer.Stop();
        _timerStarted = false;
    }

    public override void ProcessPhysics(double delta)
    {
        if (Target.Stats.Hp <= 0)
        {
            Next = OnHpZero;
        }
        else if (Target.IsPlayerOutOfRange())
        {
            Next = OnPlayerOutOfRange;
        }
        else if (Target.IsPlayerInRange())
        {
            if (!_timerStarted)
            {
                if (AttackDelayTimer.IsStopped())
                {
                    GD.Print("Waiting to attack");
                    AttackDelayTimer.Start(100.0 / Target.Stats.Speed);
                }
            }
            else if (AttackDelayTimer.TimeLeft <= 0)
            {
                GD.Print("Attacking");
                Next = OnPlayerInRange;
            }
        }
    }
}