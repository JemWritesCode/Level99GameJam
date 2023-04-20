using Coffee.UIEffects;

using DG.Tweening;

using UnityEngine;
using UnityEngine.EventSystems;
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

  EventSystem _eventSystem;
  GameObject _selectedItemSlot;

  Sequence _toggleInventoryPanelSequence;

  void Awake() {
    _eventSystem = EventSystem.current;
  }

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

    foreach (InventoryItemData itemData in ItemData) {
      AddItem(itemData);
    }

    ResetBuySellPanel();
  }

  void Update() {
    if (Input.GetKeyDown(KeyCode.Tab)) {
      ToggleInventoryPanel();
    }

    if (_selectedItemSlot && _eventSystem.currentSelectedGameObject != _selectedItemSlot) {
      _eventSystem.SetSelectedGameObject(_selectedItemSlot);
    }
  }

  public void ToggleInventoryPanel() {
    _toggleInventoryPanelSequence.Flip();
    _toggleInventoryPanelSequence.Play();
  }

  public void AddItem() {
    InventoryItemData itemData = ItemData[Random.Range(0, ItemData.Length)];
    AddItem(itemData);
  }

  public void AddItem(InventoryItemData itemData) {
    GameObject itemSlot = Instantiate(ItemSlotTemplate, ItemListContent);
    ItemSlotUIController itemSlotController = itemSlot.GetComponent<ItemSlotUIController>();
    
    itemSlotController.ItemLabel.text = itemData.ItemName;
    itemSlotController.ItemImage.sprite = itemData.ItemSprite;
    itemSlotController.ItemButton.onClick.AddListener(
        () => {
          _selectedItemSlot = itemSlot;
          SetItemInfoPanel(itemData.ItemName, itemData.ItemDescription);
          SetBuySellPanel(itemData.ItemCost, itemData.ItemCost < 500);
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

  [field: SerializeField, Header("BuySell")]
  public CanvasGroup BuySellPanel { get; private set; }

  [field: SerializeField]
  public Button BuySellButton { get; private set; }

  [field: SerializeField]
  public TMPro.TMP_Text BuySellButtonLabel { get; private set; }

  [field: SerializeField]
  public UIEffect BuySellButtonDisableEffect { get; private set; }

  [field: SerializeField]
  public UIEffect BuySellCostDisableEffect { get; private set; }

  [field: SerializeField]
  public TMPro.TMP_Text BuySellCostValue { get; private set; }

  [field: SerializeField]
  public TMPro.TMP_Text BuySellCostLabel { get; private set; }

  float _currentCostValue = 0f;

  public void ResetBuySellPanel() {
    BuySellPanel.alpha = 0f;
    BuySellPanel.blocksRaycasts = false;
    BuySellCostLabel.alpha = 0f;

    _currentCostValue = 0f;
  }

  public void SetBuySellPanel(float costValue, bool canBuySell) {
    BuySellPanel.blocksRaycasts = canBuySell;

    DOTween.Complete(BuySellPanel, withCallbacks: true);

    Sequence sequence =
        DOTween.Sequence()
            .SetTarget(BuySellPanel)
            .SetLink(gameObject)
            .Insert(0f, BuySellPanel.DOFade(1f, 0.10f))
            .Insert(0f, BuySellCostValue.DOCounter((int) _currentCostValue, (int) costValue, 0.1f, false))
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
                    () => BuySellCostDisableEffect.effectFactor,
                    x => BuySellCostDisableEffect.effectFactor = x,
                    canBuySell ? 0f : 1f,
                    0.10f));

    if (canBuySell) {
      sequence
          .Insert(0f, BuySellCostLabel.DOFade(0f, 0.1f))
          .Insert(0f, BuySellButtonLabel.DOFade(1f, 0.1f));
    } else {
      sequence
          .Insert(0f, BuySellButtonLabel.DOFade(0.4f, 0.1f))
          .Insert(0f, BuySellCostLabel.transform.DOPunchPosition(new Vector3(0f, 5f, 0f), 0.1f))
          .Insert(0f, BuySellCostLabel.DOFade(1f, 0.1f));
    }

    _currentCostValue = costValue;
  }
}
