using TMPro;
using UnityEngine;

public class InfoView : MonoBehaviour
{
    public TextMeshProUGUI infoText;
    public CanvasGroup group;

    public const string infoFormat =  "job: {0} \n"
                                    + "tool: {1} \n"
                                    + "place: {2} \n"
                                    + "suspicion: {3} \n";

    public void SetInfo(Character character) {

        string suspcionLevel = "ERROR";

        if (character.sus >= 150)
        {
            suspcionLevel = "pretty sus";
        } else if (character.sus >= 100) {
            suspcionLevel = "VERY HIGH";
        } else if (character.sus >= 75) {
            suspcionLevel = "HIGH";
        } else if (character.sus >= 50) {
            suspcionLevel = "MEDIUM";
        } else if (character.sus >= 25) {
            suspcionLevel = "LOW";
        } else if (character.sus >= 0) {
            suspcionLevel = "VERY LOW";
        } else {
            suspcionLevel = "would give you their child";
        }

        if (character.profileKnown) {
            Profile characterProfile = character.profile;
            infoText.text = string.Format(infoFormat, characterProfile.occupation, characterProfile.preferredTool, characterProfile.preferredLocation, suspcionLevel);
        } else {
            infoText.text = string.Format(infoFormat, "?", "?", "?", suspcionLevel);
        }

        group.gameObject.SetActive(true);
        group.alpha = 0;
    }

    public void HideText() {
        group.gameObject.SetActive(false);
    }
}
