using System.Collections;

using UnityEngine;

public class InteractableHoverText : MonoBehaviour {
  [field: SerializeField]
  public float HoverDistance { get; private set; } = 2f;

  InteractUIController _interactUI;

  private void Start() {
    _interactUI = FindObjectOfType<InteractUIController>();
    StartCoroutine(UpdateHoverText());
  }

  IEnumerator UpdateHoverText() {
    GameObject player = GameObject.FindGameObjectWithTag("Player");
    WaitForSeconds waitInterval = new(seconds: 0.25f);

    while (true) {
      yield return waitInterval;

      if (Vector3.Distance(player.transform.position, gameObject.transform.position) < HoverDistance) {
        if (!_interactUI.IsVisible) {
          _interactUI.ShowInteractPanel("A crowbar would be useful here.");
        }
      } else {
        if (_interactUI.IsVisible) {
          _interactUI.HideInteractPanel();
        }
      }
    }
  }
}
