using UnityEngine;
using UnityEngine.EventSystems;
using System;
using DG.Tweening;

public class OnHoverCallback : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Action OnEnter;
    public Action OnExit;
    private float _scale;

    void Start() {
        _scale = transform.localScale.x;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.DOScale(_scale * 1.3f, 0.25f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.DOScale(_scale * 0.7f, 0.25f);
    }
}