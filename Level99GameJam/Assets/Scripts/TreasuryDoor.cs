using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TreasuryDoor : MonoBehaviour
{
    Vector3 doorMoveToCoords;

    Vector3 shakeStrength;

    [SerializeField] AudioSource doorMoveSound;
    Transform parentObjectTransform;

    public void moveTreasuryDoor()
    {
        Debug.Log("You interacted with the sword on the Treasury Door!");
        parentObjectTransform = gameObject.transform.parent.gameObject.transform;

        doorMoveToCoords.Set(239.110001f, 176.809998f, 469.691589f);
        shakeStrength.Set(0f, 0f, .25f);

        parentObjectTransform.DOMove(doorMoveToCoords, 2f, false);
        parentObjectTransform.DOShakePosition(2f,shakeStrength, 2, 5, false, true, ShakeRandomnessMode.Harmonic);
        // Do blendable move by
        // do punch
        //maybe can use a sequence instead of blendable
    }

}
