using Godot;

namespace Game;

public partial class MenuTransition : Control
{
    public AnimationPlayer? Animator { get; private set; }
    TextureRect? _textureRect;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Animator = GetNode<AnimationPlayer>("AnimationPlayer");
        _textureRect = GetNode<TextureRect>("TextureRect");
        _textureRect.Visible = false;
    }

    public void FadeIn()
    {
        Animator!.Queue("fade_in");
    }

    public void FadeOut()
    {
        Animator!.Queue("fade_out");
    }
}
