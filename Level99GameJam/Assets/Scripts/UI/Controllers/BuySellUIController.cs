using Coffee.UIEffects;

using DG.Tweening;

using UnityEngine;
using UnityEngine.UI;

public class BuySellUIController : MonoBehaviour {
  [field: SerializeField]
  public CanvasGroup BuySellPanel { get; private set; }

  [field: SerializeField, Header("BuySellButton")]
  public Button BuySellButton { get; private set; }

  [field: SerializeField]
  public TMPro.TMP_Text BuySellButtonLabel { get; private set; }

  [field: SerializeField]
  public UIEffect BuySellButtonDisableEffect { get; private set; }

  [field: SerializeField, Header("BuySellCost")]
  public TMPro.TMP_Text CostValue { get; private set; }

  [field: SerializeField]
  public TMPro.TMP_Text CostLabel { get; private set; }

  [field: SerializeField]
  public UIEffect CostDisableEffect { get; private set; }

  int _currentCostValue = 0;

  public void ResetPanel() {
    BuySellPanel.alpha = 0f;
    BuySellPanel.blocksRaycasts = false;
    CostLabel.alpha = 0f;

    _currentCostValue = 0;
  }

  public void SetPanel(int costValue, bool canBuySell) {
    BuySellPanel.blocksRaycasts = canBuySell;

    DOTween.Complete(BuySellPanel, withCallbacks: true);

    Sequence sequence =
        DOTween.Sequence()
            .SetTarget(BuySellPanel)
            .SetLink(gameObject)
            .Insert(0f, BuySellPanel.DOFade(1f, 0.10f))
            .Insert(0f, CostValue.DOCounter(_currentCostValue, costValue, 0.1f, false))
            .Insert(
                0f,
                DOTween.To(
                    () => BuySellButtonDisableEffect.effectFactor,
                    x => BuySellButtonDisableEffect.effectFactor = x,
                    canBuySell ? 0f : 1f,
                    0.10f))
            .Insert(
                0f,
                DOTween.To(
                    () => CostDisableEffect.effectFactor,
                    x => CostDisableEffect.effectFactor = x,
                    canBuySell ? 0f : 1f,
                    0.10f));

    if (canBuySell) {
      sequence
          .Insert(0f, CostLabel.DOFade(0f, 0.1f))
          .Insert(0f, BuySellButtonLabel.DOFade(1f, 0.1f));
    } else {
      sequence
          .Insert(0f, BuySellButtonLabel.DOFade(0.4f, 0.1f))
          .Insert(0f, CostLabel.transform.DOPunchPosition(new Vector3(0f, 5f, 0f), 0.1f))
          .Insert(0f, CostLabel.DOFade(1f, 0.1f));
    }

    _currentCostValue = costValue;
  }

  public void HidePanel() {
    BuySellPanel.blocksRaycasts = false;

    DOTween.Complete(BuySellPanel, withCallbacks: true);

    DOTween.Sequence()
        .SetTarget(BuySellPanel)
        .SetLink(gameObject)
        .Insert(0f, BuySellPanel.transform.DOPunchPosition(new Vector3(0f, -5f, 0f), 0.3f, 0, 0f))
        .Insert(0f, BuySellPanel.DOFade(0f, 0.15f));
  }
}
