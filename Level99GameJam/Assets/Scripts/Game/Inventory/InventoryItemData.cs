using UnityEngine;

[CreateAssetMenu(menuName = "Inventory/ItemData", order = 1)]
public class InventoryItemData : ScriptableObject {
  public enum InventoryItemType {
    None,
    Loot,
    Equipment,
    Clue
  }

  [field: SerializeField]
  public InventoryItemType ItemType { get; private set; }

  [field: SerializeField]
  public string ItemTag { get; private set; } = string.Empty;

  [field: SerializeField]
  public float ItemCost { get; private set; }

  [field: Header("UI")]

  [field: SerializeField]
  public Sprite ItemSprite { get; private set; }

  [field: SerializeField]
  public Color ItemSpriteColor { get; private set; } = Color.white;

  [field: SerializeField]
  public string ItemName { get; private set; }

  [field: SerializeField, TextArea()]
  public string ItemDescription { get; private set; }
}
