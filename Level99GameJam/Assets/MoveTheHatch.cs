using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MoveTheHatch : MonoBehaviour
{
    Vector3 hatchMoveToCoords;
    [SerializeField] AudioSource hatchMoveSound;

    public void moveHatchToSide()
    {
        Debug.Log("You interacted with the hatch!");

        hatchMoveToCoords.Set(497.23819f, 124.458405f, 440.016296f);
        gameObject.transform.DOMove(hatchMoveToCoords, 2f, false);

        hatchMoveSound.Play();
    }
}
