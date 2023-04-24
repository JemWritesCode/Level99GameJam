using UnityEngine;

public class InteractManager : MonoBehaviour {
  static InteractManager _instance;

  public static InteractManager Instance {
    get {
      if (!_instance) {
        _instance = FindObjectOfType<InteractManager>();
      }

      if (!_instance) {
        GameObject manager = new("InteractManager");
        _instance = manager.AddComponent<InteractManager>();
        DontDestroyOnLoad(manager);
      }

      return _instance;
    }
  }

  void Awake() {
    if (_instance) {
      Destroy(this);
    } else {
      _instance = this;
      DontDestroyOnLoad(gameObject);
    }
  }

  void Start() {
    _mainCamera = Camera.main;
  }

  Camera _mainCamera;
  RaycastHit[] _raycastHits = new RaycastHit[10];

  void FixedUpdate() {
    if (!_interactUI) {
      return;
    }

    int count =
        Physics.SphereCastNonAlloc(
            _mainCamera.transform.position,
            0.25f,
            _mainCamera.transform.forward,
            _raycastHits,
            4f,
            -1,
            QueryTriggerInteraction.Ignore);

    if (count <= 0) {
      return;
    }

    for (int i = 0; i < count; i++) {
      if (_raycastHits[i].collider.TryGetComponent(out InteractableHoverText hoverText)
          && _raycastHits[i].distance <= hoverText.HoverDistance) {
        _interactUI.SetInteractable(hoverText);
        return;
      }
    }

    _interactUI.SetInteractable(default);
  }

  InteractUIController _interactUI;

  public void SetInteractUI(InteractUIController interactUI) {
    _interactUI = interactUI;
  }
}