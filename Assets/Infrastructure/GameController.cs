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
        yield return null;

        yield return StartCoroutine(vc.DisplayStoryText(StoryText.X_KILLED_SOMEONE));

        // Give Choice: Who
        DialogueChoice whoYouKilled = new DialogueChoice(DialogueChoice.WHO_YOU_KILLED, "Who did you kill?", GameState.Instance.GetAliveCharacters());
        yield return StartCoroutine(vc.DisplayPrompt(whoYouKilled, SelectOption));
        Debug.Log("Selected Character: " + whoYouKilled.options[recentlySelectedOption]);

        // Give Choice: where
        DialogueChoice whereYouKilled = new DialogueChoice(DialogueChoice.MURDER_LOCATION, "Where did you kill them?", GameState.Instance.GetMurderLocations());
        yield return StartCoroutine(vc.DisplayPrompt(whereYouKilled, SelectOption));
        Debug.Log("Selected location: " + whereYouKilled.options[recentlySelectedOption]);

        // Give Choice: how
        DialogueChoice howYouKilled = new DialogueChoice(DialogueChoice.MURDER_WEAPON, "How did you kill them?", GameState.Instance.GetMurderWeapons());
        yield return StartCoroutine(vc.DisplayPrompt(howYouKilled, SelectOption));
        Debug.Log("Selected method of murder: " + howYouKilled.options[recentlySelectedOption]);

        // Give Choice: did you discover it?
        DialogueChoice didYouDiscoverBody = new DialogueChoice(DialogueChoice.DISCOVER_BODY, "Did you \"discover\" the body?");
        yield return StartCoroutine(vc.DisplayPrompt(didYouDiscoverBody, SelectOption));
        Debug.Log("Did you discover the body: " + didYouDiscoverBody.options[recentlySelectedOption]);

        // Update game state
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
