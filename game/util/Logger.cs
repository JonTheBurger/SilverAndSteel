using System;
using System.Text;

using Godot;

namespace Game;

public enum Severity
{
    /// <summary>
    /// Do not log anything.
    /// </summary>
    None = 0,

    /// <summary>
    /// Ephemeral debug logging; always show.
    /// </summary>
    Debug = 1,

    /// <summary>
    /// We're about to crash.
    /// </summary>
    Critical = 2,

    /// <summary>
    /// Something is inhibiting normal game function; stability may suffer.
    /// </summary>
    Error = 3,

    /// <summary>
    /// Something unexpected happened, but we can handle it.
    /// </summary>
    Warning = 4,

    /// <summary>
    /// Some notable state change has occurred.
    /// </summary>
    Info = 5,

    /// <summary>
    /// Verbose messages; generally off unless needed. Debug mode only.
    /// </summary>
    Trace = 6,
};

public enum TimeSource
{
    None = 0,
    WallClock = 1,
    EngineTicks = 2,
};

public partial class Logger : Node
{
    [Export]
    public string Module { get; set; } = " GAME LOG ";

    [Export]
    public Severity Level { get; set; } = Severity.Info;

    [Export]
    public TimeSource TimeSource { get; set; } = TimeSource.WallClock;

    [Export]
    public int Indent
    {
        get => _indent;
        set => _indent = Math.Max(0, value);
    }
    private int _indent = 0;

    public bool Locked
    {
        get => _lock != null;
        set => _lock = value ? new object() : null;
    }
    private object? _lock = null;

    public void Log(Severity severity, string message)
    {
        switch (severity)
        {
            case Severity.Debug:
                Debug(message);
                break;
            case Severity.Critical:
                Critical(message);
                break;
            case Severity.Error:
                Error(message);
                break;
            case Severity.Warning:
                Warning(message);
                break;
            case Severity.Info:
                Info(message);
                break;
            case Severity.Trace:
                Trace(message);
                break;
            default:
                break;
        }
    }

    public void Debug(string message)
    {
        if (Level > Severity.Debug) { return; }
        GD.PrintRich(FormatMessage(message, Severity.Debug));
    }

    public void Critical(string message)
    {
        if (Level > Severity.Critical) { return; }
        GD.PushError(FormatMessage(message, Severity.Critical));
    }

    public void Error(string message)
    {
        if (Level > Severity.Error) { return; }
        GD.PushError(FormatMessage(message, Severity.Error));
    }

    public void Warning(string message)
    {
        if (Level > Severity.Warning) { return; }
        GD.PushWarning(FormatMessage(message, Severity.Warning));
    }

    public void Info(string message)
    {
        if (Level > Severity.Info) { return; }
        GD.PrintRich(FormatMessage(message, Severity.Info));
    }

    public void Trace(string message)
    {
        if (Level > Severity.Trace) { return; }
        GD.Print(FormatMessage(message, Severity.Trace));
    }

    #region Message Formatting
    private readonly StringBuilder _msg = new StringBuilder("", 100);

    private string FormatMessage(string message, Severity severity)
    {
        if (_lock != null)
        {
            lock (_lock)
            {
                return FormatSingleThreaded(message, severity);
            }
        }
        else
        {
            return FormatSingleThreaded(message, severity);
        }
    }

    private string FormatSingleThreaded(string message, Severity severity)
    {
        return NewMessage()
            .WithColor(severity)
            .WithTime()
            .WithSeverity(severity)
            .WithName()
            .WithNoColor(severity)
            .WithMessage(message)
            .ToString();
    }

    private Logger NewMessage()
    {
        _msg.Clear();
        return this;
    }

    private Logger WithColor(Severity severity)
    {
        // https://docs.godotengine.org/en/stable/classes/class_%40globalscope.html#class-globalscope-method-print-rich
        switch (severity)
        {
            case Severity.Debug:
                _msg.Append("[color=purple]");
                break;
            case Severity.Info:
                _msg.Append("[color=green]");
                break;
            default:
                break;
        }
        return this;
    }

    private Logger WithNoColor(Severity severity)
    {
        switch (severity)
        {
            case Severity.Debug:
            case Severity.Info:
                _msg.Append("[/color]");
                break;
            default:
                break;
        }
        return this;
    }


    private Logger WithTime()
    {
        switch (TimeSource)
        {
            case TimeSource.WallClock:
                _msg.Append('[');
                _msg.Append(DateTime.Now.ToString("HH:mm:ss.fff"));
                _msg.Append(']');
                break;
            case TimeSource.EngineTicks:
                _msg.Append('[');
                _msg.Append(Time.GetTicksMsec());
                _msg.Append(']');
                break;
            default:
                break;
        }
        return this;
    }

    private Logger WithSeverity(Severity severity)
    {
        switch (severity)
        {
            case Severity.Debug:
                _msg.Append("[DEBUG]");
                break;
            case Severity.Critical:
                _msg.Append("[CRIT ]");
                break;
            case Severity.Error:
                _msg.Append("[ERROR]");
                break;
            case Severity.Warning:
                _msg.Append("[WARN ]");
                break;
            case Severity.Info:
                _msg.Append("[INFO ]");
                break;
            case Severity.Trace:
                _msg.Append("[TRACE]");
                break;
            default:
                break;
        }
        return this;
    }

    private Logger WithName()
    {
        _msg.Append('[');
        _msg.Append(Module);
        _msg.Append("]: ");
        return this;
    }

    private StringBuilder WithMessage(string message)
    {
        _msg.Append(' ', _indent);
        _msg.Append(message);
        return _msg;
    }
    #endregion
};