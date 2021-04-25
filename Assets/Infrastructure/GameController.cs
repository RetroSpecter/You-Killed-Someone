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
        yield return null;

        Debug.Log("Selected Character: " + whoYouKilled.options[recentlySelectedOption]);
        GameState.Instance.KillCharacter(whoYouKilled.GetOptionID(recentlySelectedOption));
        mp.murderedCharacterID = whoYouKilled.GetOptionID(recentlySelectedOption);

        // Give Choice: where
        DialogueChoice whereYouKilled = new DialogueChoice(DialogueChoice.MURDER_LOCATION, GameState.Instance.GetMurderLocations(true));
        yield return StartCoroutine(vc.DisplayPrompt(whereYouKilled, SelectOption));
        yield return null;

        Debug.Log("Selected location: " + whereYouKilled.options[recentlySelectedOption]);
        mp.locationProfileID = whereYouKilled.GetOptionID(recentlySelectedOption);

        // Give Choice: how
        DialogueChoice howYouKilled = new DialogueChoice(DialogueChoice.MURDER_WEAPON, GameState.Instance.GetMurderWeapons(true));
        yield return StartCoroutine(vc.DisplayPrompt(howYouKilled, SelectOption));
        yield return null;

        Debug.Log("Selected method of murder: " + howYouKilled.options[recentlySelectedOption]);
        Debug.Log("Selected option ID: " + howYouKilled.options[recentlySelectedOption].optionID);
        mp.weaponProfileID = howYouKilled.GetOptionID(recentlySelectedOption);

        // You murdered _ with _
        yield return StartCoroutine(vc.DisplayStoryText(new StoryText(StoryText.YOU_KILLED_X_WITH_Y_AT_Z, new List<Character>() { CharacterLibrary.PLAYER, mp.GetMurderedCharacter() }, new List<string>() { mp.GetMurderLocation() }, new List<string>() { mp.GetMurderWeapon() })));

        // Give Choice: did you discover it?
        DialogueChoice didYouDiscoverBody = new DialogueChoice(DialogueChoice.DISCOVER_BODY);
        yield return StartCoroutine(vc.DisplayPrompt(didYouDiscoverBody, SelectOption));
        //Debug.Log("Did you discover the body: " + didYouDiscoverBody.options[recentlySelectedOption]);
        // If you did, you are the discoverer. Otherwise, a random NPC is the discoverer
        if (didYouDiscoverBody.GetOptionID(recentlySelectedOption) == DialogueChoiceOption.YES.textID) {
            mp.bodyDiscovererID = CharacterLibrary.PLAYER.characterID;
        } else {
            var aliveCharacters = GameState.Instance.GetAliveCharacters();
            mp.bodyDiscovererID = aliveCharacters[Random.Range(0, aliveCharacters.Count)].characterID;
        }


        // Update game state
        GameState.Instance.RegisterMurderProfile(mp);
    }

    public IEnumerator Investigation() {
        var mp = GameState.Instance.GetMostRecentMurderProfile();
        yield return StartCoroutine(vc.DisplayStoryText(new StoryText(StoryText.X_FINDS_THE_BODY, new List<Character>() { mp.GetBodyDiscoverer() })));

        yield return StartCoroutine(vc.DisplayStoryText(new StoryText("", "Everyone gathers s:0", null, new List<string> { mp.GetMurderLocation() })));
        yield return StartCoroutine(vc.DisplayStoryText(new StoryText("", "tension fills the atmopshere")));

        yield return StartCoroutine(vc.DisplayStoryText(StoryText.THE_PLOT_THICKENS));

        int totalInvestigations = 3;
        for (int i = 0; i < totalInvestigations; i++) {
            // Everyone is muttering among themselves
            yield return StartCoroutine(vc.DisplayStoryText(new StoryText("", "Everyone is cautiously eyeing each other")));

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
                yield return StartCoroutine(vc.DisplayStoryText(new StoryText("", "They introduce themselves as c:0", new List<Character> { })));


                // gives their name
                // their occupation
                // their favorite object
                // who they like
                // who they dislike

            }
            else {

                // Randomly select between:
                //  - Talking about murder weapon
                //  - Talking about location
                //  - talking about afinity
                int topic = Random.Range(0, 3);
                Debug.Log("Selected Topic is " + new string[] {"weapon", "location", "affinity" }[topic]);

                // Choose a character you will tell <investigatee> <something> about
                DialogueChoice tell;
                switch (topic) {
                    // Talk about murder weapon
                    case 0:
                        tell = DialogueChoice.CreateYouTellChoice(investigatee, ProfileLibrary.GetWeapon(mp.weaponProfileID));
                        break;
                    // Talk about Location 
                    case 1:
                        tell = DialogueChoice.CreateYouTellChoice(investigatee, "", ProfileLibrary.GetLocation(mp.locationProfileID));
                        break;
                    // Talk about Affinity
                    case 2:
                        tell = DialogueChoice.CreateYouTellChoice(investigatee, "", "", mp.GetMurderedCharacter());
                        break;
                    default:
                        tell = null;
                        Debug.Log("random did not roll a number 0, 1, or 2");
                        break;
                }
                yield return StartCoroutine(vc.DisplayPrompt(tell, SelectOption));
                Character selectedCharacter = GameState.GetCharacter(tell.GetOptionID(recentlySelectedOption));

                bool correct;
                switch (topic) {
                    // Talk about murder weapon
                    case 0:
                        correct = selectedCharacter.HoldsCharacterProfile(mp.weaponProfileID);
                        break;
                    // Talk about Location 
                    case 1:
                        correct = selectedCharacter.HoldsCharacterProfile(mp.locationProfileID);
                        break;
                    // Talk about Affinity
                    case 2:
                        correct = selectedCharacter.hates == mp.murderedCharacterID;
                        break;
                    default:
                        correct = false;
                        Debug.Log("random did not roll a number 0, 1, or 2");
                        break;
                }


                // If correct, sus of you goes down.
                // If incorrect, sus of you goes up
                Debug.Log("What you told " + investigatee.nickName + " is " + correct);
                if (correct) {
                    investigatee.AdjustSusSlightly(false);
                } else {
                    investigatee.AdjustSusModerately(true);
                }
            }


            // Investigatee wants to ask you a question
            yield return StartCoroutine(vc.DisplayStoryText(new StoryText("", "c:0 wants to ask c:1 a question",
                new List<Character> { investigatee, CharacterLibrary.PLAYER })));

            // Randomly select between:
            //  - Asking about preferred tools
            //  - Asking about favorite places
            //  - Asking about occupation
            int question = Random.Range(0, 3);
            Debug.Log("Selected Topic is " + new string[] { "preffered tools", "favorite place", "occupation" }[question]);

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
