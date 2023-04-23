using UnityEngine;

public class Shopkeeper : MonoBehaviour {
  [field: SerializeField]
  public InventoryUIController InventoryUI { get; private set; }

  public void OpenShop() {
    InventoryUI.ToggleInventoryPanel(true, true);
  }
}
