using Coffee.UIEffects;

using DG.Tweening;

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

  [field: SerializeField, Header("Effect")]
  public UIShiny CoinsShinyEffect { get; private set; }

  int _currentCoinsValue = 0;

  public void ResetPanel() {
    _currentCoinsValue = 0;
    CoinsValue.text = "0";
  }

  public void SetCoinsValue(int coinsValue) {
    CoinsValue.DOCounter(_currentCoinsValue, coinsValue, 0.25f, false);
    CoinsShinyEffect.Play();

    _currentCoinsValue = coinsValue;
  }
}
