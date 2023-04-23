using UnityEngine;
using UnityEngine.UI;

public class ItemSlotUIController : MonoBehaviour {
  [field: SerializeField]
  public Image ItemImage { get; private set; }

  [field: SerializeField]
  public TMPro.TMP_Text ItemLabel { get; private set; }

  [field: SerializeField]
  public Button ItemButton { get; private set; }

  [field: SerializeField, Header("Badge")]
  public GameObject ItemBadge { get; private set; }

  [field: SerializeField]
  public Image BadgeImage { get; private set; }

  [field: SerializeField]
  public TMPro.TMP_Text BadgeLabel { get; private set; }

  public void SetupItemSlot(InventoryItemData itemData) {
    ItemLabel.text = itemData.ItemName;
    ItemImage.sprite = itemData.ItemSprite;
    ItemImage.color = itemData.ItemSpriteColor;

    SetupItemBadge(itemData);
  }

  void SetupItemBadge(InventoryItemData itemData) {
    switch (itemData.ItemType) {
      case InventoryItemData.InventoryItemType.Equipment:
        ItemBadge.SetActive(true);
        BadgeImage.color = new Color(0.4313726f, 0.4039216f, 0.3686275f);
        BadgeLabel.text = "Eq";
        break;

      case InventoryItemData.InventoryItemType.Clue:
        ItemBadge.SetActive(true);
        BadgeImage.color = new(0.7075472f, 0.4959733f, 0f);
        BadgeLabel.text = "!";
        break;

      default:
        ItemBadge.SetActive(false);
        break;
    }
  }
}
