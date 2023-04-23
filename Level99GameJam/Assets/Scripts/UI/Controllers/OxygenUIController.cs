using UnityEngine;
using UnityEngine.UI;

public class OxygenUIController : MonoBehaviour {
  [field: SerializeField, Header("UI")]
  public CanvasGroup OxygenPanel { get; private set; }

  [field: SerializeField, Header("Slider")]
  public Slider OxygenSlider { get; private set; }

  [field: SerializeField]
  public TMPro.TMP_Text OxygenSliderHandle { get; private set; }

  public void ShowOxygenPanel() {

  }

  public void HideOxygenPanel() {

  }
}
