using Godot;

using System;

namespace Game;

[GlobalClass]
[Icon("res://assets/img/icons/actor.png")]
public partial class Actor : CharacterBody2D
{
#nullable disable
    [Export]
    public Stats Stats { get; set; }  // See _GetConfigurationWarnings

    public AnimationPlayer Animation { get; private set; }
    public AudioStreamPlayer2D Audio { get; private set; }
    public Directional Directional { get; private set; }
    public Sprite2D Sprite { get; private set; }
    public Area2D Hitbox { get; private set; }
#nullable enable

    [Export]
    public float Gravity { get; set; } = ProjectSettings.GetSetting("physics/2d/default_gravity").As<float>();

    [Export]
    public PackedScene[] Abilities { get; set; } = Array.Empty<PackedScene>();

    public override void _Ready()
    {
        base._Ready();

        Animation = GetNode<AnimationPlayer>("Animation");
        Audio = GetNode<AudioStreamPlayer2D>("Audio");
        Directional = GetNode<Directional>("Directional");
        Sprite = GetNode<Sprite2D>("Sprite");
        Hitbox = GetNode<Area2D>("Hitbox");

        Hitbox.BodyEntered += OnAttackHit;
    }

    public void Move(Vector2 direction)
    {
        var velocity = Velocity;
        if (direction != Vector2.Zero)
        {
            velocity.X = direction.X * Stats.Speed;
        }
        else
        {
            velocity.X = Mathf.MoveToward(Velocity.X, 0, Stats.Speed);
        }
        Velocity = velocity;
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);

        Velocity = Velocity.WithY(Velocity.Y + (float)delta * Gravity);
        if (JustTurnedAround()) { Directional.Flip(); }
        MoveAndSlide();
    }

    private void OnAttackHit(Node2D node)
    {
        if (node == this) { return; }
        if (node is Actor actor)
        {
            actor.Stats.ApplyDamage(Stats.Strength, actor);
        }
    }

    private bool JustTurnedAround()
    {
        return ((Velocity.X > 0) && (Directional.Facing == Direction.Left)) ||
               ((Velocity.X < 0) && (Directional.Facing == Direction.Right));
    }

    public override string[] _GetConfigurationWarnings()
    {
        if (Stats is null)
        {
            return new string[] { "Stats must be provided for Actors. Please create a Stats resource for it!" };
        }
        return Array.Empty<string>();
    }
}