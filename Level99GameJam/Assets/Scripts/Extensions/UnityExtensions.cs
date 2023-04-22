using UnityEngine;
using UnityEngine.UI;

public static class UnityExtensions {
  public static RectTransform RectTransform(this Component component) {
    return (RectTransform) component.transform;
  }

  public static Image SetAlpha(this Image image, float alpha) {
    Color color = image.color;
    color.a = alpha;
    image.color = color;

    return image;
  }
}
