using DG.Tweening;

using UnityEngine;
using UnityEngine.UI;

public class OxygenUIController : MonoBehaviour {
  [field: SerializeField, Header("UI")]
  public CanvasGroup OxygenPanel { get; private set; }

  [field: SerializeField, Header("Slider")]
  public Slider OxygenSlider { get; private set; }

  [field: SerializeField]
  public TMPro.TMP_Text OxygenSliderHandle { get; private set; }

  void Start() {
    ResetPanel();
    OxygenManager.Instance.SetOxygenUI(this);
  }

  bool _isVisible = false;

  public bool IsVisible() {
    return _isVisible;
  }

  public void ResetPanel() {
    _isVisible = false;
    OxygenPanel.alpha = 0f;
    OxygenPanel.blocksRaycasts = false;
  }

  public void ShowOxygenPanel() {
    _isVisible = true;

    OxygenPanel.DOComplete(withCallbacks: true);
    DOTween.Sequence()
        .Insert(0f, OxygenPanel.DOFade(1f, 0.25f))
        .Insert(0f, OxygenPanel.transform.DOPunchPosition(new(0f, 10f, 0f), 0.50f, 0, 0f))
        .SetLink(OxygenPanel.gameObject)
        .SetTarget(OxygenPanel);
  }

  public void HideOxygenPanel() {
    _isVisible = false;

    OxygenPanel.DOComplete(withCallbacks: true);
    DOTween.Sequence()
        .Insert(0f, OxygenPanel.DOFade(0f, 0.25f))
        .Insert(0f, OxygenPanel.transform.DOPunchPosition(new(0f, 10f, 0f), 0.50f, 0, 0f))
        .SetLink(OxygenPanel.gameObject)
        .SetTarget(OxygenPanel);
  }
}
