using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupItem : MonoBehaviour
{
    [SerializeField] InventoryItemData itemToPickup;
    [SerializeField] AudioSource pickupSound;
    [SerializeField] GameObject extraItemToDestroy;

    public void grabTheItem()
    {
        InventoryManager.Instance.addToInventory(itemToPickup);
        GameObject.Destroy(this.gameObject);
        if (pickupSound != null)
        {
            pickupSound.Play();
        }
        if(extraItemToDestroy != null)
        {
            Destroy(extraItemToDestroy);
        }
    }

}
