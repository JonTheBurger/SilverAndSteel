using System;
using System.Collections.Generic;

using Godot;

namespace Game;

[Tool]
public partial class AudioLibrary : Node
{
    public enum Id
    {
        None,
        SwordHit01,
        SwordMiss01,
    }

    [Export]
    public AudioStream[] Streams { get; set; } = Array.Empty<AudioStream>();

    private readonly Dictionary<Id, string> _idToPath = new();
    private readonly Dictionary<Id, AudioStream> _cache = new();

    public override void _Ready()
    {
        foreach (var stream in Streams)
        {
            Id id = (Id)PathToId(stream.ResourcePath)!;  // This can only blow up if editor warnings were ignored
            _idToPath[id] = stream.ResourcePath;
        }
    }

    public bool Load(Id id)
    {
        bool loaded = _cache.ContainsKey(id);
        if (loaded) { return true; }

        bool ok = _idToPath.TryGetValue(id, out string? path);
        if (!ok || path == null) { return false; }

        var resource = LoadFromPath(path);
        if (resource == null) { return false; }

        _cache.Add(id, resource);
        return true;
    }

    public T GetStream<T>(Id id) where T : AudioStream
    {
        return TryGetStream<T>(id)!;
    }

    public AudioStream GetStream(Id id)
    {
        return TryGetStream<AudioStream>(id)!;
    }

    public T? TryGetStream<T>(Id id) where T : AudioStream
    {
        Load(id);
        return _cache[id] as T;
    }

    public bool Evict(Id id)
    {
        return _cache.Remove(id);
    }

    public static string PathToName(string path)
    {
        string fname = path.Split('/')[^1];
        return fname[..fname.LastIndexOf('.')].Replace("-", "").Replace(".", "");
    }

    public static Id? NameToId(string name)
    {
        bool ok = Enum.TryParse(name, ignoreCase: true, out Id id);
        return ok ? id : null;
    }

    public static Id? PathToId(string path)
    {
        return NameToId(PathToName(path));
    }

    public static T? LoadFromPath<T>(string path) where T : AudioStream
    {
        return ResourceLoader.Load(path) as T;
    }

    public static AudioStream? LoadFromPath(string path)
    {
        return LoadFromPath<AudioStream>(path);
    }

    public override string[] _GetConfigurationWarnings()
    {
        var warnings = new List<string> { };
        var registeredIds = new HashSet<Id>();
        int idx = 0;

        foreach (var stream in Streams)
        {
            if (stream == null)
            {
                warnings.Add($"Stream at {idx} is null!");
                continue;
            }

            Id? maybe = PathToId(stream.ResourcePath);
            if (maybe == null)
            {
                warnings.Add($"No corresponding AudioLibrary.Id for {stream.ResourcePath}! Add it to the .cs file!");
                continue;
            }

            Id id = maybe.Value;
            if (registeredIds.Contains(id))
            {
                warnings.Add($"Duplicate ID parsed: {id} already exists for {stream.ResourcePath}. Files must have different extensions!");
                continue;
            }

            registeredIds.Add(id);
            ++idx;
        }

        return warnings.ToArray();
    }
}
