using Godot;

namespace Game;

[Icon("res://assets/img/icons/state.png")]
public partial class SkeletonAliveHsm : Hsm<Skeleton>
{
    [Export]
    public Hsm<Skeleton>? OnDie { get; set; }

    protected override void OnProcessPhysics(double delta)
    {
        if ((Target.Stats.Hp <= 0) && (Current != OnDie) && (Next != OnDie))
        {
            Next = OnDie;
        }
    }
}