using Godot;

namespace Game;

public partial class MainMenu : CanvasLayer
{
    [Export]
    public PackedScene FirstLevel { get; set; }

    private MenuTransition? Transition;

    private Button? StartButton;
    private Button? LoadButton;
    private Button? OptionsButton;
    private Button? QuitButton;

    public override void _Ready()
    {
        Transition = GetNode<MenuTransition>("MenuTransition");

        StartButton = GetNode<Button>("PanelContainer/GridContainer/StartButton");
        LoadButton = GetNode<Button>("PanelContainer/GridContainer/LoadButton");
        OptionsButton = GetNode<Button>("PanelContainer/GridContainer/OptionsButton");
        QuitButton = GetNode<Button>("PanelContainer/GridContainer/QuitButton");

        StartButton.Pressed += OnStartPressed;
        LoadButton.Pressed += OnLoadPressed;
        OptionsButton.Pressed += OnOptionsPressed;
        QuitButton.Pressed += OnQuitPressed;

        StartButton.GrabFocus();
    }

    private void OnStartPressed()
    {
        Transition.Animator.AnimationFinished += (_) => LoadLevel();
        Transition.FadeOut();
    }

    private void LoadLevel()
    {

        GetTree().ChangeSceneToPacked(FirstLevel);
    }

    private void OnLoadPressed()
    {
    }

    private void OnOptionsPressed()
    {
    }

    private void OnQuitPressed()
    {
        GetTree().Quit();
    }
}
