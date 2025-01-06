using Godot;
using static Game.Globals;
using static System.Math;

namespace Game;


[GlobalClass]
[Tool]
[Icon("res://assets/img/icons/stats.png")]
public partial class Stats : Resource
{
    private const int DEFAULT_MAX_HEALTH = 100;

    [Export]
    public int Health
    {
        get => _hp;
        set => _hp = Clamp(value, 0, MaxHealth);
    }
    private int _hp = DEFAULT_MAX_HEALTH;

    [Export]
    public int MaxHealth
    {
        get => _maxHp;
        set
        {
            _maxHp = Max(0, value);
            Health = Min(Health, MaxHealth);
        }
    }
    private int _maxHp = DEFAULT_MAX_HEALTH;

    [Export]
    public int Defense { get; set; } = 0;

    [Export]
    public int Strength { get; set; } = 50;

    [Export]
    public float Speed { get; set; } = 200.0f;

    public int ApplyDamage(int damage, Actor actor)
    {
        // Clamp to max of 0 - damage shouldn't heal if Defense is high
        int delta = -Max(damage - Defense, 0);
        Health += delta;
        Global.EventBus.OnHpChanged(actor, delta);
        return delta;
    }
}
