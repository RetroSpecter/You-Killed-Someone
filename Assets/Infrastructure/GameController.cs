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
        yield return StartCoroutine(Investigation());

    }

    public IEnumerator Murder() {
        yield return null;

        yield return StartCoroutine(vc.DisplayStoryText(StoryText.YOU_KILLED_SOMEONE));

        MurderProfile mp = new MurderProfile();

        // Give Choice: Who
        DialogueChoice whoYouKilled = new DialogueChoice(DialogueChoice.WHO_YOU_KILLED, GameState.Instance.GetAliveCharacters());
        yield return StartCoroutine(vc.DisplayPrompt(whoYouKilled, SelectOption));
        //Debug.Log("Selected Character: " + whoYouKilled.options[recentlySelectedOption]);
        GameState.Instance.KillCharacter(whoYouKilled.GetOptionID(recentlySelectedOption));
        mp.murderedCharacterID = whoYouKilled.GetOptionID(recentlySelectedOption);

        // Give Choice: where
        DialogueChoice whereYouKilled = new DialogueChoice(DialogueChoice.MURDER_LOCATION, GameState.Instance.GetMurderLocations(true));
        yield return StartCoroutine(vc.DisplayPrompt(whereYouKilled, SelectOption));
        //Debug.Log("Selected location: " + whereYouKilled.options[recentlySelectedOption]);
        mp.weaponProfileID = whereYouKilled.GetOptionID(recentlySelectedOption);

        // Give Choice: how
        DialogueChoice howYouKilled = new DialogueChoice(DialogueChoice.MURDER_WEAPON, GameState.Instance.GetMurderWeapons(true));
        yield return StartCoroutine(vc.DisplayPrompt(howYouKilled, SelectOption));
        //Debug.Log("Selected method of murder: " + howYouKilled.options[recentlySelectedOption]);
        mp.locationProfileID = howYouKilled.GetOptionID(recentlySelectedOption);

        // Give Choice: did you discover it?
        DialogueChoice didYouDiscoverBody = new DialogueChoice(DialogueChoice.DISCOVER_BODY);
        yield return StartCoroutine(vc.DisplayPrompt(didYouDiscoverBody, SelectOption));
        //Debug.Log("Did you discover the body: " + didYouDiscoverBody.options[recentlySelectedOption]);
        // If you did, you are the discoverer. Otherwise, a random NPC is the discoverer
        if (didYouDiscoverBody.GetOptionID(recentlySelectedOption) == DialogueChoiceOption.YES.textID) {
            mp.bodyDiscovererID = Character.playerID;
        } else {
            var aliveCharacters = GameState.Instance.GetAliveCharacters();
            mp.bodyDiscovererID = aliveCharacters[Random.Range(0, aliveCharacters.Count)].characterID;
        }

        // Update game state
        GameState.Instance.RegisterMurderProfile(mp);
    }

    public IEnumerator Investigation() {
        var mp = GameState.Instance.GetMostRecentMurderProfile();

        // Someone announces a murder occurred
        // if you, then: you yell for everyone to come
        // if someone else, then: they yell for everyone to come

        // Everyone gathers around.

        // Fear and anger fill the room.

        // Seeds of doubt begin to sow

        // Can you get away with it?

        int totalInvestigations = 3;
        for (int i = 0; i < totalInvestigations; i++) {
            // Everyone is muttering among themselves

            // Who will you talk to?
            DialogueChoice talkToSomeone = new DialogueChoice(DialogueChoice.WHO_TO_TALK_TO, GameState.Instance.GetAliveCharacters());
            yield return StartCoroutine(vc.DisplayPrompt(talkToSomeone, SelectOption));
            Character investigatee = GameState.GetCharacter(talkToSomeone.GetOptionID(recentlySelectedOption));

            // What will you do?
            DialogueChoice askOrTell = DialogueChoice.CreateAskOrTellChoice(investigatee);
            yield return StartCoroutine(vc.DisplayPrompt(askOrTell, SelectOption));
            bool asking = recentlySelectedOption == 0;


            if (asking) {
                // character talks about themselves

            } else {

                // Randomly select between:
                //  - Talking about murder weapon
                //  - Talking about location
                //  - talking about afinity
                int topic = Random.Range(0, 3);
                string topicText;
                switch (topic) {
                    // Talk about murder weapon
                    case 0:
                        topicText = ProfileLibrary.GetWeapon(mp.weaponProfileID);
                        break;
                    case 1:
                        break;
                    case 2:
                        break;
                }


                // Who will you say?
                // Choose a character
                DialogueChoice tell = DialogueChoice.CreateYouTellChoice(investigatee, "");
                yield return StartCoroutine(vc.DisplayPrompt(tell, SelectOption));


                // Result: <Character> likes <murder weapon>
                // Result: <Character> likes <location>
                // Result: <Character> hated <dead person>

                // If correct, sus of you goes down.
                // If incorrect, sus of you goes up
            }


            // Investigatee wants to ask you a question
            // Randomly select between:
            //  - Asking about preferred tools
            //  - Asking about favorite places
            //  - Asking about occupation

            // Randomly produce four options
            // Which do you like? / Which are you?

            // Update investigatees perception of you

            yield return null;
        }
    }



    public void SelectOption(int selectedOption) {
        this.recentlySelectedOption = selectedOption;
    }
}
