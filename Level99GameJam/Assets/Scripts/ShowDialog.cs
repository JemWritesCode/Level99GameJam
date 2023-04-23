using UnityEngine;

public class ShowDialog : MonoBehaviour
{
    [field: SerializeField]
    public DialogUIController DialogUI { get; private set; }

    [field: SerializeField]
    public DialogData DialogDataToShow { get; private set; }

    private DialogUIController GetDialogUI() {
      if (!DialogUI) {
        DialogUI = FindObjectOfType<DialogUIController>();
      }

      return DialogUI;
    }

    public void showDialog()
    {
        if (DialogDataToShow) {
          GetDialogUI().OpenDialog(DialogDataToShow);
        }
    }
}
