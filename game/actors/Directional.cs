using Godot;

namespace Game;

public enum Direction
{
    Right,
    Left,
};

[Icon("res://assets/img/icons/flip.png")]
public partial class Directional : Node2D
{
    // All good nodes face right by default
    [Export]
    public Direction Facing
    {
        get => (Scale.X < 0) ? Direction.Left : Direction.Right;
        set
        {
            if (value != Facing)
            {
                Flip();
            }
        }
    }

    [Signal]
    public delegate void DirectionChangedEventHandler(Direction direction);

    public void Flip()
    {
        Scale = Scale.WithXFlipped();
        EmitSignal(SignalName.DirectionChanged, (int)Facing);
    }
}