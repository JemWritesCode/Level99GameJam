using System.Collections.Generic;

using UnityEngine;

public class InventoryManager : MonoBehaviour {
  [field: SerializeField, Header("Coins")]
  public float PlayerCurrentCoins { get; set; } = 0f;

  [field: SerializeField, Header("Inventory")]
  public List<InventoryItemData> PlayerInventory { get; private set; } = new();

  [field: SerializeField]
  public List<InventoryItemData> ShopInventory { get; private set; } = new();

  static InventoryManager _instance;

    GameObject player;

  public static InventoryManager Instance {
    get {
      if (!_instance) {
        _instance = FindObjectOfType<InventoryManager>();
      }

      if (!_instance) {
        GameObject manager = new("InventoryManager");
        _instance = manager.AddComponent<InventoryManager>();
        DontDestroyOnLoad(manager);
      }

      return _instance;
    }
  }

    public void AddToInventory(InventoryItemData itemToAdd)
    {
        PlayerInventory.Add(itemToAdd);
        if(itemToAdd.ItemTag == "Goggles")
        {
            player = GameObject.FindGameObjectWithTag("Player");
            player.GetComponent<PlayerEquipment>().putOnPlayerGoggles();
        }
    }

  void Awake() {
    if (!_instance) {
      _instance = this;
      DontDestroyOnLoad(gameObject);
    } else {
      Destroy(this);
    }
  }
}
