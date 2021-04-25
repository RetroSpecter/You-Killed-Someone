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
    [SerializeField] public Button button; // should be private but im lazy
    [SerializeField] private CharacterAttributesLookup attributesLookup;
    [SerializeField] private SoundEffectProfile defaultTextTic;
    [SerializeField] private SoundEffectProfile buttonSFX;

    public void AnimatePrompt(StoryText text, Action<int> callback, string characterIndex = "")
    {
        textMeshPro.gameObject.SetActive(true);
        var sequence = text.settings.byWord ? AnimatePerWord(text) : AnimatePerCharacter(text, characterIndex);

        ActivateButton(text.settings, callback);
    }

    private Sequence AnimatePerWord(StoryText text) {
        textMeshPro.text = text.ProcessText();



        List<Color> ColorMapping = text.MapColor(attributesLookup);
        CharTweener _tweener = textMeshPro.GetCharTweener();


        SoundEffectProfile textBlip = defaultTextTic;

        textMeshPro.color = Color.clear;
        var sequence = DOTween.Sequence();
        sequence.AppendInterval(text.settings.speed);

        int i = 0;

        while (i < textMeshPro.text.Length) {
            int j = i;
            sequence.AppendCallback(() => textBlip?.Play());
            while (j < textMeshPro.text.Length-1 && textMeshPro.text[j] != ' ') {
                sequence.Join(_tweener.DOColor(j, ColorMapping[j], 0.01f))
                .Join(_tweener.DOScale(j, 0, 0.01f).From());
                j++;
            }

            sequence.AppendInterval(text.settings.speed);
            i = j;
            i++;
        }
        return sequence;
    }

    private Sequence AnimatePerCharacter(StoryText text,  string characterIndex = "") {
        textMeshPro.color = Color.clear;
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
                .Join(_tweener.DOColor(i, ColorMapping[i], 0.01f))
                .Join(_tweener.DOScale(i, 0, 0.01f).From());
            sequence.Insert(timeOffset * text.settings.speed, charSequence);
        }
        
        
        return sequence;
    }

    public void UpdatePrompt(StoryText text, Action<int> callback, string characterIndex = "")
    {
        textMeshPro.gameObject.SetActive(true);
        button.enabled = true;
        textMeshPro.text = text.ProcessText();
        SetTextSettings(text);

        if (GameState.Instance.characters.ContainsKey(characterIndex)) {
            Character target = GameState.Instance.characters[characterIndex];
            GetComponent<InfoView>().SetInfo(target);
        }

        List<Color> ColorMapping = text.MapColor(attributesLookup);
        CharTweener _tweener = textMeshPro.GetCharTweener();

        var sequence = DOTween.Sequence();

        for (var i = 0; i < textMeshPro.text.Length; i++) {
            _tweener.DOColor(i, ColorMapping[i], 0.1f);
        }

        ActivateButton(text.settings, callback);
    }

    public void HidePrompt()
    {
        textMeshPro.gameObject.SetActive(false);
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
    }

    public void ActivateButton(TextSettings settings, Action<int> callback) {
        if (button != null)
        {
            button.enabled = true;
            button.gameObject.SetActive(true);
            button.onClick.AddListener(() =>
            {
                callback?.Invoke(0);
                buttonSFX?.Play();
                button.onClick = new Button.ButtonClickedEvent();
            });

            button.interactable = !settings.strikethrough;
            button.GetComponent<OnHoverCallback>().enabled = !settings.strikethrough;
        }
    }

    public void CancelButton() {
        if (button != null)
        {
            button.onClick = new Button.ButtonClickedEvent();
            button.enabled = false;
            button.GetComponent<OnHoverCallback>().OnPointerExit(null);
            button.GetComponent<OnHoverCallback>().enabled = false;
        }
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