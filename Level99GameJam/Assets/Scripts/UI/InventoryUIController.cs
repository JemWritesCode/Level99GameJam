using DG.Tweening;

using UnityEngine;

public class InventoryUIController : MonoBehaviour {
  [field: SerializeField, Header("UI")]
  public CanvasGroup InventoryPanel { get; private set; }

  [field: SerializeField, Header("ItemList")]
  public RectTransform ItemListContent { get; private set; }

  [field: SerializeField, Header("ItemSlot")]
  public GameObject ItemSlotTemplate { get; private set; }

  Sequence _toggleInventoryPanelSequence;

  void Start() {
    _toggleInventoryPanelSequence =
        DOTween.Sequence()
            .InsertCallback(0f, () => InventoryPanel.blocksRaycasts = false)
            .Insert(0f, InventoryPanel.DOFade(1f, 0.25f))
            .Insert(0f, InventoryPanel.transform.DOLocalMoveX(25f, 0.25f).SetRelative(true))
            .InsertCallback(0.25f, () => InventoryPanel.blocksRaycasts = true)
            .SetLink(InventoryPanel.gameObject)
            .SetId(InventoryPanel.gameObject)
            .SetAutoKill(false)
            .Pause();

    _toggleInventoryPanelSequence.Flip();

    InventoryPanel.alpha = 0f;
    InventoryPanel.blocksRaycasts = false;
  }

  void Update() {
    if (Input.GetKeyDown(KeyCode.Tab)) {
      ToggleInventoryPanel();
    }
  }

  public void ToggleInventoryPanel() {
    _toggleInventoryPanelSequence.Flip();
    _toggleInventoryPanelSequence.Play();
  }

  public void AddItem() {
    GameObject itemSlot = Instantiate(ItemSlotTemplate, ItemListContent);
    itemSlot.SetActive(true);
  }
}
