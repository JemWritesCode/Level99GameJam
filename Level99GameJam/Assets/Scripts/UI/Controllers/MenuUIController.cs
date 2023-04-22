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

  const KeyCode ToggleMenuPanelKeyCode = KeyCode.F2;

  void Update() {
    if (Input.GetKeyDown(KeyCode.F2)) {
      ToggleMenu(!MenuPanel.activeSelf);
    }
  }

  public void ToggleMenu(bool toggleOn) {
    MenuPanel.SetActive(toggleOn);

    _depthOfFieldEffect.enabled.value = toggleOn;

    DOTween
        .To(() =>
              _colorGradingEffect.saturation.value,
              x => _colorGradingEffect.saturation.value = x,
              toggleOn ? -100f : 0f,
              1f)
        .SetUpdate(true)
        .SetEase(Ease.Linear);

    if (toggleOn) {
      InputManager.Instance.UnlockCursor();
      Time.timeScale = 0f;
    } else {
      InputManager.Instance.LockCursor();
      Time.timeScale = 1f;
    }
  }

  public void OnQuitButton() {
    Debug.Log("Button is quit.");
#if UNITY_EDITOR
    UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
  }
}
