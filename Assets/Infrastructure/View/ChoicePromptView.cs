using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using DG.Tweening;

public class ChoicePromptView : MonoBehaviour
{
    [SerializeField] private GenericTextView gtv;
    [SerializeField] private GenericTextView[] buttons;

    public void UpdatePrompt(StoryText text, List<DialogueChoiceOption> choices, Action<int> callback)
    {
        if (choices.Count > buttons.Length) {
            Debug.LogError("too many choices for the container");
        }

        gtv.UpdatePrompt(text, null);

        for (int i = 0; i < buttons.Length; i++) {
            if (i < choices.Count)
            {
                GenericTextView b = buttons[i];
                b.gameObject.SetActive(true);

                int j = i;
                b.UpdatePrompt(choices[i].optionText, _ => buttonSelected(j, callback), choices[i].optionID);
            } else {
                buttons[i].gameObject.SetActive(false);
            }
        }
    }

    private void buttonSelected(int index, Action<int> callback) {
        for (int i = 0; i < buttons.Length; i++)
        {
            if(i != index)
                buttons[i].CancelButton();
        }

        DOTween.Sequence()
            .AppendInterval(UnityEngine.Random.Range(1, 2))
            .AppendCallback(() => callback(index));
    }
}
