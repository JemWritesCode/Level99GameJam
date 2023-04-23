using System.Collections;

using UnityEngine;

public class OxygenManager : MonoBehaviour {
  [field: SerializeField, Header("Oxygen")]
  public float OxygenMaxValue { get; private set; }

  [field: SerializeField]
  public float OxygenCurrentValue { get; private set; }

  [field: SerializeField, Header("Tick")]
  public float OxygenTickInterval { get; private set; }

  [field: SerializeField]
  public float OxygenTickValue { get; private set; }

  static OxygenManager _instance = null;

  public static OxygenManager Instance {
    get {
      if (!_instance) {
        _instance = FindObjectOfType<OxygenManager>();
      }

      if (!_instance) {
        GameObject manager = new("OxygenManager");
        _instance = manager.AddComponent<OxygenManager>();
      }

      return _instance;
    }
  }

  void Awake() {
    if (!_instance) {
      _instance = this;
    } else {
      Destroy(this);
    }
  }

  float _timeSinceLastTick = 0f;

  void Update() {
    if (_isBelowOceanSurface) {
      _timeSinceLastTick += Time.deltaTime;

      if (_timeSinceLastTick >= OxygenTickInterval) {
        _timeSinceLastTick = 0f;
        OxygenCurrentValue = Mathf.Clamp(OxygenCurrentValue - OxygenTickValue, 0f, OxygenMaxValue);

        if (_oxygenUI) {
          _oxygenUI.SetOxygenPercent(OxygenCurrentValue / OxygenMaxValue);
        }
      }
    } else if (OxygenCurrentValue < OxygenMaxValue) {
      _timeSinceLastTick += Time.deltaTime;

      if (_timeSinceLastTick >= 1f) {
        _timeSinceLastTick = 0f;
        OxygenCurrentValue = Mathf.Clamp(OxygenCurrentValue + (OxygenMaxValue * 0.25f), 0f, OxygenMaxValue);

        if (_oxygenUI) {
          _oxygenUI.SetOxygenPercent(OxygenCurrentValue / OxygenMaxValue);
        }
      }
    } else {
      _timeSinceLastTick = 0f;
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
      } else if (OxygenCurrentValue >= OxygenMaxValue) {
        _oxygenUI.HideOxygenPanel();
      }
    }
  }
}
