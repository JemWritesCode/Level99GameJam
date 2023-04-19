using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Crest;

public class Swimming : MonoBehaviour
{

    [SerializeField] AudioSource underWaterSound;
    // Start is called before the first frame update
    void Start()
    {
        GameObject ocean = GameObject.Find("Ocean");
        OceanRenderer oceanRenderer = ocean.GetComponent<OceanRenderer>();

        
    }


    public void isUnderwater()
    {
        Debug.Log("I'm underwater");
        underWaterSound.Play();
    }

    public void isNotUnderwater()
    {
        Debug.Log("I'm NOT underwater");
        underWaterSound.Stop();
    }

}
