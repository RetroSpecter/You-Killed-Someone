using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class ChoicePromptView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMeshPro;
    [SerializeField] private Button[] button;

    public void UpdatePrompt(string text, List<string> choices, Action<int> callback)
    {
        textMeshPro.text = text;

        for (int i = 0; i < button.Length; i++) {
            button[i].GetComponentInChildren<TextMeshProUGUI>().text = choices[i];
            button[i].onClick.AddListener(() => {
                callback?.Invoke(i);
                button[i].onClick.RemoveAllListeners();
            });
        }
    }

}
