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

        yield return StartCoroutine(vc.DisplayStoryText(StoryText.YOU_KILLED_SOMEONE));

        MurderProfile mp = new MurderProfile();

        // Give Choice: Who
        DialogueChoice whoYouKilled = new DialogueChoice(DialogueChoice.WHO_YOU_KILLED, GameState.Instance.GetAliveCharacters());
        yield return StartCoroutine(vc.DisplayPrompt(whoYouKilled, SelectOption));
        Debug.Log("Selected Character: " + whoYouKilled.options[recentlySelectedOption]);
        GameState.Instance.KillCharacter(whoYouKilled.options[recentlySelectedOption].optionID);
        mp.murderedCharacterID = whoYouKilled.options[recentlySelectedOption].optionID;

        // Give Choice: where
        DialogueChoice whereYouKilled = new DialogueChoice(DialogueChoice.MURDER_LOCATION, GameState.Instance.GetMurderLocations());
        yield return StartCoroutine(vc.DisplayPrompt(whereYouKilled, SelectOption));
        Debug.Log("Selected location: " + whereYouKilled.options[recentlySelectedOption]);
        mp.weaponProfileID = whereYouKilled.options[recentlySelectedOption].optionID;

        // Give Choice: how
        DialogueChoice howYouKilled = new DialogueChoice(DialogueChoice.MURDER_WEAPON, GameState.Instance.GetMurderWeapons());
        yield return StartCoroutine(vc.DisplayPrompt(howYouKilled, SelectOption));
        Debug.Log("Selected method of murder: " + howYouKilled.options[recentlySelectedOption]);
        mp.locationProfileID = howYouKilled.options[recentlySelectedOption].optionID;

        // Give Choice: did you discover it?
        DialogueChoice didYouDiscoverBody = new DialogueChoice(DialogueChoice.DISCOVER_BODY);
        yield return StartCoroutine(vc.DisplayPrompt(didYouDiscoverBody, SelectOption));
        Debug.Log("Did you discover the body: " + didYouDiscoverBody.options[recentlySelectedOption]);
        // If you did, you are the discoverer. Otherwise, a random NPC is the discoverer
        if (didYouDiscoverBody.options[recentlySelectedOption].optionID == DialogueChoiceOption.YES) {
            mp.bodyDiscovererID = Character.playerID;
        } else {
            var aliveCharacters = GameState.Instance.GetAliveCharacters();
            mp.bodyDiscovererID = aliveCharacters[Random.Range(0, aliveCharacters.Count)].characterID;
        }

        // Update game state
        GameState.Instance.RegisterMurderProfile(mp);
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
