using System.Linq;

using Godot;

namespace Game;

public partial class Skeleton : CharacterBody2D, IActor
{
    public Stats Stats
    {
        get => _stats ??= GetNode<Stats>("Stats");
    }
    private Stats? _stats;

    public AnimationPlayer AnimationPlayer
    {
        get => _animationPlayer ??= GetNode<AnimationPlayer>("AnimationPlayer");
        set => _animationPlayer = value;
    }
    private AnimationPlayer? _animationPlayer;

    private Player? Player;

    public SkeletonFsm Fsm
    {
        get => _fsm ??= GetNode<SkeletonFsm>("SkeletonFsm");
        set => _fsm = value;
    }
    private SkeletonFsm? _fsm;

    private Vector2 _direction = Vector2.Right;

    public override void _Ready()
    {
        Stats.HpChanged += OnHpChanged;
        Fsm.Target = this;
        Player = GetTree().GetNodesInGroup(Groups.PLAYERS).OfType<Player>().FirstOrDefault();
    }

    public override void _PhysicsProcess(double delta)
    {
    }

    private void OnHpChanged(int hp)
    {
        if (Stats.Hp <= 0)
        {
            QueueFree();
        }
    }
}
