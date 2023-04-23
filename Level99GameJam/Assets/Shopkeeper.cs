using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shopkeeper : MonoBehaviour
{
    GameObject inventoryUIController;
    InventoryUIController inventoryUIControllerComponent;

    private void Start()
    {
        inventoryUIController = GameObject.Find("InventoryUIController");
        inventoryUIControllerComponent = inventoryUIController.GetComponent<InventoryUIController>();
    }

    public void openShop()
    {
        inventoryUIControllerComponent.ToggleInventoryPanel(true, true);
    }
}
