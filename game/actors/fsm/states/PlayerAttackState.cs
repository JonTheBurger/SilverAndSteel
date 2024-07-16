using Godot;

namespace Game;

public partial class PlayerAttackState : ActorState
{

    [Export]
    public StringName Animation { get; set; } = "attack";

    private ActorState? _previous;

    public override void OnEnter(ActorState previous)
    {
        _previous = previous;
        AnimationPlayer?.Play(Animation);
    }

    public override void OnAnimationFinished(StringName animation)
    {
        Next = _previous;
    }
}