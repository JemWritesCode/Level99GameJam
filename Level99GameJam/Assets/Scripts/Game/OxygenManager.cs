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

  [field: SerializeField, Header("Recover")]
  public float OxygenRecoverValue { get; private set; }

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
        OxygenCurrentValue = Mathf.Clamp(OxygenCurrentValue + OxygenRecoverValue, 0f, OxygenMaxValue);

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
    float oxygenAtFullTime = 0f;

    while (_oxygenUI) {
      yield return null;

      if (_isBelowOceanSurface == _oxygenUI.IsVisible()) {
        continue;
      } else if (_isBelowOceanSurface) {
        _oxygenUI.ShowOxygenPanel();
        oxygenAtFullTime = 0f;
      } else if (_oxygenUI.IsVisible()) {
        if (OxygenCurrentValue < OxygenMaxValue) {
          oxygenAtFullTime = 0f;
        } else {
          oxygenAtFullTime += Time.deltaTime;

          if (oxygenAtFullTime > 3f) {
            _oxygenUI.HideOxygenPanel();
          }
        }
      }
    }
  }
}
