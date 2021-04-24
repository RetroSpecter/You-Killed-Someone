using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TextDrift : MonoBehaviour
{

    void Start()
    {
        transform.DOShakePosition(100, 5, 1, 1000, false, false).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
    }

}
