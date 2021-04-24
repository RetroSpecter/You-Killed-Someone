using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {


    public ViewController vc;
    private int recentlySelectedOption;

    // Start is called before the first frame update
    void Start() {

        // Initiate view?

        StartCoroutine(PlayGame());
    }

    public IEnumerator PlayGame() {
        yield return StartCoroutine(Murder());

    }

    public IEnumerator Murder() {
        yield return StartCoroutine(vc.DisplayStoryText(StoryText.YOU_KILLED_SOMEONE));

        // Give Choice: Who
        DialogueChoice whoYouKilled = new DialogueChoice(DialogueChoice.WHO_YOU_KILLED, "Who did you kill?", GameState.Instance.GetAliveCharacters());
        yield return StartCoroutine(vc.DisplayPrompt(whoYouKilled, SelectOption));

        // Give Choice: where
        DialogueChoice whereYouKilled = new DialogueChoice(DialogueChoice.MURDER_LOCATION, "Where did you kill them?",
            new List<DialogueChoiceOption> {
                new DialogueChoiceOption("", "On a box"),
                new DialogueChoiceOption("", "with a fox"),
                new DialogueChoiceOption("", "On a lake"),
                new DialogueChoiceOption("", "With Josh & Drake")
            });
        yield return StartCoroutine(vc.DisplayPrompt(whereYouKilled, SelectOption));

        // Give Choice: how
        // Give Choice: did you discover it?

        // Update game state
        yield return null;
    }

    public IEnumerator Investigation() {
        // Someone announces a murder occurred (is it you? if not, who?)

        // Everyone gathers
        // Character gets an entrance line
        // character reacts to death


        // we tell player to investigate
        // Who do want to talk to?

        int totalInvestigations = 3;
        for (int i = 0; i < totalInvestigations; i++) {

            
            yield return null;
        }


    }



    public void SelectOption(int selectedOption) {
        this.recentlySelectedOption = selectedOption;
    }
}
