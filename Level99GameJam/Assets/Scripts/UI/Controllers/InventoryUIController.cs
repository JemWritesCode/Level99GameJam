using System.Collections.Generic;

using DG.Tweening;

using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryUIController : MonoBehaviour {
  [field: SerializeField, Header("UI")]
  public CanvasGroup InventoryPanel { get; private set; }

  [field: SerializeField, Header("ItemList")]
  public RectTransform ItemListContent { get; private set; }

  [field: SerializeField, Header("ItemSlot")]
  public GameObject ItemSlotTemplate { get; private set; }

  [field: SerializeField, Header("Inventory")]
  public List<InventoryItemData> CurrentInventory { get; private set; } = new();

  [field: Header("Controllers")]

  [field: SerializeField]
  public ItemInfoUIController ItemInfoUI { get; private set; }

  [field: SerializeField]
  public BuySellUIController BuySellUI { get; private set; }

  [field: SerializeField]
  public TreasuryUIController TreasuryUI { get; private set; }

  SUPERCharacter.SUPERCharacterAIO _superCharacterController;

  EventSystem _eventSystem;
  GameObject _selectedItemSlot;

  bool _isVisible = false;
  Sequence _toggleInventoryPanelSequence;

  float _playerCoinsValue = 0f;

  void Awake() {
    ItemSlotTemplate.SetActive(false);
  }

  void Start() {
    GameObject player = GameObject.FindWithTag("Player");
    _superCharacterController = player ? player.GetComponent<SUPERCharacter.SUPERCharacterAIO>() : default;
    _eventSystem = EventSystem.current;

    _isVisible = false;

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

    foreach (InventoryItemData itemData in CurrentInventory) {
      AddItem(itemData);
    }

    ItemInfoUI.ResetPanel();
    BuySellUI.ResetPanel();
    TreasuryUI.ResetPanel();
  }

  void Update() {
    if (Input.GetKeyDown(KeyCode.Tab)) {
      _isVisible = !_isVisible;
      ToggleInventoryPanel(_isVisible);
    }

    if (_selectedItemSlot && _eventSystem.currentSelectedGameObject != _selectedItemSlot) {
      _eventSystem.SetSelectedGameObject(_selectedItemSlot);
    }
  }

  public void CloseInventoryPanel() {
    _isVisible = false;
    ToggleInventoryPanel(_isVisible);
  }

  public void ToggleInventoryPanel(bool toggleOn) {
    if (toggleOn) {
      _toggleInventoryPanelSequence.PlayForward();

      if (_superCharacterController) {
        _superCharacterController.PausePlayer(SUPERCharacter.PauseModes.BlockInputOnly);
      }

      Cursor.lockState = CursorLockMode.Confined;
      Cursor.visible = true;

      Time.timeScale = 0f;
    } else {
      _toggleInventoryPanelSequence.PlayBackwards();

      if (_superCharacterController) {
        _superCharacterController.UnpausePlayer();
      }

      Cursor.lockState = CursorLockMode.Locked;
      Cursor.visible = false;

      Time.timeScale = 1f;
    }
  }

  public void AddItem() {
    InventoryItemData itemData = CurrentInventory[Random.Range(0, CurrentInventory.Count)];
    AddItem(itemData);
  }

  public void AddItem(InventoryItemData itemData) {
    GameObject itemSlot = Instantiate(ItemSlotTemplate, ItemListContent);
    ItemSlotUIController itemSlotController = itemSlot.GetComponent<ItemSlotUIController>();

    bool showBadge = itemData.ItemType == InventoryItemData.InventoryItemType.Upgrade;

    itemSlotController.ItemLabel.text = itemData.ItemName;
    itemSlotController.ItemImage.sprite = itemData.ItemSprite;
    itemSlotController.ItemButton.onClick.AddListener(
        () => {
          _selectedItemSlot = itemSlot;
          ItemInfoUI.SetPanel(itemData.ItemName, itemData.ItemDescription, showBadge);

          BuySellUI.BuySellButton.onClick.RemoveAllListeners();

          if (itemData.ItemType != InventoryItemData.InventoryItemType.Loot) {
            BuySellUI.HidePanel();
          } else {
            BuySellUI.SetPanel((int) itemData.ItemCost, itemData.ItemCost < 500);
            BuySellUI.BuySellButton.onClick.AddListener(() => SellItem(itemData));
          }
        });

    itemSlotController.ItemBadge.SetActive(showBadge);

    itemSlot.SetActive(true);
  }

  public void SellItem(InventoryItemData itemData) {
    _playerCoinsValue += itemData.ItemCost;
    TreasuryUI.SetCoinsValue(Mathf.RoundToInt(_playerCoinsValue));
  }
}
