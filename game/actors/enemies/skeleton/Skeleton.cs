using Godot;

namespace Game;

public partial class Skeleton : CharacterBody2D, IActor
{
    public Stats Stats
    {
        get => _stats ??= GetNode<Stats>("Stats");
    }
    private Stats? _stats;

    public override void _Ready()
    {
        Stats.HpChanged += OnHpChanged;
    }

    private void OnHpChanged(int hp)
    {
        if (Stats.Hp <= 0)
        {
            QueueFree();
        }
    }
}
