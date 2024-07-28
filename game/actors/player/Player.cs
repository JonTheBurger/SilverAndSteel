using Godot;

namespace Game;

public partial class Player : Actor
{
    private PlayerHsm? _fsm;

    public override void _Ready()
    {
        base._Ready();

        var camera = GetNodeOrNull<PlayerCamera>("PlayerCamera");
        if (camera != null)
        {
            Directional.DirectionChanged += camera.OnDirectionChanged;
        }

        _fsm = GetNode<PlayerHsm>("PlayerHsm");
        _fsm.Start(this, Animation);
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        _fsm.ProcessPhysics(delta);
    }

    public override void _Input(InputEvent @event)
    {
        base._Input(@event);
        _fsm.ProcessInput(@event);
    }

    public void Move()
    {
        Vector2 direction = Input.GetVector("left", "right", "up", "down");
        Move(direction);
    }
}
