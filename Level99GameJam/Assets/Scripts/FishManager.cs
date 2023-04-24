using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishManager : MonoBehaviour
{
    [SerializeField] List<GameObject> fishPrefabs; // The GameObject to spawn.
    //public float minDistance = 1000f; // The minimum distance between fish.
    //public float maxDistance = 1000f; // The maximum distance between fish.
    //public float topOfOceanApprox = 180f;
    //public float bottomOfOceanApprox = 100f;

    [SerializeField] int fishPoolSize = 30;
    GameObject[] fishPool;

    public float RateOfSpawn = 1;

    private float nextSpawn = 0;

    // Update is called once per frame
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

    //private void PopulateFishPool()
    //{
    //    fishPool = new GameObject[fishPoolSize];

    //    for (int i = 0; i < fishPool.Length; i++)
    //    {
    //        Vector3 randomPosition = transform.position + Random.insideUnitSphere * 100f;
    //        GameObject newFish = Instantiate(RandomFishPrefab(), randomPosition, transform.rotation);
    //        fishPool[i] = newFish;
    //    }
    //}

    private GameObject RandomFishPrefab()
    {
        return fishPrefabs[Random.Range(0, fishPrefabs.Count)];
    }
}



