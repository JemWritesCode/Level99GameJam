using System.Collections.Generic;

using DG.Tweening;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryUIController : MonoBehaviour {
  [field: SerializeField, Header("UI")]
  public CanvasGroup InventoryPanel { get; private set; }

  [field: SerializeField, Header("Title")]
  public TMPro.TMP_Text TitleText { get; private set; }

  [field: SerializeField]
  public Image TitleBackground { get; private set; }

  [field: SerializeField, Header("ItemList")]
  public RectTransform ItemListContent { get; private set; }

  [field: SerializeField, Header("ItemSlot")]
  public GameObject ItemSlotTemplate { get; private set; }

  [field: SerializeField, Header("ShopItemList")]
  public GameObject ShopItemList { get; private set; }

  [field: SerializeField]
  public RectTransform ShopItemListContent { get; private set; }

  [field: Header("Controllers")]

  [field: SerializeField]
  public ItemInfoUIController ItemInfoUI { get; private set; }

  [field: SerializeField]
  public BuySellUIController BuySellUI { get; private set; }

  [field: SerializeField]
  public TreasuryUIController TreasuryUI { get; private set; }

  [field: SerializeField]
  public DialogUIController DialogUI { get; private set; }

  [field: SerializeField, Header("Help")]
  public CanvasGroup HelpPanel { get; private set; }

  EventSystem _eventSystem;

  readonly List<GameObject> _playerItemSlots = new();
  readonly List<GameObject> _shopItemSlots = new();
  GameObject _selectedItemSlot;

  public bool IsVisible { get; private set; } = false;
  public bool IsShopVisible { get; private set; } = false;

  Sequence _toggleInventoryPanelSequence;

  void Awake() {
    ItemSlotTemplate.SetActive(false);
  }

  void Start() {
    _eventSystem = EventSystem.current;

    IsVisible = false;
    IsShopVisible = false;

    InventoryPanel.alpha = 0f;
    InventoryPanel.blocksRaycasts = false;

    _toggleInventoryPanelSequence =
        DOTween.Sequence()
            .InsertCallback(0f, () => InventoryPanel.blocksRaycasts = false)
            .Insert(0f, InventoryPanel.DOFade(1f, 0.25f))
            .Insert(0f, InventoryPanel.transform.DOLocalMoveX(25f, 0.25f).SetRelative(true))
            .Insert(0f, HelpPanel.DOFade(1f, 0.25f))
            .InsertCallback(0.25f, () => InventoryPanel.blocksRaycasts = true)
            .SetLink(InventoryPanel.gameObject)
            .SetId(InventoryPanel.gameObject)
            .SetAutoKill(false)
            .Pause();

    ItemInfoUI.ResetPanel();
    BuySellUI.ResetPanel();
    TreasuryUI.ResetPanel();
    DialogUI.ResetDialog();
  }

  void Update() {
    if (IsVisible && _selectedItemSlot && _eventSystem.currentSelectedGameObject != _selectedItemSlot) {
      _eventSystem.SetSelectedGameObject(_selectedItemSlot);
    }
  }

  public void CloseInventoryPanel() {
    ToggleInventoryPanel(false);
  }

  public void ToggleInventoryPanel(bool toggleOn, bool isShopping = false) {
    IsVisible = toggleOn;

    if (toggleOn) {
      SetInventoryPanelState();
      ToggleShopPanel(isShopping);

      _toggleInventoryPanelSequence.PlayForward();

      InputManager.Instance.UnlockCursor();
      Time.timeScale = 0f;
    } else {
      _toggleInventoryPanelSequence.PlayBackwards();

      InputManager.Instance.LockCursor();
      Time.timeScale = 1f;
    }
  }

  void SetInventoryPanelState() {
    foreach (GameObject itemSlot in _playerItemSlots) {
      Destroy(itemSlot);
    }

    _playerItemSlots.Clear();

    foreach (GameObject itemSlot in _shopItemSlots) {
      Destroy(itemSlot);
    }

    _shopItemSlots.Clear();

    foreach (InventoryItemData itemData in InventoryManager.Instance.PlayerInventory) {
      AddPlayerItem(itemData);
    }

    foreach (InventoryItemData itemData in InventoryManager.Instance.ShopInventory) {
      AddShopItem(itemData);
    }

    ItemInfoUI.ResetPanel();
    BuySellUI.ResetPanel();
    TreasuryUI.ResetPanel((int) InventoryManager.Instance.PlayerCurrentCoins);
  }

  void ToggleShopPanel(bool toggleOn) {
    IsShopVisible = toggleOn;

    ShopItemList.SetActive(toggleOn);
    TitleText.SetText(toggleOn ? "Shopping" : "Inventory");
    InventoryPanel.RectTransform().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, toggleOn ? 640f : 450f);
  }

  public GameObject AddPlayerItem(InventoryItemData itemData) {
    GameObject itemSlot = Instantiate(ItemSlotTemplate, ItemListContent);
    ItemSlotUIController itemSlotUI = itemSlot.GetComponent<ItemSlotUIController>();
    itemSlotUI.SetupItemSlot(itemData);
    itemSlotUI.ItemButton.onClick.AddListener(() => OnPlayerItemClicked(itemSlot, itemData));

    itemSlot.SetActive(true);

    _playerItemSlots.Add(itemSlot);
    return itemSlot;
  }

  public void OnPlayerItemClicked(GameObject itemSlot, InventoryItemData itemData) {
    _selectedItemSlot = itemSlot;

    ItemInfoUI.SetPanel(
        itemData.ItemName,
        itemData.ItemDescription,
        showBadge: itemData.ItemType == InventoryItemData.InventoryItemType.Equipment);

    BuySellUI.BuySellButton.onClick.RemoveAllListeners();

    if (IsShopVisible && itemData.ItemType == InventoryItemData.InventoryItemType.Loot) {
      BuySellUI.BuySellButtonLabel.text = "Sell";
      BuySellUI.SetPanel((int) itemData.ItemCost, canBuySell: true);
      BuySellUI.BuySellButton.onClick.AddListener(() => SellPlayerItem(itemSlot, itemData));
    } else if (itemData.ItemType == InventoryItemData.InventoryItemType.Clue && itemData.ClueDialogData) {
      BuySellUI.BuySellButtonLabel.text = "Read";
      BuySellUI.SetPanel((int) itemData.ItemCost, canBuySell: true, isClue: true);
      BuySellUI.BuySellButton.onClick.AddListener(() => DialogUI.OpenDialog(itemData.ClueDialogData));
    } else {
      BuySellUI.HidePanel();
    }
  }

  public void SellPlayerItem(GameObject itemSlot, InventoryItemData itemData) {
    InventoryManager.Instance.PlayerInventory.Remove(itemData);
    InventoryManager.Instance.PlayerCurrentCoins += itemData.ItemCost;
    TreasuryUI.SetCoinsValue(Mathf.RoundToInt(InventoryManager.Instance.PlayerCurrentCoins));

    _playerItemSlots.Remove(itemSlot);
    Destroy(itemSlot);

    ItemInfoUI.HidePanel();
    BuySellUI.HidePanel();
  }

  public GameObject AddShopItem(InventoryItemData itemData) {
    GameObject itemSlot = Instantiate(ItemSlotTemplate, ShopItemListContent);
    ItemSlotUIController itemSlotUI = itemSlot.GetComponent<ItemSlotUIController>();
    itemSlotUI.SetupItemSlot(itemData);
    itemSlotUI.ItemBadge.SetActive(false);
    itemSlotUI.ItemButton.onClick.AddListener(() => OnShopItemClicked(itemSlot, itemData));

    itemSlot.SetActive(true);

    _shopItemSlots.Add(itemSlot);
    return itemSlot;
  }

  public void OnShopItemClicked(GameObject itemSlot, InventoryItemData itemData) {
    _selectedItemSlot = itemSlot;
    ItemInfoUI.SetPanel(itemData.ItemName, itemData.ItemDescription, showBadge: false);

    BuySellUI.BuySellButtonLabel.text = "Buy";
    BuySellUI.SetPanel((int) itemData.ItemCost, InventoryManager.Instance.PlayerCurrentCoins >= itemData.ItemCost);

    BuySellUI.BuySellButton.onClick.RemoveAllListeners();
    BuySellUI.BuySellButton.onClick.AddListener(() => BuyShopItem(itemSlot, itemData));
  }

  public void BuyShopItem(GameObject itemSlot, InventoryItemData itemData) {
    InventoryManager.Instance.ShopInventory.Remove(itemData);
    InventoryManager.Instance.AddToInventory(itemData);
    InventoryManager.Instance.PlayerCurrentCoins -= itemData.ItemCost;
    TreasuryUI.SetCoinsValue(Mathf.RoundToInt(InventoryManager.Instance.PlayerCurrentCoins));

    _shopItemSlots.Remove(itemSlot);
    Destroy(itemSlot);

    GameObject playerItemSlot = AddPlayerItem(itemData);
    ItemSlotUIController playerItemSlotUI = playerItemSlot.GetComponent<ItemSlotUIController>();
    playerItemSlotUI.ItemBadge.transform.DOPunchScale(new(0.5f, 0.5f, 0.5f), 0.25f, 2, 0);

    OnPlayerItemClicked(playerItemSlot, itemData);
  }
}
