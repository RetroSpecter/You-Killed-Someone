using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InfoView : MonoBehaviour
{
    public TextMeshProUGUI infoText;
    public CanvasGroup group;

    public const string infoFormat = "job: {0} \n"
                                    + "tool: {1} \n"
                                    +"place: {2}";

    public void SetInfo(Profile characterProfile) {
        if (characterProfile != null)
        {
            infoText.text = string.Format(infoFormat, characterProfile.occupation, characterProfile.preferredTool, characterProfile.preferredLocation);
        }
        else {
            infoText.text = string.Format(infoFormat, "?", "?", "?");

        }
        group.gameObject.SetActive(true);
        group.alpha = 0;
    }

    public void HideText() {
        group.gameObject.SetActive(false);
    }
}
