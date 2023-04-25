using DG.Tweening;

using UnityEngine;

public class LadderMove : MonoBehaviour {
  [field: SerializeField]
  public Vector3 MoveToPosition { get; private set; }

  Collider _playerCollider;

  private void Start() {
    _playerCollider = GameObject.FindGameObjectWithTag("Player").GetComponent<Collider>();
  }

  bool _isMovingPlayer = false;

  public void MovePlayer() {
    if (_isMovingPlayer) {
      return;
    }

    Debug.Log("Moving player!");

    _isMovingPlayer = true;
    _playerCollider.enabled = false;

    DOTween.Sequence()
        .Insert(0f, _playerCollider.transform.DOMove(MoveToPosition, 1f))
        .InsertCallback(
            1f,
            () => {
              _isMovingPlayer = false;
              _playerCollider.enabled = true;
            });
  }
}
