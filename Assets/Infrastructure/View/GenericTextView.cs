using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using CharTween;
using DG.Tweening;

public class GenericTextView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMeshPro;
    [SerializeField] private Button button;
    [SerializeField] private AudioSource source;


    public void UpdatePrompt(string text, Action<int> callback) {
        CharTweener _tweener = textMeshPro.GetCharTweener();

        textMeshPro.text = text;

        var sequence = DOTween.Sequence();

        for (var i = 0; i <= text.Length; ++i) {
            var timeOffset = Mathf.Lerp(0, 1, (i) / (float)(text.Length + 1));
            var charSequence = DOTween.Sequence();
            charSequence
                .AppendCallback(() => source.Play())
                .Append(_tweener.DOFade(i, 0, 0.05f).From())
                .Join(_tweener.DOScale(i, 2f, 0.1f).From());

            sequence.Insert(timeOffset, charSequence);
        }

        button.onClick.AddListener(() => {
            callback?.Invoke(0);
            button.onClick.RemoveAllListeners();
        });
    }
}
