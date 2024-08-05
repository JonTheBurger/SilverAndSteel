using System.Collections.Generic;
using System.IO.Enumeration;
using System.Linq;

using Game;

namespace Godot;

public static class Extensions
{
    #region Node
    public static T GetNodeOrCreate<T>(this Node self, NodePath path) where T : Node, new()
    {
        T? child = self.GetNodeOrNull<T>(path);
        if (child == null)
        {
            child = new T();
            self.AddChild(child);
        }
        return child;
    }

    public static void LoadNodeOrWarn<T>(this Node self, ref T? node, string? pattern = null) where T : Node, new()
    {
        node ??= self.FindChildBfs<T>(pattern).FirstOrDefault();
        if (node == null) { GD.PushWarning($"Could not find node of type '{typeof(T).Name}' from {self.Name}"); }
    }

    public static IEnumerable<T> FindChildBfs<T>(this Node self, string? pattern = null, bool includeInternal = false) where T : Node
    {
        var queue = new Queue<Node>();
        queue.Enqueue(self);

        while (queue.Count > 0)
        {
            Node node = queue.Dequeue();
            int count = node.GetChildCount(includeInternal);
            for (int i = 0; i < count; ++i)
            {
                Node child = node.GetChild(i);
                T? match = MatchesPatternOrNull<T>(child, pattern);
                if (match != null)
                {
                    yield return match;
                }
                queue.Enqueue(child);
            }
        }
    }

    private static T? MatchesPatternOrNull<T>(Node node, string? pattern = null) where T : Node
    {
        if (node is T match && ((pattern == null) || FileSystemName.MatchesSimpleExpression(pattern, node.GetPath().ToString())))
        {
            return match;
        }
        return null;
    }

    public static IEnumerable<T> FindChildDfsPre<T>(this Node self, string? pattern = null, bool includeInternal = false) where T : Node
    {
        var stack = new Stack<Node>();
        PushChildren(self, stack);

        while (stack.Count > 0)
        {
            Node node = stack.Pop();
            T? match = MatchesPatternOrNull<T>(node, pattern);
            if (match != null)
            {
                yield return match;
            }
            PushChildren(node, stack);
        }
    }

    private static void PushChildren(Node node, Stack<Node> stack, bool includeInternal = false)
    {
        int count = node.GetChildCount(includeInternal);
        for (int i = count - 1; i >= 0; --i)
        {
            stack.Push(node.GetChild(i));
        }
    }

    public static Globals Global(this Node self)
    {
        // return self.GetTree().Root.GetNode<Globals>("Globals");
        return self.GetNode<Globals>("/root/Globals");
    }
    #endregion

    #region CharacterBody2D
    public static void SlideToward(this CharacterBody2D self, CharacterBody2D body, float speed)
    {
        var direction = (body.GlobalPosition - self.GlobalPosition).Normalized();
        var velocity = self.Velocity;
        if (direction != Vector2.Zero)
        {
            velocity.X = direction.X * speed;
        }
        else
        {
            velocity.X = Mathf.MoveToward(self.Velocity.X, 0, speed);
        }
        self.Velocity = velocity;
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