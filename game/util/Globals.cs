using Godot;

namespace Game;

public partial class Globals : Node
{
    public Logger Logger => _logger ??= GetNode<Logger>("Logger");
    private Logger? _logger;

    public EventBus EventBus => _eventBus ??= GetNode<EventBus>("EventBus");
    private EventBus? _eventBus;
};