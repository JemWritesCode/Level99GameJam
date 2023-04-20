using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DoubleSidedCollider : MonoBehaviour
{
    void Start()
    {
        // Jem: Leaving this here for testing but currently this makes really weird nauseating effect on the backface of objects
        MeshFilter meshFilter = GetComponent<MeshFilter>();
            meshFilter.mesh.SetIndices(meshFilter.mesh.GetIndices(0).Concat(meshFilter.mesh.GetIndices(0).Reverse()).ToArray(), MeshTopology.Triangles, 0);
        
    }
}
