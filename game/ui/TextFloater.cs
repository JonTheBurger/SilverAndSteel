using Godot;
using static Game.Globals;

namespace Game;

public partial class TextFloater : Control
{
    [Export]
    PackedScene HpChangedLabel { get; set; }

    [Export]
    Color DamageColor { get; set; } = Color.Color8(0xFF, 0x00, 0x00);

    [Export]
    Color HealColor { get; set; } = Color.Color8(0x00, 0x00, 0xFF);

    public override void _Ready()
    {
        Global.EventBus.HealthChanged += OnHealthChanged;
    }

    public void OnHealthChanged(CharacterBody2D actor, int diff)
    {
        Label label = HpChangedLabel.Instantiate<Label>();
        label.Text = diff.ToString();
        if (diff >= 0)
        {
            label.Modulate = HealColor;
        }
        else
        {
            label.Modulate = DamageColor;
        }

        label.GlobalPosition = actor.GlobalPosition;
        AddChild(label);
    }
}