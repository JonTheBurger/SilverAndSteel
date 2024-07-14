using Godot;

namespace Game;

public partial class PlayerCamera : Camera2D
{
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }

    public void OnTurnAround(bool isFacingRight) // NOSONAR S1172
    {
        DragHorizontalOffset *= -1;
    }
}
