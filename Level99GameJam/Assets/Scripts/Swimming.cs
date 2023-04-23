using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Crest;
using System;

public class Swimming : MonoBehaviour
{

    [SerializeField] AudioSource underWaterSound;
    [SerializeField] AudioSource aboveWaterSound;
    public float swimSpeed = 1f;
    public Transform target;

    bool isBelowWater;

    Rigidbody rigidBody;

    void Start()
    {
        GameObject ocean = GameObject.Find("Ocean");
        OceanRenderer oceanRenderer = ocean.GetComponent<OceanRenderer>();
        rigidBody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
         if (isBelowWater)
        {
            SwimMovement();
        }
    }


    public void isUnderwater()
    {
        Debug.Log("I'm underwater");
        underWaterSound.Play();
        aboveWaterSound.Pause();
        SwimMovement();
        isBelowWater = true;

    }

    private void SwimMovement()
    {
        rigidBody.useGravity = false;
        if(Input.GetAxisRaw("Vertical") > 0)
        {
            transform.position += target.forward * swimSpeed * Time.deltaTime;
        }
        if(Input.GetAxisRaw("Vertical") < 0)
        {
            transform.position -= target.forward * swimSpeed * Time.deltaTime;
        }
        ResetVelocity();
    }

    public void isNotUnderwater()
    {
        Debug.Log("I'm NOT underwater");
        aboveWaterSound.Play();
        underWaterSound.Pause();
        StopSwimMovement();
        isBelowWater = false;
        ResetVelocity();
    }

    private void StopSwimMovement()
    {
        rigidBody.useGravity = true;
    }

    public void ResetVelocity()
    {
        rigidBody.velocity = Vector3.zero;
        rigidBody.angularVelocity = Vector3.zero;
    }
}
