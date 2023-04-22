using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TreasuryDoor : MonoBehaviour
{
    Transform parentObjectTransform;
    Vector3 doorOriginalPosition;
    Vector3 doorMoveToCoords;
    Vector3 doorMoveBy;
    [SerializeField] float Amplitude = .5f; // this becomes how far it can move I think
    
    [SerializeField] AudioSource doorMoveSound;

    public void moveTreasuryDoor()
    {
        Debug.Log("You interacted with the sword on the Treasury Door!");

        shakeAndGoDown();
        doorMoveSound.Play();

        // Leaving this here in case I wanna try to spurce up the shake more
        //shake, then go down, shake, then go down
        //shakeAndGoDown = DOTween.Sequence()
        //    .Insert(0f, parentObjectTransform.DOMove(doorMoveToCoords, 2f, false))
        //    .Insert(0f,parentObjectTransform.DOShakePosition(2f, shakeStrength, 2, 5, false, true, ShakeRandomnessMode.Harmonic));
    }

    private void shakeAndGoDown()
    {
        parentObjectTransform = gameObject.transform.parent.gameObject.transform;
        doorOriginalPosition = parentObjectTransform.position;
        doorMoveToCoords.Set(239.110001f, 176.809998f, 469.691589f);
        doorMoveBy = doorMoveToCoords - doorOriginalPosition;

        var randomTremor = GetRandomTremor();

        parentObjectTransform.DOBlendableMoveBy(randomTremor, .5f)
            .SetLoops(6, LoopType.Yoyo);
        parentObjectTransform.DOBlendableMoveBy(doorMoveBy, 5f);
    }

    private Vector3 GetRandomTremor()
    {
        return new Vector3(Random.Range(.2f, Amplitude), 0, Random.Range(.2f, Amplitude));
    }

}
