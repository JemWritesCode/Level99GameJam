using System.Collections;

using UnityEngine;

public class OxygenManager : MonoBehaviour {

  static OxygenManager _instance;

  public static OxygenManager Instance {
    get {
      if (!_instance) {
        _instance = FindObjectOfType<OxygenManager>();
      }

      if (!_instance) {
        GameObject manager = new("OxygenManager");
        _instance = manager.AddComponent<OxygenManager>();
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

  OxygenUIController _oxygenUI;

  public void SetOxygenUI(OxygenUIController oxygenUI) {
    _oxygenUI = oxygenUI;
    _oxygenUI.StartCoroutine(UpdateOxygenUI());
  }

  bool _isBelowOceanSurface = false;

  public void OnBelowOceanSurface() {
    _isBelowOceanSurface = true;
  }

  public void OnAboveOceanSurface() {
    _isBelowOceanSurface = false;
  }

  IEnumerator UpdateOxygenUI() {
    WaitForSeconds waitInterval = new(seconds: 0.5f);

    while (_oxygenUI) {
      yield return waitInterval;

      if (_isBelowOceanSurface == _oxygenUI.IsVisible()) {
        continue;
      } else if (_isBelowOceanSurface) {
        _oxygenUI.ShowOxygenPanel();
      } else {
        _oxygenUI.HideOxygenPanel();
      }
    }
  }
}
