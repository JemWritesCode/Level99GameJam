using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class OpenTreasureLid : MonoBehaviour
{
    [SerializeField] AudioSource lidMoveSound;

    public void moveChestLid()
    {
        Debug.Log("You interacted with the lid!");

        gameObject.transform.DOBlendableRotateBy(new Vector3(0, 0, 100), 3f, RotateMode.Fast);

        lidMoveSound.Play();
    }



}
