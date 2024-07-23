using Godot;

namespace Game;

public partial class PlayerCamera : Camera2D
{
    public void OnDirectionChanged(Direction direction) // NOSONAR S1172
    {
        DragHorizontalOffset *= -1;
    }
}