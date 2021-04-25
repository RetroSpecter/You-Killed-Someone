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
        var sequence = text.settings.byWord ? AnimatePerWord(text) : AnimatePerCharacter(text, characterIndex);

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


        SoundEffectProfile textBlip = defaultTextTic;

        textMeshPro.color = Color.clear;
        var sequence = DOTween.Sequence();

        /*
int i = 0;

while (i < textMeshPro.text.Length) {
    int j = i;
    while (j < textMeshPro.text.Length && textMeshPro.text[i] != ' ') {
        var charSequence = DOTween.Sequence();
        charSequence.AppendCallback(() => {
            textBlip?.Play();
            textMeshPro.ForceMeshUpdate();
        })
            .Append(_tweener.DOColor(j, ColorMapping[j], 0.01f))
            .Join(_tweener.DOScale(j, 0, 0.01f).From());
        sequence.Append(charSequence);
        j++;
    }
    sequence.AppendInterval(1);
    i = j;
    i++;
}
*/

        textMeshPro.gameObject.SetActive(false); 
        textMeshPro.gameObject.SetActive(true);
        SetTextSettings(text);
        TMP_WordInfo[] words =  textMeshPro.textInfo.wordInfo;
        for (var i = 0; i < words.Length-1; i++)
        {
            var timeOffset = Mathf.Lerp(0, 1, i / (float)(words.Length + 1)) * 3;
            var charSequence = DOTween.Sequence();
            charSequence.AppendCallback(() =>
            {
                textMeshPro.gameObject.SetActive(false);
                textMeshPro.gameObject.SetActive(true);
            });

            for (int j = words[i].firstCharacterIndex; j <= words[i].lastCharacterIndex; j++) {
                 charSequence
                    .Append(_tweener.DOColor(j, ColorMapping[j], 0.01f))
                    .Join(_tweener.DOScale(j, 0, 0.01f).From());
                sequence.Insert(1 + timeOffset * text.settings.speed, charSequence);
            }
        }
        

        return sequence;
    }

    private Sequence AnimatePerCharacter(StoryText text,  string characterIndex = "") {
        //textMeshPro.color = Color.clear;


        textMeshPro.text = text.ProcessText();
        List<Color> ColorMapping = text.MapColor(attributesLookup);
        CharTweener _tweener = textMeshPro.GetCharTweener();
        SetTextSettings(text);

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
                .Join(_tweener.DOColor(i, ColorMapping[i], 0.1f))
                .Join(_tweener.DOScale(i, 0, 0.2f).From().SetEase(Ease.OutBack, 5));
            sequence.Insert(1 + timeOffset * text.settings.speed, charSequence);
        }
        
        
        return sequence;
    }

    public void UpdatePrompt(StoryText text, Action<int> callback, string characterIndex = "")
    {
        textMeshPro.text = text.ProcessText();
        SetTextSettings( text);

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

    Sequence shakeSequence;
    public void SetTextSettings(StoryText text) {
        textMeshPro.fontStyle = text.settings.strikethrough ? FontStyles.Strikethrough : FontStyles.Normal;
        CharTweener _tweener = textMeshPro.GetCharTweener();

        shakeSequence?.Kill();
        shakeSequence = DOTween.Sequence();

        for (var i = 0; i < textMeshPro.textInfo.characterCount; i++) {
            _tweener.DOShakePosition(i, 1, text.settings.shake, 50, 90, false, false)
                .SetLoops(-1, LoopType.Restart);
        }

        if (button != null)
        {
            button.interactable = !text.settings.strikethrough;
            button.GetComponent<OnHoverCallback>().enabled = !text.settings.strikethrough;
        }
    }

    public void CancelButton() {
        button.gameObject.SetActive(false);
    }
}


public class TextSettings {
    public static TextSettings DEFAULT = new TextSettings(1, false, 0.5f, false);

    public float speed;
    public bool strikethrough;
    public bool byWord;
    public float shake;
    public TextSettings(float speed, bool strikethrough, float shake, bool byWord)
    {
        this.speed = speed;
        this.strikethrough = strikethrough;
        this.shake = shake;
        this.byWord = byWord;
    }


}