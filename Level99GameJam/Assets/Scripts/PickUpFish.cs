using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpFish : MonoBehaviour
{
    [SerializeField] InventoryItemData fishType;

    public void grabTheFishie()
    {
        // jem todo look into singletons so you don't have to cache in every script every times

        InventoryManager.Instance.AddToInventory(fishType);
        GameObject.Destroy(this.gameObject);
    }

}
