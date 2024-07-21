using System.Collections.Generic;
using System.Linq;

using Godot;

namespace Game;

public partial class Hud : CanvasLayer
{
    /// <summary>
    /// Shader float parameter that will be down-scaled to zero as HP goes down.
    /// </summary>
    [Export]
    public StringName ShaderParameter { get; set; } = "saturation";

    /// <summary>
    /// Uniform parameters are often 1.0f as a default, but we'll load from the first
    /// heart to check.
    /// </summary>
    private float _shaderParamMax = 1.0f;

    public HBoxContainer HpBoxContainer => _hpBoxContainer ??= GetNode<HBoxContainer>("HpBoxContainer");
    private HBoxContainer? _hpBoxContainer;

    /// <summary>
    /// HP is represented as a series of discrete images, which we will call "hearts".
    /// </summary>
    private List<TextureRect> _hearts = new(10);

    public override void _Ready()
    {
        _hearts.AddRange(HpBoxContainer.GetChildren().OfType<TextureRect>());
        _shaderParamMax = ((_hearts[0].Material as ShaderMaterial)?.GetShaderParameter(ShaderParameter).As<float>()) ?? _shaderParamMax;
        this.Global().EventBus.HealthChanged += (actor, diff) => {
            if (actor is Player player)
            {
                SetHealth(player.Stats.Hp, player.Stats.MaxHp);
            }
        };
    }

    public void SetHealth(int current, int max)
    {
        // Pick which heart to visually affect based on our current health
        float percent = (float)current / max;
        int idx = Mathf.Max((int)Mathf.Ceil(percent * _hearts.Count) - 1, 0);

        // All hearts below the current heart are at 100%
        for (int i = 0; i < idx; ++i)
        {
            SetHeartShaderParameter(i, _shaderParamMax);
        }

        // The current heart is the % remaining after shaving all off all of the lower 100% hearts contributions
        float heartPercent = (percent * _hearts.Count) - idx;
        // NOTE: This *should* be scaled by _shaderParamMax, but we're using an
        // RGB desaturate shader. Our eyes don't perceive color linearly in RGB
        // space, so we fudge it to make partly filled hearts look a bit better.
        if (heartPercent > 0.99f)
        {
            heartPercent = _shaderParamMax;  // Hack to set to max when full
        }
        SetHeartShaderParameter(idx, Mathf.Max(heartPercent, 0.0f));

        // All hearts above the current heart are at 0%
        for (int i = idx + 1; i < _hearts.Count; ++i)
        {
            SetHeartShaderParameter(i, 0.0f);
        }
    }

    private void SetHeartShaderParameter(int index, float value)
    {
        (_hearts[index].Material as ShaderMaterial)?.SetShaderParameter(ShaderParameter, value);
    }
}