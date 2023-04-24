using UnityEngine;

public class InteractableHoverText : MonoBehaviour {
  [field: SerializeField]
  public float HoverDistance { get; private set; } = 2f;

  [field: SerializeField]
  public string HoverText { get; private set; } = "...";
}
