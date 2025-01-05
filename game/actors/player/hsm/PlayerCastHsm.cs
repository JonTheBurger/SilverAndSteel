using Godot;

namespace Game;

[GlobalClass]
[Icon("res://assets/img/icons/state.png")]
public partial class PlayerCastHsm : Hsm<Player>
{
    [Export]
    public StringName Animation { get; set; } = "magic_attack";

    [Export]
    public Hsm<Player>? OnFall { get; set; }

    [Export]
    public Hsm<Player>? OnLand { get; set; }

    protected override void OnEnter()
    {
        Target.Animation?.Play(Animation);
        ((Bolt)Target.Abilities[0].Instantiate()).CastFrom(Target);
    }

    protected override void OnExit()
    {
        Target.Animation?.Stop();
    }

    protected override void OnProcessPhysics(double delta)
    {
        Target.Move();
    }

    protected override void OnAnimationFinished(StringName animation)
    {
        if (Target.IsOnFloor())
        {
            Next = OnLand;
        }
        else
        {
            Next = OnFall;
        }
    }
}