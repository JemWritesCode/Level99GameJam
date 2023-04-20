using DG.Tweening;

using UnityEngine;


public class CubeController : MonoBehaviour {
  [field: SerializeField, Min(0)]
  public int CubeQuantity { get; private set; }

  void Start() {
    for (int i = 0; i < CubeQuantity; i++) {
      CreateCube();
    }
  }

  void CreateCube() {
    GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
    cube.transform.SetParent(transform, worldPositionStays: false);

    cube.transform.localScale = Vector3.one * Random.Range(0.25f, 0.5f);
    cube.transform.position = Random.insideUnitSphere * 3f;

    Material material = cube.GetComponent<MeshRenderer>().material;
    material.color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));

    cube.SetActive(true);

    material
        .DOColor(new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f)), 2f)
        .SetLoops(-1, LoopType.Yoyo)
        .SetLink(cube);

    cube.transform
        .DOLocalRotate(Random.rotation.eulerAngles, Random.Range(3f, 6f))
        .SetRelative(true)
        .SetEase(Ease.Linear)
        .SetLoops(-1, LoopType.Incremental)
        .SetLink(cube);

    cube.transform
        .DOLocalMove(Random.onUnitSphere * 2f, Random.Range(5f, 10f))
        .SetEase(Ease.Linear)
        .SetRelative(true)
        .SetLoops(-1, LoopType.Yoyo)
        .SetLink(cube);
  }
}
