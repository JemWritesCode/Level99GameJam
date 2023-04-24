using DG.Tweening;

using System.Collections;

using UnityEngine;

public class GameStartEvent : MonoBehaviour {
  [field: SerializeField]
  public DialogUIController DialogUI { get; private set; }

  [field: SerializeField]
  public DialogData DialogDataToShow { get; private set; }

  private void Start() {
    StartCoroutine(OpenDialogAndHelpPanel());
  }

  IEnumerator OpenDialogAndHelpPanel() {
    yield return null;

    DialogUI.OpenDialog(DialogDataToShow);
    InputManager.Instance.InventoryUI.HelpPanel.DOFade(1f, 0.25f);

    while (DialogUI.DialogPanel.blocksRaycasts) {
      yield return null;
    }

    InputManager.Instance.InventoryUI.HelpPanel.DOFade(0f, 0.25f);
  }
}