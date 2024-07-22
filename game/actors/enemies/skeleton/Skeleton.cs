using System.Linq;
using Godot;
using static Godot.Mathf;
using static Game.Globals;

namespace Game;

public partial class Skeleton : CharacterBody2D, IActor
{
    public Node2D Direction => GetNode<Node2D>("Directional");

    public Stats Stats
    {
        get => _stats ??= GetNode<Stats>("Stats");
    }
    private Stats? _stats;

    public Sprite2D Sprite2D
    {
        get => _sprite2D ??= GetNode<Sprite2D>("Directional/Sprite2D");
        set => _sprite2D = value;
    }
    private Sprite2D? _sprite2D;

    public AnimationPlayer AnimationPlayer
    {
        get => _animationPlayer ??= GetNode<AnimationPlayer>("Directional/AnimationPlayer");
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

    public Area2D Weapon
    {
        get => _weapon ??= GetNode<Area2D>("Directional/Weapon");
        set => _weapon = value;
    }
    private Area2D? _weapon;

    public CollisionShape2D Hitbox
    {
        get => _hitbox ??= GetNode<CollisionShape2D>("Directional/Weapon/Hitbox");
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
        Weapon.BodyEntered += OnWeaponHit;
        Fsm.Target = this;
        Player = GetTree().GetNodesInGroup(Groups.PLAYERS).OfType<Player>().FirstOrDefault();
    }

    private void OnWeaponHit(Node2D node)
    {
        if (node == this) { return; }
        if (node is Player player)
        {
            int change = player.Stats.ApplyDamage(Stats);
            Global.EventBus.OnHpChanged(player, change);
        }
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
        Direction.Scale = Direction.Scale.WithXFlipped();
        // Scale = Scale.WithXFlipped();
    }

    public void OnHpChanged(int hp)
    {
        if (Stats.Hp <= 0)
        {
            QueueFree();
        }
    }
}
