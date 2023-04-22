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

  readonly List<GameObject> _itemSlots = new();
  GameObject _selectedItemSlot;

  bool _isVisible = false;
  bool _isShopVisible = false;

  Sequence _toggleInventoryPanelSequence;

  void Awake() {
    ItemSlotTemplate.SetActive(false);
  }

  void Start() {
    GameObject player = GameObject.FindWithTag("Player");
    _superCharacterController = player ? player.GetComponent<SUPERCharacter.SUPERCharacterAIO>() : default;
    _eventSystem = EventSystem.current;

    _isVisible = false;
    _isShopVisible = false;

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

    ItemInfoUI.ResetPanel();
    BuySellUI.ResetPanel();
    TreasuryUI.ResetPanel();
  }

  void Update() {
    if (Input.GetKeyDown(KeyCode.Tab)) {
      ToggleInventoryPanel(!_isVisible, Input.GetKey(KeyCode.LeftShift));
    }

    if (_selectedItemSlot && _eventSystem.currentSelectedGameObject != _selectedItemSlot) {
      _eventSystem.SetSelectedGameObject(_selectedItemSlot);
    }
  }

  public void CloseInventoryPanel() {
    ToggleInventoryPanel(false);
  }

  public void ToggleInventoryPanel(bool toggleOn, bool isShopping = false) {
    _isVisible = toggleOn;

    if (toggleOn) {
      SetInventoryPanelState();
      ToggleShopPanel(isShopping);

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

  void SetInventoryPanelState() {
    foreach (GameObject itemSlot in _itemSlots) {
      Destroy(itemSlot);
    }

    _itemSlots.Clear();

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
    _isShopVisible = toggleOn;

    ShopItemList.SetActive(toggleOn);
    TitleText.SetText(toggleOn ? "Shopping" : "Inventory");
    InventoryPanel.RectTransform().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, toggleOn ? 640f : 450f);
  }

  public GameObject AddPlayerItem(InventoryItemData itemData) {
    GameObject itemSlot = Instantiate(ItemSlotTemplate, ItemListContent);
    ItemSlotUIController itemSlotUI = itemSlot.GetComponent<ItemSlotUIController>();

    itemSlotUI.ItemLabel.text = itemData.ItemName;
    itemSlotUI.ItemImage.sprite = itemData.ItemSprite;
    itemSlotUI.ItemButton.onClick.AddListener(() => OnPlayerItemClicked(itemSlot, itemData));
    itemSlotUI.ItemBadge.SetActive(itemData.ItemType == InventoryItemData.InventoryItemType.Equipment);

    itemSlot.SetActive(true);

    _itemSlots.Add(itemSlot);
    return itemSlot;
  }

  public void OnPlayerItemClicked(GameObject itemSlot, InventoryItemData itemData) {
    _selectedItemSlot = itemSlot;

    ItemInfoUI.SetPanel(
        itemData.ItemName,
        itemData.ItemDescription,
        showBadge: itemData.ItemType == InventoryItemData.InventoryItemType.Equipment);

    BuySellUI.BuySellButton.onClick.RemoveAllListeners();

    if (_isShopVisible && itemData.ItemType == InventoryItemData.InventoryItemType.Loot) {
      BuySellUI.BuySellButtonLabel.text = "Sell";
      BuySellUI.SetPanel((int) itemData.ItemCost, canBuySell: true);
      BuySellUI.BuySellButton.onClick.AddListener(() => SellPlayerItem(itemSlot, itemData));
    } else {
      BuySellUI.HidePanel();
    }
  }

  public void SellPlayerItem(GameObject itemSlot, InventoryItemData itemData) {
    InventoryManager.Instance.PlayerInventory.Remove(itemData);
    InventoryManager.Instance.PlayerCurrentCoins += itemData.ItemCost;
    TreasuryUI.SetCoinsValue(Mathf.RoundToInt(InventoryManager.Instance.PlayerCurrentCoins));

    Destroy(itemSlot);

    ItemInfoUI.HidePanel();
    BuySellUI.HidePanel();
  }

  public GameObject AddShopItem(InventoryItemData itemData) {
    GameObject itemSlot = Instantiate(ItemSlotTemplate, ShopItemListContent);
    ItemSlotUIController itemSlotUI = itemSlot.GetComponent<ItemSlotUIController>();

    itemSlotUI.ItemLabel.text = itemData.ItemName;
    itemSlotUI.ItemImage.sprite = itemData.ItemSprite;
    itemSlotUI.ItemBadge.SetActive(false);
    itemSlotUI.ItemButton.onClick.AddListener(() => OnShopItemClicked(itemSlot, itemData));

    itemSlot.SetActive(true);

    _itemSlots.Add(itemSlot);
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
    InventoryManager.Instance.PlayerInventory.Add(itemData);
    InventoryManager.Instance.PlayerCurrentCoins -= itemData.ItemCost;
    TreasuryUI.SetCoinsValue(Mathf.RoundToInt(InventoryManager.Instance.PlayerCurrentCoins));

    Destroy(itemSlot);
    OnPlayerItemClicked(AddPlayerItem(itemData), itemData);
  }
}
