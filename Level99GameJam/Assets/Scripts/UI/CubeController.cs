using DG.Tweening;

using UnityEngine;


public class CubeController : MonoBehaviour {
  [field: SerializeField]
  public Gradient CubeColor { get; private set; }

  [field: SerializeField]
  public Vector3 CubeRotation { get; private set; }

  [field: SerializeField, Min(0f)]
  public float RotationTime { get; private set; }

  Material _material;

  void Start() {
    _material = GetComponent<MeshRenderer>().material;
    _material
        .DOColor(CubeColor.Evaluate(1f), Shader.PropertyToID("_Color"), 2f)
        .SetLoops(-1, LoopType.Yoyo)
        .SetLink(gameObject);

    transform
        .DOLocalRotate(CubeRotation, RotationTime)
        .SetRelative(true)
        .SetEase(Ease.Linear)
        .SetLoops(-1, LoopType.Incremental)
        .SetLink(gameObject);
  }
}
