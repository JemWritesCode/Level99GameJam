using DG.Tweening;

using UnityEngine;
using UnityEngine.UI;

public class InventoryUIController : MonoBehaviour {
  [field: SerializeField, Header("UI")]
  public CanvasGroup InventoryPanel { get; private set; }

  [field: SerializeField, Header("ItemList")]
  public RectTransform ItemListContent { get; private set; }

  [field: SerializeField, Header("ItemSlot")]
  public GameObject ItemSlotTemplate { get; private set; }

  [field: SerializeField, Header("ItemInfo")]
  public CanvasGroup ItemInfoPanel { get; private set; }

  [field: SerializeField]
  public TMPro.TMP_Text ItemInfoItemName { get; private set; }

  [field: SerializeField]
  public TMPro.TMP_Text ItemInfoItemDescription { get; private set; }

  [field: SerializeField, Header("ItemData")]
  public InventoryItemData[] ItemData { get; private set; }

  Sequence _toggleInventoryPanelSequence;

  void Start() {
    InventoryPanel.alpha = 0f;
    InventoryPanel.blocksRaycasts = false;

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

    _toggleInventoryPanelSequence.Complete(true);

    ItemInfoPanel.alpha = 0f;
    AddItem();
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
    ItemSlotUIController itemSlotController = itemSlot.GetComponent<ItemSlotUIController>();

    InventoryItemData itemData = ItemData[Random.Range(0, ItemData.Length)];
    itemSlotController.ItemLabel.text = itemData.ItemName;
    itemSlotController.ItemImage.sprite = itemData.ItemSprite;
    itemSlotController.ItemButton.onClick.AddListener(
        () => {
          SetItemInfoPanel(itemData.name, itemData.ItemDescription);
        });

    itemSlot.SetActive(true);
  }

  public void SetItemInfoPanel(string name, string description) {
    DOTween.Sequence()
        .Insert(0f, ItemInfoPanel.DOFade(0f, 0.10f))
        .InsertCallback(0.10f, () => {
          ItemInfoItemName.text = name;
          ItemInfoItemDescription.text = description;
        })
        .Insert(0.10f, ItemInfoPanel.DOFade(1f, 0.25f));
  }

  public void ClearItemInfoPanel() {
    ItemInfoPanel.DOKill();
    ItemInfoPanel.DOFade(0f, 0.25f);
  }
}
