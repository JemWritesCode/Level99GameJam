using DG.Tweening;

using System.Collections;

using UnityEngine;

public class RestartPlayer : MonoBehaviour {
  [field: SerializeField]
  public Vector3 RestartPosition { get; private set; }

  Camera _camera;
  Collider _playerCollider;

  private void Start() {
    _camera = Camera.main;
    _playerCollider = GameObject.FindGameObjectWithTag("Player").GetComponent<Collider>();

    OxygenManager.Instance.OxygenEmptyEvent += OnOxygenEmptyEvent;
  }

  bool _isRestartingPlayer = false;

  void OnOxygenEmptyEvent(object sender, bool isBelowOceanSurface) {
    if (!isBelowOceanSurface || _isRestartingPlayer) {
      return;
    }

    _isRestartingPlayer = true;

    Debug.Log($"Restarting player from position: {_playerCollider.transform.position} to: {RestartPosition}");

    DOTween.Sequence()
        .Insert(0f, _camera.DOShakePosition(3f, randomnessMode: ShakeRandomnessMode.Harmonic))
        .InsertCallback(2f, () => _playerCollider.enabled = false)
        .Insert(2f, _playerCollider.transform.DOMove(RestartPosition, 1f))
        .InsertCallback(3f, () => _playerCollider.enabled = true)
        .InsertCallback(4f, () => _isRestartingPlayer = false)
        .SetTarget(_playerCollider);
  }
}
