using Godot;

namespace Game;

public partial class PlayerAttackState : State<Player>
{

    [Export]
    public StringName Animation { get; set; } = "attack";

    private State<Player>? _previous;

    public override void OnEnter(State<Player> previous)
    {
        _previous = previous;
        Target.AnimationPlayer?.Play(Animation);
    }

    public override void OnAnimationFinished(StringName animation)
    {
        Next = _previous;
    }
}