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

    [Export]
    public bool CanTurn { get; set; } = true;

    public bool IsFacing(CharacterBody2D body)
    {
        return ((body.GlobalPosition.X >= GlobalPosition.X) || (Facing != Direction.Right)) &&
               ((body.GlobalPosition.X <= GlobalPosition.X) || (Facing != Direction.Left));
    }

    [Signal]
    public delegate void DirectionChangedEventHandler(Direction direction);

    public void Flip()
    {
        if (CanTurn)
        {
            Scale = Scale.WithXFlipped();
            EmitSignal(SignalName.DirectionChanged, (int)Facing);
        }
    }
}