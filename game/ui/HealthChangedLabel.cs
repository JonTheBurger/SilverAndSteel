using Godot;

using System;

public partial class HealthChangedLabel : Label
{
    [Export]
    Vector2 FloatSpeed { get; set; } = new Vector2(0, -50);

    public override void _Process(double delta)
    {
        Position += FloatSpeed * (float)delta;
    }

    public void OnTimeout()
    {
        QueueFree();
    }
}
