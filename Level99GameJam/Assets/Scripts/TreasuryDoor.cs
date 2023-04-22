using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TreasuryDoor : MonoBehaviour
{
    Vector3 doorOriginalPosition;
    Vector3 doorMoveToCoords;
    Vector3 doorMoveBy;

    Transform parentObjectTransform;
    Vector3 shakeStrength;
    Sequence shakeAndGoDown;
    
    [SerializeField] AudioSource doorMoveSound;

    [SerializeField] float Amplitude = 1f; // this becomes how far it can move I think
    [SerializeField] float Duration = .1f;
  

    public void moveTreasuryDoor()
    {
        Debug.Log("You interacted with the sword on the Treasury Door!");

        parentObjectTransform = gameObject.transform.parent.gameObject.transform;

        doorOriginalPosition = parentObjectTransform.position;
        doorMoveToCoords.Set(239.110001f, 176.809998f, 469.691589f);
        doorMoveBy = doorMoveToCoords - doorOriginalPosition;


        //shakeStrength.Set(0f, 0f, .25f);

        var randomTremor = GetRandomTremor();

        parentObjectTransform.DOBlendableMoveBy(randomTremor, Duration)
            .OnUpdate(() => randomTremor = GetRandomTremor())
            .SetLoops(4, LoopType.Yoyo)
            .SetEase(Ease.InFlash);
        //parentObjectTransform.DOBlendableMoveBy(doorMoveBy, 3f).SetLoops(4, LoopType.Incremental);



        //var randomTremor = GetRandomTremor();
        //shakeAndGoDown = DOTween.Sequence()
        //    .Insert(parentObjectTransform.DOBlendableMoveBy(randomTremor, Duration))
        //    .Insert(0f, parentObjectTransform.DOBlendableMoveBy(doorMoveBy, 2f))
        //    //.OnUpdate(() => randomTremor = GetRandomTremor())
        //    .SetLoops(4, LoopType.Incremental)
        //    .SetEase(Ease.InFlash);

        //shake, then go down, shake, then go down



        //shakeAndGoDown = DOTween.Sequence()
        //    .Insert(0f, parentObjectTransform.DOMove(doorMoveToCoords, 2f, false))
        //    .Insert(0f,parentObjectTransform.DOShakePosition(2f, shakeStrength, 2, 5, false, true, ShakeRandomnessMode.Harmonic));



        // Do blendable move by
        // do punch
        //maybe can use a sequence instead of blendable
    }

    private Vector3 GetRandomTremor()
    {
        return new Vector3(Random.Range(0, Amplitude), Random.Range(0, Amplitude), Random.Range(0, Amplitude));
    }

}
