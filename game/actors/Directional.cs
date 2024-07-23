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
    [Export]
    public Direction Facing
    {
        get => (Scale.X < 0) ? Direction.Left : Direction.Right;
        set
        {
            if (value == Direction.Left && Facing == Direction.Right)
            {
                Flip();
            }
            else if (value == Direction.Right && Facing == Direction.Left)
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