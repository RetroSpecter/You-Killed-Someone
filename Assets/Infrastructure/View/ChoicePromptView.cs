using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using DG.Tweening;
using System.Linq;

public class ChoicePromptView : MonoBehaviour
{
    [SerializeField] private GenericTextView gtv;
    [SerializeField] private GenericTextView[] buttons; 
    private Vector2[] buttonInitialPos;
    private Vector2[] buttonInitScale;

    public void UpdatePrompt(StoryText text, List<DialogueChoiceOption> choices, Action<int> callback)
    {
        if (choices.Count > buttons.Length) {
            Debug.LogError("too many choices for the container");
        }


        gtv.AnimatePrompt(new StoryText(text.textID, text.text, text.characters, text.scenery, text.weapons, new TextSettings(1f, false, 0.5f, false)), null);

        for (int i = 0; i < buttons.Length; i++) {
            if (i < choices.Count)
            {
                GenericTextView b = buttons[i];
                b.gameObject.SetActive(true);

                int j = i;

                Vector2 dir = b.transform.position.normalized;

                if (buttonInitialPos == null) {
                    buttonInitialPos = buttons.Select(x => (Vector2)x.transform.position).ToArray();
                    buttonInitScale = buttons.Select(x => (Vector2)x.transform.localScale).ToArray();
                }

                b.transform.position = buttonInitialPos[i];
                b.transform.localScale = buttonInitScale[i];
                

                buttons[i].button.interactable = true;

                b.transform.DOMove(buttonInitialPos[i] + dir * 200, UnityEngine.Random.Range(1f, 2f)).SetEase(Ease.InOutQuad).From();
                b.UpdatePrompt(choices[i].optionText, _ => buttonSelected(j, callback), choices[i].optionID);
            } else {
                buttons[i].gameObject.SetActive(false);
            }
        }
    }

    private void buttonSelected(int index, Action<int> callback) {
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].CancelButton();
            if (i != index)
                buttons[i].gameObject.SetActive(false);
        }

        gtv.HidePrompt();

        float t = UnityEngine.Random.Range(2.5f, 1.5f);

        DOTween.Sequence()
            .Append(buttons[index].transform.DOMove(Vector2.zero, t).SetEase(Ease.OutCubic, 10))
            .Join(buttons[index].transform.DOScale(buttonInitScale[index].x * 2.5f, t + t * 0.5f).SetEase(Ease.OutCubic, 10))
            .AppendInterval(0.5f)
            .AppendCallback(() => { 
                callback(index);
                buttons[index].transform.position = buttonInitialPos[index];
                buttons[index].transform.localScale = buttonInitScale[index];
            });
    }
}
