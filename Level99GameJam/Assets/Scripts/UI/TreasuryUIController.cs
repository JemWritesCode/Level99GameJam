using UnityEngine;
using UnityEngine.UI;

public class TreasuryUIController : MonoBehaviour {
  [field: SerializeField]
  public CanvasGroup TreasuryPanel { get; private set; }

  [field: SerializeField]
  public TMPro.TMP_Text CoinsValue { get; private set; }

  [field: SerializeField]
  public TMPro.TMP_Text CoinsLabel { get; private set; }

  [field: SerializeField]
  public Image CoinsImage { get; private set; }

  public void ResetPanel() {
    CoinsValue.text = "0";
  }
}
