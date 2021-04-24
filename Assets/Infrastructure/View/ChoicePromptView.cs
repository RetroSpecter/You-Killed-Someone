using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class ChoicePromptView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMeshPro;
    [SerializeField] private Button[] buttons;

    public void UpdatePrompt(string text, List<DialogueChoiceOption> choices, Action<int> callback)
    {
        if (choices.Count > buttons.Length) {
            Debug.LogError("too many choices for the container");
        }

        textMeshPro.text = text;

        for (int i = 0; i < buttons.Length; i++) {
            if (i < choices.Count)
            {
                Button b = buttons[i];
                b.gameObject.SetActive(true);
                b.GetComponentInChildren<TextMeshProUGUI>().text = choices[i].optionText;
                int j = i;
                b.onClick.AddListener(() =>
                {
                    callback?.Invoke(j);
                    b.onClick.RemoveAllListeners();
                });
            } else {
                buttons[i].gameObject.SetActive(false);
            }

        }
    }  
}
