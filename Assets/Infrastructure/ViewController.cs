using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewController : MonoBehaviour
{
    public GenericTextView storyText;
    public ChoicePromptView genericPrompt, yesNoPrompt, characterPrompt;

    int choice = -1;

    private void Start()
    {
        ResetChoice();
        storyText.gameObject.SetActive(false);
        genericPrompt.gameObject.SetActive(false);
        yesNoPrompt.gameObject.SetActive(false);
        characterPrompt.gameObject.SetActive(false);
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            StartCoroutine(DisplayStoryText());
        }
    }

    IEnumerator DisplayStoryText() {
        storyText.gameObject.SetActive(true);
        storyText.UpdatePrompt("You Killed Someone", SetChoice);
        yield return new WaitUntil(() => choice != -1);

        storyText.gameObject.SetActive(false);
        ResetChoice();
    }

    IEnumerator DisplayGenericPrompt() {
        genericPrompt.gameObject.SetActive(true);
        genericPrompt.UpdatePrompt("Question", new List<string> { "1", "2", "3", "4" }, SetChoice);
        yield return new WaitUntil(() => choice != -1);

        storyText.gameObject.SetActive(false);
        ResetChoice();
    }

    IEnumerator DisplayYesNoPrompt()
    {
        yesNoPrompt.gameObject.SetActive(true);
        yesNoPrompt.UpdatePrompt("Question", new List<string> { "Yes", "No"}, SetChoice);
        yield return new WaitUntil(() => choice != -1);

        storyText.gameObject.SetActive(false);
        ResetChoice();
    }

    IEnumerator DisplayCharacterSelectPrompt()
    {
        characterPrompt.gameObject.SetActive(true);
        characterPrompt.UpdatePrompt("Question", new List<string> { "1", "2", "3", "4" }, SetChoice);
        yield return new WaitUntil(() => choice != -1);

        storyText.gameObject.SetActive(false);
        ResetChoice();
    }

    public void SetChoice(int i) {
        choice = i;
    }

    public void ResetChoice() {
        choice = -1;
    }
}
