namespace Godot;

public static class Extensions
{
    #region Node
    public static T GetNodeOrCreate<T>(this Node self, NodePath path) where T : Node, new()
    {
        if (self.GetNode(path) is not T node)
        {
            node = new T();
            self.AddChild(node);
        }
        return node;
    }
    #endregion

    #region Vector2
    public static Vector2 WithX(this Vector2 self, float x)
    {
        self.X = x;
        return self;
    }

    public static Vector2 WithXFlipped(this Vector2 self)
    {
        return self.WithX(-self.X);
    }

    public static Vector2 WithY(this Vector2 self, float y)
    {
        self.Y = y;
        return self;
    }

    public static Vector2 WithYFlipped(this Vector2 self)
    {
        return self.WithX(-self.X);
    }
    #endregion
}