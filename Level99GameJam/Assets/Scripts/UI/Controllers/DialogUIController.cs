using DG.Tweening;

using UnityEngine;

public class DialogUIController : MonoBehaviour {
  [field: SerializeField, Header("UI")]
  public CanvasGroup DialogPanel { get; private set; }

  [field: SerializeField]
  public TMPro.TMP_Text DialogTitle { get; private set; }

  [field: SerializeField]
  public TMPro.TMP_Text DialogText { get; private set; }

  [field: SerializeField]
  public TMPro.TMP_Text DialogConfirmText { get; private set; }

  public void ResetDialog() {
    DialogPanel.blocksRaycasts = false;
    DialogPanel.alpha = 0f;
  }

  public void OpenDialog(DialogData dialogData) {
    DialogPanel.blocksRaycasts = true;
    DialogTitle.text = dialogData.DialogTitle;
    DialogText.text = dialogData.DialogText;
    DialogConfirmText.text = dialogData.DialogConfirmText;

    DOTween.Complete(this, withCallbacks: true);
    DOTween.Sequence()
        .InsertCallback(0f, () => InputManager.Instance.SetCurrentDialogUI(this))
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
        .InsertCallback(0f, () => InputManager.Instance.SetCurrentDialogUI(default))
        .InsertCallback(0.1f, () => InputManager.Instance.LockCursor())
        .Insert(0f, DialogPanel.DOFade(0f, 0.1f))
        .SetTarget(this)
        .SetLink(gameObject)
        .SetUpdate(true);
  }
}
