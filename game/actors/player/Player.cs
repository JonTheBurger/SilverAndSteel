using Godot;

namespace Game;

public partial class Player : Actor
{
	[Export]
	public float JumpVelocity { get; set; } = -150.0f;

	private PlayerFsm? _fsm;

	public override void _Ready()
	{
		base._Ready();

		_fsm = GetNode<PlayerFsm>("Fsm");
		_fsm.Target = this;
		Animation.AnimationFinished += _fsm.OnAnimationFinished;

		var camera = GetNodeOrNull<PlayerCamera>("PlayerCamera");
		if (camera != null)
		{
			Directional.DirectionChanged += camera.OnDirectionChanged;
		}
	}

	public void Move()
	{
		Vector2 direction = Input.GetVector("left", "right", "up", "down");
		Move(direction);
	}
}
