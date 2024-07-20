using Godot;

namespace Game;

[GlobalClass]
[Icon("res://assets/img/icons/stats.png")]
public partial class Stats : Node
{
    [Export]
    public int Hp { get; set; } = 100;

    [Export]
    public int MaxHp { get; set; } = 100;

    [Export]
    public int Def { get; set; } = 0;

    [Export]
    public int Str { get; set; } = 50;

    [Export]
    public float Speed { get; set; } = 200.0f;

    [Signal]
    public delegate void HpChangedEventHandler(int hp);

    public void CalculateDamage(Stats other)
    {
        int hp = (other.Str - this.Def);
        EmitSignal(SignalName.HpChanged, -hp);
        Hp -= hp;
    }
}
