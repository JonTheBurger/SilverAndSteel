using Godot;

namespace Game;

[Icon("res://assets/img/icons/globe.png")]
public partial class Globals : Node
{
    public override void _Ready()
    {
        s_globals = this;
    }

    public static Globals Global => s_globals;
    private static Globals? s_globals;

    public Logger Logger => _logger ??= GetNode<Logger>("Logger");
    private Logger? _logger;

    public EventBus EventBus => _eventBus ??= GetNode<EventBus>("EventBus");
    private EventBus? _eventBus;
};