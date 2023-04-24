using UnityEngine;
using System.Linq;
using Crest;

public class PlayerEquipment : MonoBehaviour
{
    //[field: SerializeField, Header("GogglesEquipped")]
    //public string KeyItemTag { get; private set; } = "Goggles";

    //public bool hasGogglesEquipped()
    //{
    //    return InventoryManager.Instance.PlayerInventory.Any(item => item.ItemTag == KeyItemTag);
    //}

    // I need to capture the goggles only when they got bought the first time otherwise it's really unperformant

    public void putOnPlayerGoggles()
    {
        UnderwaterRenderer.Instance._depthFogDensityFactor = .06f;
    }

}
