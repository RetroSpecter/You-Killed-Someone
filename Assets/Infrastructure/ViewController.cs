using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewController : MonoBehaviour
{
    public GenericTextView storyText;
    public ChoicePromptView genericPrompt, yesNoPrompt, characterPrompt;

    int choice = -1;

    private void Awake()
    {
        ResetChoice();
        storyText.gameObject.SetActive(false);
        genericPrompt.gameObject.SetActive(false);
        yesNoPrompt.gameObject.SetActive(false);
        characterPrompt.gameObject.SetActive(false);
    }

    public IEnumerator DisplayStoryText(StoryText story) {
        storyText.gameObject.SetActive(true); 

        //for (int i = 0; i < story.text.Length; i++) {
            storyText.UpdatePrompt(story, SetChoice);
            yield return new WaitUntil(() => choice != -1);
        //}

        storyText.gameObject.SetActive(false);
        ResetChoice();
    }

    public IEnumerator DisplayCharacterText(CharacterText character) {
        // TODO: Bleg, hehe
        yield return null;
    }

    public IEnumerator DisplayPrompt(DialogueChoice DialogueChoiceOption, Action<int> callback)
    {
        if (DialogueChoiceOption.isYesNo())
            yield return StartCoroutine(DisplayYesNoPrompt(DialogueChoiceOption, callback));
        else if (DialogueChoiceOption.isFour())
            yield return StartCoroutine(DisplayFourSquarePrompt(DialogueChoiceOption, callback));
        else
            yield return StartCoroutine(DisplayCharacterSelectPrompt(DialogueChoiceOption, callback));
    }

    public IEnumerator DisplayFourSquarePrompt(DialogueChoice DialogueChoiceOption, Action<int> callback) {
        genericPrompt.gameObject.SetActive(true);
        genericPrompt.UpdatePrompt(DialogueChoiceOption.prompt, DialogueChoiceOption.options, SetChoice);
        yield return new WaitUntil(() => choice != -1);

        genericPrompt.gameObject.SetActive(false);
        callback?.Invoke(choice);
        ResetChoice();
    }

    public IEnumerator DisplayYesNoPrompt(DialogueChoice DialogueChoiceOption, Action<int> callback)
    {
        yesNoPrompt.gameObject.SetActive(true);
        yesNoPrompt.UpdatePrompt(DialogueChoiceOption.prompt, DialogueChoiceOption.options, SetChoice);
        yield return new WaitUntil(() => choice != -1);

        yesNoPrompt.gameObject.SetActive(false);
        callback?.Invoke(choice);
        ResetChoice();
    }

    public IEnumerator DisplayCharacterSelectPrompt(DialogueChoice DialogueChoiceOption, Action<int> callback)
    {
        characterPrompt.gameObject.SetActive(true);
        characterPrompt.UpdatePrompt(DialogueChoiceOption.prompt, DialogueChoiceOption.options, SetChoice);
        yield return new WaitUntil(() => choice != -1);

        characterPrompt.gameObject.SetActive(false);
        callback?.Invoke(choice);
        ResetChoice();
    }

    public void SetChoice(int i) {
        choice = i;
    }

    public void ResetChoice() {
        choice = -1;
    }
}
