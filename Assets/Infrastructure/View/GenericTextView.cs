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
    [SerializeField] private CharacterAttributesLookup attributesLookup;
    [SerializeField] private SoundEffectProfile defaultTextTic;

    public void AnimatePrompt(StoryText text, Action<int> callback, string characterIndex = "")
    {
        var sequence = AnimatePerCharacter(text, characterIndex);

        if (button != null) {
            button.gameObject.SetActive(false);
            sequence.AppendCallback(() => { 
                button.gameObject.SetActive(true);
                button.onClick.AddListener(() =>
                {
                    callback?.Invoke(0);
                    button.onClick.RemoveAllListeners();
                });
            });
        }
    }

    private Sequence AnimatePerWord(StoryText text) {
        textMeshPro.text = text.ProcessText();

        List<Color> ColorMapping = text.MapColor(attributesLookup);
        CharTweener _tweener = textMeshPro.GetCharTweener();
        TMP_WordInfo[] words = textMeshPro.textInfo.wordInfo;

        textMeshPro.color = Color.clear;
        var sequence = DOTween.Sequence();

        for (var i = 0; i < words.Length-1; i++)
        {
            var timeOffset = Mathf.Lerp(0, 1, i / (float)(words.Length + 1)) * 3;

            for (int j = words[i].firstCharacterIndex; j <= words[i].lastCharacterIndex; j++) {
                var charSequence = DOTween.Sequence()
                    .Append(_tweener.DOColor(j, ColorMapping[j], 0.01f))
                    .Join(_tweener.DOScale(j, 0, 0.01f).From());
                    //.Append(_tweener.DOLocalMoveY(j, 0, 0.1f).SetEase(Ease.OutBounce));
                sequence.Insert(1 + timeOffset, charSequence);
            }
        }

        return sequence;
    }

    private Sequence AnimatePerCharacter(StoryText text, string characterIndex = "") {
        textMeshPro.color = Color.clear;
        textMeshPro.text = text.ProcessText();
        List<Color> ColorMapping = text.MapColor(attributesLookup);
        CharTweener _tweener = textMeshPro.GetCharTweener();

        SoundEffectProfile textBlip = defaultTextTic;
        if (characterIndex != "")
        {
            textBlip = attributesLookup.GetCharacterAttributes(characterIndex).soundPing;
        }

        var sequence = DOTween.Sequence();

        for (var i = 0; i < textMeshPro.text.Length; i++)
        {
            var timeOffset = Mathf.Lerp(0, 1, i / (float)(textMeshPro.text.Length + 1));
            var charSequence = DOTween.Sequence();
            charSequence.AppendCallback(() => textBlip?.Play())
                //.Append(_tweener.DOLocalMoveY(i, 0.5f, 0.5f).SetEase(Ease.InOutCubic))
                .Join(_tweener.DOColor(i, ColorMapping[i], 0.1f))
                .Join(_tweener.DOScale(i, 0, 0.2f).From().SetEase(Ease.OutBack, 5));
                //.Append(_tweener.DOLocalMoveY(i, 0, 0.1f).SetEase(Ease.OutBounce));
            sequence.Insert(timeOffset, charSequence);
        }

        return sequence;
    }

    public void UpdatePrompt(StoryText text, Action<int> callback, string characterIndex = "")
    {
        textMeshPro.text = text.ProcessText();
        List<Color> ColorMapping = text.MapColor(attributesLookup);
        CharTweener _tweener = textMeshPro.GetCharTweener();

        var sequence = DOTween.Sequence();

        for (var i = 0; i < textMeshPro.text.Length; i++) {
            _tweener.DOColor(i, ColorMapping[i], 0.1f);
        }

        if (button != null)
        {
            button.onClick.AddListener(() =>
            {
                callback?.Invoke(0);
                button.onClick.RemoveAllListeners();
            });
        }
    }

    public void CancelButton() {
        button.gameObject.SetActive(false);
    }
}
