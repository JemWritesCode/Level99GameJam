using UnityEngine;

public class InputManager : MonoBehaviour {
  [field: SerializeField, Header("UIControllers")]
  public InventoryUIController InventoryUI { get; private set; }

  [field: SerializeField]
  public MenuUIController MenuUI { get; private set; }

  [field: SerializeField]
  public OxygenUIController OxygenUI { get; private set; }

  static InputManager _instance;

  public static InputManager Instance {
    get {
      if (!_instance) {
        _instance = FindObjectOfType<InputManager>();
      }

      if (!_instance) {
        GameObject manager = new("InputManager");
        _instance = manager.AddComponent<InputManager>();
        DontDestroyOnLoad(manager);
      }

      return _instance;
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

#if UNITY_EDITOR
  public const KeyCode ToggleMenuKey = KeyCode.F2;
#else
  public const KeyCode ToggleMenuKey = KeyCode.F3;
#endif

  void Update() {
    if (Input.GetKeyDown(ToggleMenuKey)) {
      bool toggleOn = !MenuUI.MenuPanel.activeSelf;

      if (toggleOn && InventoryUI.IsVisible) {
        InventoryUI.ToggleInventoryPanel(false);
      }

      MenuUI.ToggleMenu(toggleOn, shouldSkipLockUnlock: CurrentDialogUI);
    }

    if (Input.GetKeyDown(KeyCode.Tab) && !MenuUI.MenuPanel.activeSelf && !CurrentDialogUI) {
      InventoryUI.ToggleInventoryPanel(!InventoryUI.IsVisible, Input.GetKey(KeyCode.LeftShift));
    }
  }

  public DialogUIController CurrentDialogUI { get; set; }

  SUPERCharacter.SUPERCharacterAIO _playerCharacterController;

  public SUPERCharacter.SUPERCharacterAIO PlayerCharacterController {
    get {
      if (!_playerCharacterController) {
        _playerCharacterController =
          GameObject.FindGameObjectWithTag("Player").GetComponent<SUPERCharacter.SUPERCharacterAIO>();
      }

      return _playerCharacterController;
    }
  }

  public void LockCursor() {
    PlayerCharacterController.UnpausePlayer();

    Cursor.lockState = CursorLockMode.Locked;
    Cursor.visible = false;
  }

  public void UnlockCursor() {
    PlayerCharacterController.PausePlayer(SUPERCharacter.PauseModes.BlockInputOnly);

    Cursor.lockState = CursorLockMode.Confined;
    Cursor.visible = true;
  }
}
