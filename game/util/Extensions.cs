namespace Godot;

public static class Extensions
{
    public static Vector2 FlippedX(this Vector2 self)
    {
        Vector2 flipped = self;
        flipped.X = -flipped.X;
        return flipped;
    }
}