namespace Game;

public partial class Skeleton : Enemy
{
    public override void _Ready()
    {
        base._Ready();

        // GetNode("Blackboard").Call("set_value", "owner", this);
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);

        if (Player != null && !Directional.IsFacing(Player))
        {
            Directional.Flip();
        }

        if (Stats.Health <= 0 && !IsDying)
        {
            IsDying = true;
            // Disable AI
            // GetNode("SkeletonAi").Set("enabled", false);
            // Disable hitbox
            Hitbox.Monitoring = false;
            // Die
            Animation.Play("die");
        }
    }
}