using UnityEngine;

public class InputManager : MonoBehaviour {
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
