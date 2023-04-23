using DG.Tweening;

using System.Linq;

using UnityEngine;

public class MoveTheHatch : MonoBehaviour
{
    [field: SerializeField]
    public Vector3 HatchMoveToCoords { get; private set; }

    [SerializeField] AudioSource hatchMoveSound;

    Sequence _moveHatchToSideSequence;
    bool _isHatchOpened;

    private void Start() {
      _moveHatchToSideSequence =
          DOTween.Sequence()
              .Insert(0f, transform.DOMove(HatchMoveToCoords, 2f))
              .SetTarget(gameObject)
              .SetLink(gameObject)
              .SetAutoKill(false)
              .Pause();

      _isHatchOpened = false;
    }

    public void MoveHatchToSide()
    {
        if (!CanMoveHatch()) {
          Debug.Log($"Missing item with tag: {KeyItemTag}");
          return;
        }

        Debug.Log("You interacted with the hatch!");

        if (_isHatchOpened) {
          _moveHatchToSideSequence.PlayBackwards();
          _isHatchOpened = false;
        } else {
          _moveHatchToSideSequence.PlayForward();
          _isHatchOpened = true;
        }

        hatchMoveSound.Play();
    }

  [field: SerializeField, Header("KeyItem")]
  public string KeyItemTag { get; private set; } = "Crowbar";

  public bool CanMoveHatch() {
    return InventoryManager.Instance.PlayerInventory.Any(item => item.ItemTag == KeyItemTag);
  }
}
