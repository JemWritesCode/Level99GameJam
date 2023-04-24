using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishManager : MonoBehaviour
{
    [SerializeField] List<GameObject> fishPrefabs; 

    [SerializeField] int fishPoolSize = 30;
    GameObject[] fishPool;

    void Start()
    {
        fishPool = new GameObject[fishPoolSize];

        for (int i = 0; i < fishPoolSize; i++)
        {
            Vector3 rndPosWithin;
            rndPosWithin = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            rndPosWithin = transform.TransformPoint(rndPosWithin * .5f);
            GameObject newFish = Instantiate(RandomFishPrefab(), rndPosWithin, transform.rotation);
            fishPool[i] = newFish;
            Debug.Log("i: " + i);
        }
    }

    private GameObject RandomFishPrefab()
    {
        return fishPrefabs[Random.Range(0, fishPrefabs.Count)];
    }
}



