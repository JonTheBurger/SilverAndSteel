using System.Linq;

using Godot;

namespace Game;

[Icon("res://assets/img/icons/state.png")]
public partial class AliveState : Hsm<CharacterBody2D>
{
    [Export]
    public Directional? Directional { get; set; }

    [Export]
    public Hsm<CharacterBody2D>? OnDie { get; set; }

    private Player? _player;
    private Stats? _stats;

    public override void _Ready()
    {
        base._Ready();
        _player = GetTree().GetNodesInGroup(Groups.PLAYERS).OfType<Player>().FirstOrDefault();
    }

    protected override void OnInit()
    {
        Target.LoadNodeOrWarn(ref _stats);
    }

    protected override void OnProcessPhysics(double delta)
    {
        if ((Directional != null) && (_player != null) && Directional.IsFacing(_player))
        {
            Directional.Flip();
        }

        if (_stats.Hp <= 0)
        {
            Next = OnDie;
        }
    }
}
