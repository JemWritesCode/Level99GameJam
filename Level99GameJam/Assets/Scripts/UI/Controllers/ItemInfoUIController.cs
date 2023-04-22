using DG.Tweening;

using UnityEngine;

public class ItemInfoUIController : MonoBehaviour {
  [field: SerializeField]
  public CanvasGroup ItemInfoPanel { get; private set; }

  [field: SerializeField]
  public TMPro.TMP_Text ItemName { get; private set; }

  [field: SerializeField]
  public TMPro.TMP_Text ItemDescription { get; private set; }

  [field: SerializeField, Header("Badge")]
  public GameObject ItemBadge { get; private set; }

  [field: SerializeField]
  public TMPro.TMP_Text ItemBadgeLabel { get; private set; }

  public void ResetPanel() {
    ItemInfoPanel.alpha = 0f;
    ItemInfoPanel.blocksRaycasts = false;
  }

  public void SetPanel(string name, string description, bool showBadge) {
    DOTween.Complete(ItemInfoPanel, withCallbacks: true);

    DOTween.Sequence()
        .SetTarget(ItemInfoPanel)
        .Insert(0f, ItemInfoPanel.DOFade(0f, 0.15f))
        .InsertCallback(0.15f, () => {
          ItemName.text = name;
          ItemDescription.text = description;
          ItemBadge.SetActive(showBadge);
        })
        .Insert(0f, ItemName.transform.DOPunchPosition(new(0f, 7.5f, 0f), 0.3f, 0, 0))
        .Insert(0f, ItemDescription.transform.DOPunchPosition(new(0f, 7.5f, 0f), 0.3f, 0, 0))
        .Insert(0f, ItemBadge.transform.DOPunchPosition(new(0f, 7.5f, 0f), 0.3f, 0, 0))
        .Insert(0.15f, ItemInfoPanel.DOFade(1f, 0.15f));
  }

  public void ClearItemInfoPanel() {
    ItemInfoPanel.DOKill();
    ItemInfoPanel.DOFade(0f, 0.25f);
  }
}
