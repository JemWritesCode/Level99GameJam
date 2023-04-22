using UnityEngine;

public static class UnityExtensions {
  public static RectTransform RectTransform(this Component component) {
    return (RectTransform) component.transform;
  }
}
