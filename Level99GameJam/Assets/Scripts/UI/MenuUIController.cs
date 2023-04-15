using DG.Tweening;

using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class MenuUIController : MonoBehaviour {
  [field: SerializeField, Header("UI")]
  public GameObject MenuPanel { get; private set; }

  [field: SerializeField, Header("Effects")]
  public PostProcessVolume EffectsVolume { get; private set; }

  ColorGrading _colorGradingEffect;
  DepthOfField _depthOfFieldEffect;

  void Start() {
    _colorGradingEffect = EffectsVolume.profile.GetSetting<ColorGrading>();
    _depthOfFieldEffect = EffectsVolume.profile.GetSetting<DepthOfField>();

    ToggleMenu(toggleOn: false);
  }

  void Update() {
    if (Input.GetKeyDown(KeyCode.Tab)) {
      ToggleMenu(!MenuPanel.activeSelf);
    }
  }

  public void ToggleMenu(bool toggleOn) {
    MenuPanel.SetActive(toggleOn);

    Time.timeScale = toggleOn ? 0f : 1f;

    _depthOfFieldEffect.enabled.value = toggleOn;

    DOTween
        .To(() =>
              _colorGradingEffect.saturation.value,
              x => _colorGradingEffect.saturation.value = x,
              toggleOn ? -100f : 0f,
              1f)
        .SetUpdate(true)
        .OnStart(() => _colorGradingEffect.enabled.value = true)
        .OnComplete(() => _colorGradingEffect.enabled.value = toggleOn);
  }
}
