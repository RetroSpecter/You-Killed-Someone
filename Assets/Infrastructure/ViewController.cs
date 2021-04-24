using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewController : MonoBehaviour
{
    public GenericTextView storyText;
    public ChoicePromptView genericView, yesNoView, characterView, askCharacterView;

    int choice = -1;

    private void Awake()
    {
        ResetChoice();
        storyText.gameObject.SetActive(false);
        genericView.gameObject.SetActive(false);
        yesNoView.gameObject.SetActive(false);
        characterView.gameObject.SetActive(false);
        askCharacterView.gameObject.SetActive(false);
    }

    public IEnumerator DisplayStoryText(StoryText story) {
        storyText.gameObject.SetActive(true); 

        //for (int i = 0; i < story.text.Length; i++) {
            storyText.AnimatePrompt(story, SetChoice);
            yield return new WaitUntil(() => choice != -1);
        //}

        storyText.gameObject.SetActive(false);
        ResetChoice();
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
        genericView.gameObject.SetActive(true);
        genericView.UpdatePrompt(DialogueChoiceOption.prompt, DialogueChoiceOption.options, SetChoice);
        yield return new WaitUntil(() => choice != -1);

        genericView.gameObject.SetActive(false);
        callback?.Invoke(choice);
        ResetChoice();
    }

    public IEnumerator DisplayYesNoPrompt(DialogueChoice DialogueChoiceOption, Action<int> callback)
    {
        yesNoView.gameObject.SetActive(true);
        yesNoView.UpdatePrompt(DialogueChoiceOption.prompt, DialogueChoiceOption.options, SetChoice);
        yield return new WaitUntil(() => choice != -1);

        yesNoView.gameObject.SetActive(false);
        callback?.Invoke(choice);
        ResetChoice();
    }

    public IEnumerator DisplayCharacterSelectPrompt(DialogueChoice DialogueChoiceOption, Action<int> callback)
    {
        characterView.gameObject.SetActive(true);
        characterView.UpdatePrompt(DialogueChoiceOption.prompt, DialogueChoiceOption.options, SetChoice);
        yield return new WaitUntil(() => choice != -1);

        characterView.gameObject.SetActive(false);
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
