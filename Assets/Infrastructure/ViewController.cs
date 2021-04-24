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
            //StartCoroutine(DisplayStoryText());
        }
    }

    public IEnumerator DisplayStoryText(StoryText story) {
        storyText.gameObject.SetActive(true);

        for (int i = 0; i < story.text.Length; i++)
        {
            storyText.UpdatePrompt(story.text[i], SetChoice);
            yield return new WaitUntil(() => choice != -1);
        }

        storyText.gameObject.SetActive(false);
        ResetChoice();
    }

    public IEnumerator DisplayPrompt(DialogueChoice DialogueChoiceOption)
    {
        genericPrompt.gameObject.SetActive(true);
        genericPrompt.UpdatePrompt("Question", new List<string> { "1", "2", "3", "4" }, SetChoice);
        yield return new WaitUntil(() => choice != -1);

        storyText.gameObject.SetActive(false);
        ResetChoice();
    }

    public IEnumerator DisplayGenericPrompt(DialogueChoice DialogueChoiceOption) {
        genericPrompt.gameObject.SetActive(true);
        genericPrompt.UpdatePrompt("Question", new List<string> { "1", "2", "3", "4" }, SetChoice);
        yield return new WaitUntil(() => choice != -1);

        storyText.gameObject.SetActive(false);
        ResetChoice();
    }

    public IEnumerator DisplayYesNoPrompt(DialogueChoice DialogueChoiceOption)
    {
        yesNoPrompt.gameObject.SetActive(true);
        yesNoPrompt.UpdatePrompt("Question", new List<string> { "Yes", "No"}, SetChoice);
        yield return new WaitUntil(() => choice != -1);

        storyText.gameObject.SetActive(false);
        ResetChoice();
    }

    public IEnumerator DisplayCharacterSelectPrompt(DialogueChoice DialogueChoiceOption)
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
