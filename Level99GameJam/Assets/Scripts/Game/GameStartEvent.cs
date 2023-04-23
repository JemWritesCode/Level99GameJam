using System.Collections;

using UnityEngine;

public class GameStartEvent : MonoBehaviour {
  [field: SerializeField]
  public DialogUIController DialogUI { get; private set; }

  [field: SerializeField]
  public DialogData DialogDataToShow { get; private set; }

  private IEnumerator Start() {
    yield return null;
    DialogUI.OpenDialog(DialogDataToShow);
  }
}