using UnityEngine;
using UnityEngine.EventSystems;

public class OffsetPositionOnPressed : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {
  [field: SerializeField]
  public RectTransform TargetTransform { get; private set; }

  [field: SerializeField]
  public Vector2 Offset { get; private set; }

  void IPointerDownHandler.OnPointerDown(PointerEventData eventData) {
    TargetTransform.anchoredPosition += Offset;
  }

  void IPointerUpHandler.OnPointerUp(PointerEventData eventData) {
    TargetTransform.anchoredPosition -= Offset;
  }
}
