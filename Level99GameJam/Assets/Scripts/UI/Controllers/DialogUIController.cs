using DG.Tweening;

using UnityEngine;

public class DialogUIController : MonoBehaviour {
  [field: SerializeField, Header("UI")]
  public CanvasGroup DialogPanel { get; private set; }

  private void OnEnable() {
    OpenDialog();
  }

  public void OpenDialog() {
    DialogPanel.blocksRaycasts = true;

    DOTween.Complete(this, withCallbacks: true);
    DOTween.Sequence()
        .InsertCallback(0f, () => InputManager.Instance.CurrentDialogUI = this)
        .InsertCallback(0.1f, () => InputManager.Instance.UnlockCursor())
        .Insert(0f, DialogPanel.DOFade(1f, 0.1f))
        .SetTarget(this)
        .SetLink(gameObject)
        .SetUpdate(true);
  }

  public void CloseDialog() {
    DialogPanel.blocksRaycasts = false;

    DOTween.Complete(this, withCallbacks: true);
    DOTween.Sequence()
        .InsertCallback(0f, () => InputManager.Instance.CurrentDialogUI = default)
        .InsertCallback(0.1f, () => InputManager.Instance.LockCursor())
        .Insert(0f, DialogPanel.DOFade(0f, 0.1f))
        .SetTarget(this)
        .SetLink(gameObject)
        .SetUpdate(true);
  }
}
