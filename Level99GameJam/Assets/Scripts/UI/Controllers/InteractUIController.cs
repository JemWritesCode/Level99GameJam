using DG.Tweening;

using UnityEngine;

public class InteractUIController : MonoBehaviour {
  [field: SerializeField, Header("UI")]
  public CanvasGroup InteractPanel { get; private set; }

  [field: SerializeField]
  public TMPro.TMP_Text InteractText { get; private set; }

  private void Awake() {
    ResetPanel();
  }

  public void ResetPanel() {
    InteractPanel.alpha = 0f;
    InteractPanel.blocksRaycasts = false;
    InteractText.text = "...";
  }

  public void ShowInteractPanel(string interactText) {
    InteractText.text = interactText;

    InteractPanel.DOComplete(withCallbacks: true);

    DOTween.Sequence()
        .SetTarget(InteractPanel)
        .Insert(0f, InteractPanel.transform.DOPunchPosition(new(0f, 10f, 0f), 0.2f, 0, 0f))
        .Insert(0.1f, InteractPanel.DOFade(1f, 0.1f));
  }

  public void HideInteractPanel() {
    InteractPanel.DOComplete(withCallbacks: false);
    InteractPanel.DOFade(0f, 0.1f);
  }
}