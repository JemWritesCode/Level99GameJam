using System.Collections;

using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class RestartPlayer : MonoBehaviour {
  [field: SerializeField]
  public Vector3 RestartPosition { get; private set; }

  Rigidbody _rigidBody;

  private IEnumerator Start() {
    yield return null;

    _rigidBody = GetComponent<Rigidbody>();
    OxygenManager.Instance.OxygenEmptyEvent += OnOxygenEmptyEvent;
  }

  void OnOxygenEmptyEvent(object sender, bool isBelowOceanSurface) {

  }
}
