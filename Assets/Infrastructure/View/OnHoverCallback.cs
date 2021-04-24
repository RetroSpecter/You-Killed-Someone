using UnityEngine;
using UnityEngine.EventSystems;
using System;
using DG.Tweening;
using UnityEngine.Events;

public class OnHoverCallback : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public UnityEvent OnEnter;
    public UnityEvent OnExit;
    private float _scale;

    void Start() {
        _scale = transform.localScale.x;
    }

    void OnEnable() {
        OnExit?.Invoke();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.DOScale(_scale * 1.3f, 0.25f);
        OnEnter?.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.DOScale(_scale * 0.7f, 0.25f);
        OnExit?.Invoke();
    }
}