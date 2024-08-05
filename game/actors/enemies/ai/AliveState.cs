using System.Linq;

using Godot;

namespace Game;

[Icon("res://assets/img/icons/state.png")]
public partial class AliveState : Hsm<CharacterBody2D>
{
    [Export]
    public Hsm<CharacterBody2D>? OnDie { get; set; }

    private Stats? _stats;

    protected override void OnInit()
    {
        Target.LoadNodeOrWarn(ref _stats);
    }

    protected override void OnProcessPhysics(double delta)
    {
        if (_stats.Hp <= 0)
        {
            Next = OnDie;
        }
    }
}
