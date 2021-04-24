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
    //[SerializeField] private AudioSource source;


    public void UpdatePrompt(StoryText text, Action<int> callback) {
        textMeshPro.text = text.processText();
        CharTweener _tweener = textMeshPro.GetCharTweener();

        print(_tweener.CharacterCount + " vs "  + textMeshPro.GetParsedText().Length);

        //TODO: Dropping this for now because it doesn't respect color correctly. i prioritize color over this effect any day
        
        var sequence = DOTween.Sequence();

        /*
        for (var i = 0; i < textMeshPro.GetParsedText(); i++) {
           if (payload) {
                //var timeOffset = Mathf.Lerp(0, 1, (val) / (float)(text.Length + 1));
                
                Sequence charSequence = DOTween.Sequence()
                    .Append(_tweener.DOFade(i, 0, 0.05f).From())
                    .Join(_tweener.DOScale(i, 2f, 0.1f).From());
                
                _tweener.DOFade(i, 0.5f, 0.1f);
            }
        }*/
        

        button.onClick.AddListener(() => {
            callback?.Invoke(0);
            button.onClick.RemoveAllListeners();
        });
    }
}
