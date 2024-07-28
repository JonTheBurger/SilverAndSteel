using Godot;

namespace Game;

[Icon("res://assets/img/icons/state.png")]
public partial class PlayerFallHsm : Hsm<Player>
{
    [Export]
    public Hsm<Player>? OnLand { get; set; }

    [Export]
    public StringName Animation { get; set; } = "jump_fall";

    protected override void OnEnter()
    {
        Target.Animation?.Play(Animation);
    }

    protected override void OnExit()
    {
        Target.Animation?.Stop();
    }

    protected override void OnProcessPhysics(double delta)
    {
        Target.Move();
        if (Target.IsOnFloor())
        {
            Next = OnLand;
        }
    }
}
