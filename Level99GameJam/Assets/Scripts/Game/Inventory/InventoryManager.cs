using System.Collections.Generic;

using UnityEngine;

public class InventoryManager : MonoBehaviour {
  [field: SerializeField]
  public List<InventoryItemData> PlayerInventory { get; private set; } = new();

  [field: SerializeField]
  public List<InventoryItemData> ShopInventory { get; private set; } = new();
}
