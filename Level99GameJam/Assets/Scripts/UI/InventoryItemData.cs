using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "Inventory/ItemData", order = 1)]
public class InventoryItemData : ScriptableObject {
  [field: SerializeField]
  public Sprite ItemSprite { get; private set; }

  [field: SerializeField]
  public string ItemName { get; private set; }

  [field: SerializeField]
  public string ItemDescription { get; private set; }

  [field: SerializeField]
  public float ItemCost { get; private set; }
}
