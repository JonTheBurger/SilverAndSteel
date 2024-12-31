using Godot;
using System;

namespace Game;

public partial class Worm : Actor
{
    [Export]
    public StatSheet StatSheet { get; set; }

    public override void _Ready()
    {
        base._Ready();
    }
}
