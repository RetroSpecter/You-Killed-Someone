using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class GenericTextView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMeshPro;
    [SerializeField] private Button button;

    public void UpdatePrompt(string text, Action<int> callback) {
        textMeshPro.text = text;

        button.onClick.AddListener(() => {
            callback?.Invoke(0);
            button.onClick.RemoveAllListeners();
        });
    }
}
