using Godot;
using System;

namespace Game;

public enum Direction
{
    Right = 1,
    Left = -1,
};

[Icon("res://assets/img/icons/flip.png")]
public partial class Directional : Node2D
{
    // All good nodes face right by default
    [Export]
    public Direction Facing { get; set; } = Direction.Right;

    [Export]
    public bool CanTurn { get; set; } = true;

    [Export]
    public Node2D[] Nodes { get; set; } = Array.Empty<Node2D>();

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
            Facing = Facing == Direction.Right ? Direction.Left : Direction.Right;
            Scale = Scale.WithXFlipped();
            foreach (var node in Nodes)
            {
                var directional = node.GetNodeOrNull<Directional>("");
                if (directional != null)
                {
                    directional.Flip();
                }
                else
                {
                    node.Scale = node.Scale.WithXFlipped();
                }
            }
            EmitSignal(SignalName.DirectionChanged, (int)Facing);
        }
    }

    public void Face(Direction direction)
    {
        if (Facing != direction)
        {
            Flip();
        }
    }
}