using DG.Tweening;

using UnityEngine;

public class LadderMove : MonoBehaviour {
  [field: SerializeField]
  public Vector3 MoveToPosition { get; private set; }

  [field: SerializeField, Min(0f)]
  public float ClimbDuration { get; private set; } = 1f;

  [field: SerializeField, Min(0f)]
  public float MoveDuration { get; private set; } = 0.5f;

  Collider _ladderCollider;
  Collider _playerCollider;

  private void Start() {
    _ladderCollider = GetComponent<Collider>();
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

    Vector3 ladderPosition = _ladderCollider.ClosestPointOnBounds(_playerCollider.transform.position);

    DOTween.Sequence()
        .Insert(0f, _playerCollider.transform.DOMove(ladderPosition, 0.5f).SetEase(Ease.Linear))
        .Append(_playerCollider.transform.DOMoveY(MoveToPosition.y, ClimbDuration).SetEase(Ease.Linear))
        .Append(_playerCollider.transform.DOMove(MoveToPosition, MoveDuration).SetEase(Ease.Linear))
        .AppendCallback(
            () => {
              _isMovingPlayer = false;
              _playerCollider.enabled = true;
            });
  }
}
