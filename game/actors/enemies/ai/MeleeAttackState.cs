using Godot;

namespace Game;

[Icon("res://assets/img/icons/state.png")]
public partial class MeleeAttackState : Hsm<CharacterBody2D>
{
    [Export]
    public StringName Animation { get; set; } = "attack";

    [Export]
    public Hsm<CharacterBody2D>? OnAttackComplete { get; set; }

    protected override void OnEnter()
    {
        Animator?.Play(Animation);
        Target.Velocity = Target.Velocity.WithX(0);
    }

    protected override void OnAnimationFinished(StringName animation)
    {
        Next = OnAttackComplete;
    }
}