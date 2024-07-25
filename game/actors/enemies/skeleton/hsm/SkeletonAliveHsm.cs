using Godot;

namespace Game;

[Icon("res://assets/img/icons/state.png")]
public partial class SkeletonAliveHsm : Hsm<Skeleton>
{
    [Export]
    public Hsm<Skeleton>? OnDie { get; set; }

    public override void ProcessPhysics(double delta)
    {
        base.ProcessPhysics(delta);

        if (Target.Stats.Hp <= 0)
        {
            Next = OnDie;
        }
    }
}