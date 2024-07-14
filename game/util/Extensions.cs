using Godot;

namespace Game;

public static class Extensions
{
    public static T OnReady<T>(this Node self, string path, ref T? field) where T : Node
    {
        field ??= self.GetNode<T>(path);
        return field;
    }
}
