using UnityEngine;

[CreateAssetMenu(menuName = "Dialog/DialogData", order = 1)]
public class DialogData : ScriptableObject {
  [field: SerializeField]
  public string DialogId { get; private set; }

  [field: SerializeField, Header("UI")]
  public string DialogTitle { get; private set; } = "...";

  [field: SerializeField, TextArea(minLines: 5, maxLines: 15)]
  public string DialogText { get; private set; } = "...?";

  [field: SerializeField]
  public string DialogConfirmText { get; private set; } = "...!";
}
