using Godot;

namespace Game;

[Icon("res://assets/img/icons/state.png")]
public partial class PlayerAttackState : State<Player>
{
    [Export]
    public StringName Animation { get; set; } = "attack";

    [Export]
    public State<Player>? OnAttackComplete { get; set; }

    public override void OnEnter(State<Player> previous)
    {
        Target.Animation?.Play(Animation);
        Target.Velocity = Target.Velocity.WithX(0);
    }

    public override void OnAnimationFinished(StringName animation)
    {
        Next = OnAttackComplete;
    }
}