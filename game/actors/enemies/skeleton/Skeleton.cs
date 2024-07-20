using System.Linq;

using Godot;
using static Godot.Mathf;

namespace Game;

public partial class Skeleton : CharacterBody2D, IActor
{
    public Stats Stats
    {
        get => _stats ??= GetNode<Stats>("Stats");
    }
    private Stats? _stats;

    public Sprite2D Sprite2D
    {
        get => _sprite2D ??= GetNode<Sprite2D>("Sprite2D");
        set => _sprite2D = value;
    }
    private Sprite2D? _sprite2D;

    public AnimationPlayer AnimationPlayer
    {
        get => _animationPlayer ??= GetNode<AnimationPlayer>("AnimationPlayer");
        set => _animationPlayer = value;
    }
    private AnimationPlayer? _animationPlayer;

    private Player? Player;

    public SkeletonFsm Fsm
    {
        get => _fsm ??= GetNode<SkeletonFsm>("SkeletonFsm");
        set => _fsm = value;
    }
    private SkeletonFsm? _fsm;

    public Area2D Mace
    {
        get => _mace ??= GetNode<Area2D>("Mace");
        set => _mace = value;
    }
    private Area2D? _mace;

    public CollisionShape2D Hitbox
    {
        get => _hitbox ??= GetNode<CollisionShape2D>("Mace/Hitbox");
        set => _hitbox = value;
    }
    private CollisionShape2D? _hitbox;

    private Vector2 _direction = Vector2.Right;

    public override void _EnterTree()
    {
        GetNode<SkeletonFsm>("SkeletonFsm").Target = this;
    }

    public override void _Ready()
    {
        Logger.Global.Debug("Debug");
        Logger.Global.Info("Info");
        Logger.Global.Trace("Trace");
        Logger.Global.Warning("Warning");
        Stats.HpChanged += OnHpChanged;
        Fsm.Target = this;
        Player = GetTree().GetNodesInGroup(Groups.PLAYERS).OfType<Player>().FirstOrDefault();
    }

    public override void _PhysicsProcess(double delta)
    {
        MoveAndSlide();
        if (Player != null)
        {
            if ((Player.GlobalPosition.X < GlobalPosition.X) && (_isFacingRight))
            {
                TurnAround();
            }
            else if ((Player.GlobalPosition.X > GlobalPosition.X) && (!_isFacingRight))
            {
                TurnAround();
            }
        }
    }

    public bool IsPlayerInRange()
    {
        return (Player != null) && (Abs(Player.GlobalPosition.X - GlobalPosition.X) <= 25);
    }

    public bool IsPlayerOutOfRange()
    {
        return (Player != null) && (Abs(Player.GlobalPosition.X - GlobalPosition.X) > 25);
    }

    public void MoveTowardsPlayer()
    {
        if (Player == null) { return; }

        var direction = (Player.GlobalPosition - GlobalPosition).Normalized();
        var velocity = Velocity;
        if (direction != Vector2.Zero)
        {
            velocity.X = direction.X * Stats.Speed;
        }
        else
        {
            velocity.X = MoveToward(Velocity.X, 0, Stats.Speed);
        }
        Velocity = velocity;
    }

    private bool _isFacingRight = true;
    private void TurnAround()
    {
        _isFacingRight = !_isFacingRight;
        Sprite2D.FlipH = !Sprite2D.FlipH;
        Hitbox.GlobalPosition = Hitbox.GlobalPosition.WithXFlipped();
    }

    private void OnHpChanged(int hp)
    {
        if (Stats.Hp <= 0)
        {
            QueueFree();
        }
    }
}
