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
  public TMPro.TMP_Text BadgeLabel { get; private set; }
}
