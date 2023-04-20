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

  [field: SerializeField, Header("ItemData")]
  public InventoryItemData[] ItemData { get; private set; }

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

  void Start() {
    GameObject player = GameObject.FindWithTag("Player");
    _superCharacterController = player.GetComponent<SUPERCharacter.SUPERCharacterAIO>();
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

    foreach (InventoryItemData itemData in ItemData) {
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

  public void ToggleInventoryPanel(bool toggleOn) {
    if (toggleOn) {
      _toggleInventoryPanelSequence.PlayForward();
      _superCharacterController.PausePlayer(SUPERCharacter.PauseModes.BlockInputOnly);

      Cursor.lockState = CursorLockMode.Confined;
      Cursor.visible = true;

      Time.timeScale = 0f;
    } else {
      _toggleInventoryPanelSequence.PlayBackwards();
      _superCharacterController.UnpausePlayer();

      Cursor.lockState = CursorLockMode.Locked;
      Cursor.visible = false;

      Time.timeScale = 1f;
    }
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
          ItemInfoUI.SetPanel(itemData.ItemName, itemData.ItemDescription);
          BuySellUI.SetPanel((int) itemData.ItemCost, itemData.ItemCost < 500);

          BuySellUI.BuySellButton.onClick.RemoveAllListeners();
          BuySellUI.BuySellButton.onClick.AddListener(() => SellItem(itemData));
        });

    itemSlot.SetActive(true);
  }

  public void SellItem(InventoryItemData itemData) {
    _playerCoinsValue += itemData.ItemCost;
    TreasuryUI.SetCoinsValue(Mathf.RoundToInt(_playerCoinsValue));
  }
}
