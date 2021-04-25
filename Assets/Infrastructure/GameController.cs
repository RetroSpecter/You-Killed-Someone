﻿using System.Collections;
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

        // temp stuff
        yield return StartCoroutine(vc.DisplayStoryText(new StoryText("", "No one has pieced it together. You are in the clear.")));
        yield return StartCoroutine(vc.DisplayStoryText(new StoryText("", "However, people will remember what you told them.")));
        yield return StartCoroutine(vc.DisplayStoryText(new StoryText("", "Don't dig yourself too deep")));

        yield return new WaitForSeconds(0.5f);
        yield return StartCoroutine(vc.DisplayStoryText(new StoryText("", "As much as you resist, your bloodlust gets the best of you.")));
        yield return StartCoroutine(Murder());
    }

    public IEnumerator Murder() {
        yield return null;

        yield return StartCoroutine(vc.DisplayStoryText(new StoryText(StoryText.YOU_KILLED_SOMEONE, new List<Character> { CharacterLibrary.PLAYER }, null, null, new TextSettings(0.5f, false, 0, true))));

        MurderProfile mp = new MurderProfile();

        // Give Choice: Who
        DialogueChoice whoYouKilled = new DialogueChoice(DialogueChoice.WHO_YOU_KILLED, GameState.Instance.GetCharacters());
        yield return StartCoroutine(vc.DisplayPrompt(whoYouKilled, SelectOption));
        yield return null;

        Debug.Log("Selected Character: " + whoYouKilled.options[recentlySelectedOption]);
        GameState.Instance.KillCharacter(whoYouKilled.GetOptionID(recentlySelectedOption));
        mp.murderedCharacterID = whoYouKilled.GetOptionID(recentlySelectedOption);


        // Give Choice: how
        DialogueChoice howYouKilled = new DialogueChoice(DialogueChoice.MURDER_WEAPON, GameState.Instance.GetMurderWeapons(true));
        yield return StartCoroutine(vc.DisplayPrompt(howYouKilled, SelectOption));
        yield return null;

        Debug.Log("Selected method of murder: " + howYouKilled.options[recentlySelectedOption]);
        Debug.Log("Selected option ID: " + howYouKilled.options[recentlySelectedOption].optionID);
        mp.weaponProfileID = howYouKilled.GetOptionID(recentlySelectedOption);


        // Give Choice: where
        DialogueChoice whereYouKilled = new DialogueChoice(DialogueChoice.MURDER_LOCATION, GameState.Instance.GetMurderLocations(true));
        yield return StartCoroutine(vc.DisplayPrompt(whereYouKilled, SelectOption));
        yield return null;

        Debug.Log("Selected location: " + whereYouKilled.options[recentlySelectedOption]);
        mp.locationProfileID = whereYouKilled.GetOptionID(recentlySelectedOption);


        // You murdered _ with _
        yield return StartCoroutine(vc.DisplayStoryText(new StoryText(StoryText.YOU_KILLED_X_WITH_Y_AT_Z, new List<Character>() { CharacterLibrary.PLAYER, mp.GetMurderedCharacter() }, new List<string>() { mp.GetMurderLocation() }, new List<string>() { mp.GetMurderWeapon() }, new TextSettings(0.15f, false, 2, true))));

        //// Give Choice: did you discover it?
        //DialogueChoice didYouDiscoverBody = new DialogueChoice(DialogueChoice.DISCOVER_BODY);
        //yield return StartCoroutine(vc.DisplayPrompt(didYouDiscoverBody, SelectOption));
        //Debug.Log("Did you discover the body: " + didYouDiscoverBody.options[recentlySelectedOption]);
        // If you did, you are the discoverer. Otherwise, a random NPC is the discoverer
        //if (didYouDiscoverBody.GetOptionID(recentlySelectedOption) == DialogueChoiceOption.YES.textID) {
        //    mp.bodyDiscovererID = CharacterLibrary.PLAYER.characterID;
        //} else {

        // No choice to select who finds the body
        {
            var aliveCharacters = GameState.Instance.GetAliveCharacters();
            mp.bodyDiscovererID = aliveCharacters[Random.Range(0, aliveCharacters.Count)].characterID;
        }

        // Update game state
        GameState.Instance.RegisterMurderProfile(mp);
        yield return new WaitForSeconds(1);
    }

    public IEnumerator Investigation() {
        var mp = GameState.Instance.GetMostRecentMurderProfile();
        yield return StartCoroutine(vc.DisplayStoryText(new StoryText(StoryText.X_FINDS_THE_BODY, new List<Character>() { mp.GetBodyDiscoverer() }, null, null, new TextSettings(0, false, 10, true))));

        yield return StartCoroutine(vc.DisplayStoryText(new StoryText("", "Everyone gathers s:0", null, new List<string> { mp.GetMurderLocation() })));
        //yield return StartCoroutine(vc.DisplayStoryText(new StoryText("", "tension fills the atmopshere")));

        yield return StartCoroutine(vc.DisplayStoryText(new StoryText("", "NO ONE MUST KNOW", null, null, null, new TextSettings(0, false, 5, true))));

        yield return new WaitForSeconds(1f);
        int totalInvestigations = 3;
        for (int i = 0; i < totalInvestigations; i++) {
            // Everyone is muttering among themselves
            yield return StartCoroutine(vc.DisplayStoryText(new StoryText("", "Everyone is cautiously eyeing each other")));
            yield return StartCoroutine(vc.DisplayStoryText(new StoryText("", "c:0 can talk to s:0 more people", new List<Character> { CharacterLibrary.PLAYER }, new List<string> { (3 - i) + "" })));


            // Who will you talk to?
            DialogueChoice talkToSomeone = new DialogueChoice(DialogueChoice.WHO_TO_TALK_TO, GameState.Instance.GetCharacters());
            yield return StartCoroutine(vc.DisplayPrompt(talkToSomeone, SelectOption));
            Character investigatee = GameState.GetCharacter(talkToSomeone.GetOptionID(recentlySelectedOption));

            // What will you do?
            DialogueChoice askOrTell = DialogueChoice.CreateAskOrTellChoice(investigatee);
            yield return StartCoroutine(vc.DisplayPrompt(askOrTell, SelectOption));
            bool asking = recentlySelectedOption == 0;


            if (asking) {             
                // character talks about themselves
                yield return StartCoroutine(vc.DisplayStoryText(new StoryText("", "They introduce themselves as c:0", new List<Character> { investigatee })));
                yield return StartCoroutine(vc.DisplayStoryText(new StoryText("", "They are a s:0", null, new List<string> { investigatee.profile.occupation })));
                yield return StartCoroutine(vc.DisplayStoryText(new StoryText("", "They like to use w:0", null, null, new List<string> { investigatee.profile.preferredTool })));
                yield return StartCoroutine(vc.DisplayStoryText(new StoryText("", "And they enjoy hanging out s:0", null, new List<string> { investigatee.profile.preferredLocation })));

                investigatee.profileKnown = true;
            } else {

                // Randomly select between:
                //  - Talking about murder weapon
                //  - Talking about location
                int topic = Random.Range(0, 2);
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
                    investigatee.AdjustSusSlightly(correct);
                    yield return StartCoroutine(vc.DisplayStoryText(new StoryText("", "c:0 nods", new List<Character> { investigatee })));
                    yield return StartCoroutine(vc.DisplayStoryText(new StoryText("", "c:0 is definetly a fan of that", new List<Character> { selectedCharacter })));
                    yield return StartCoroutine(vc.DisplayStoryText(new StoryText("", "c:0 trusts you a bit more", new List<Character> { investigatee })));
                }
                else {
                    investigatee.AdjustSusModerately(false);
                    yield return StartCoroutine(vc.DisplayStoryText(new StoryText("", "c:0 raises an eyebrow", new List<Character> { investigatee })));
                    yield return StartCoroutine(vc.DisplayStoryText(new StoryText("", "c:0 doesn't seem like the person who'd like that", new List<Character> { selectedCharacter })));
                    yield return StartCoroutine(vc.DisplayStoryText(new StoryText("", "c:0 is more suspicious of you", new List<Character> { investigatee })));
                }
            }


            // Investigatee wants to ask you a question
            yield return StartCoroutine(vc.DisplayStoryText(new StoryText("", "c:0 wants to ask c:1 a question",
                new List<Character> { investigatee, CharacterLibrary.PLAYER })));

            // Randomly select between:
            //  - Asking about preferred tools
            //  - Asking about favorite places
            //  - Asking about occupation
            int questionType = Random.Range(0, 3);
            Debug.Log("Selected Topic is " + new string[] { "preffered tools", "favorite place", "occupation" }[questionType]);

            // Randomly produce four options
            var profiles = ProfileLibrary.GetFourProfiles();

            // Which do you like? / Which are you?
            switch (questionType) {
                // Asking about preferred tools
                case 0:
                    Profile murderWeaponProfile = ProfileLibrary.GetProfile(mp.weaponProfileID);
                    // Ensure the murder weapon is in this list
                    if (!profiles.Contains(murderWeaponProfile)) {
                        profiles[0] = murderWeaponProfile;
                    }
                    // If investigatee already has a field filled for this, ensure it is in there too
                    if (investigatee.believedPlayerToolID != "") {
                        Profile believedPlayerToolProfile = ProfileLibrary.GetProfile(investigatee.believedPlayerToolID);
                        if (!profiles.Contains(believedPlayerToolProfile)) {
                            profiles[3] = believedPlayerToolProfile;
                        }
                    }

                    // Reshuffle the list of profiles
                    for (int j = profiles.Count - 1; j > 0; j--) {
                        int r = Random.Range(0, j);
                        var temp = profiles[j];
                        profiles[j] = profiles[r];
                        profiles[r] = temp;
                    }

                    // NPC Asks about if the player preferrs any of the above weapons
                    DialogueChoice weaponsQuestion = DialogueChoice.CreateAQuestion(investigatee, profiles);
                    yield return StartCoroutine(vc.DisplayPrompt(weaponsQuestion, SelectOption));
                    string selectedWeaponID = weaponsQuestion.GetOptionID(recentlySelectedOption);

                    // If selected weapon is murder weapon, increase sus moderately
                    if (selectedWeaponID == mp.weaponProfileID) {
                        investigatee.AdjustSusModerately(true);
                        Debug.Log(investigatee.nickName + "'s sus of the player has moderately raised to " + investigatee.sus);

                        yield return StartCoroutine(vc.DisplayStoryText(new StoryText("", "c:0 squints his eyes suspciously.", new List<Character> { investigatee })));
                        yield return StartCoroutine(vc.DisplayStoryText(new StoryText("", "They glance at the w:0 sticking out of c:0 's back", new List<Character> { mp.GetMurderedCharacter() }, null, new List<string> { mp.GetMurderWeapon() })));
                    }

                    // if selected weapon contradicts what they know, increase sus greatly
                    if (!investigatee.MatchesBelievedPlayerTool(selectedWeaponID)) {
                        investigatee.AdjustSusGreatly(true);
                        Debug.Log(investigatee.nickName + "'s sus of the player has greatly raised to " + investigatee.sus);

                        yield return StartCoroutine(vc.DisplayStoryText(new StoryText("", "c:0 raises an eyebrow", new List<Character> { investigatee })));
                        yield return StartCoroutine(vc.DisplayStoryText(new StoryText("", "c:0 thought they heard something different before", new List<Character> { investigatee })));
                    }

                    // Assign the the weapon
                    investigatee.believedPlayerToolID = selectedWeaponID;

                    break;
                // Asking about favorite place
                case 1:
                    Profile murderLocationProfile = ProfileLibrary.GetProfile(mp.locationProfileID);
                    // Ensure the murder location is in this list
                    if (!profiles.Contains(murderLocationProfile)) {
                        profiles[0] = murderLocationProfile;
                    }
                    // If investigatee already has a field filled for this, ensure it is in there too
                    if (investigatee.believedPlayerLocationID != "") {
                        Profile believedPlayerLocationProfile = ProfileLibrary.GetProfile(investigatee.believedPlayerLocationID);
                        if (!profiles.Contains(believedPlayerLocationProfile)) {
                            profiles[3] = believedPlayerLocationProfile;
                        }
                    }

                    // Reshuffle the list of profiles
                    for (int j = profiles.Count - 1; j > 0; j--) {
                        int r = Random.Range(0, j);
                        var temp = profiles[j];
                        profiles[j] = profiles[r];
                        profiles[r] = temp;
                    }

                    // NPC Asks about if the player preferrs any of the above locations
                    DialogueChoice locationsQuestion = DialogueChoice.CreateAQuestion(investigatee, null, profiles);
                    yield return StartCoroutine(vc.DisplayPrompt(locationsQuestion, SelectOption));
                    string selectedLocationID = locationsQuestion.GetOptionID(recentlySelectedOption);

                    // If selected location is murder location, increase sus moderately
                    if (selectedLocationID == mp.locationProfileID) {
                        investigatee.AdjustSusModerately(true);
                        Debug.Log(investigatee.nickName + "'s sus of the player has moderately raised to " + investigatee.sus);

                        yield return StartCoroutine(vc.DisplayStoryText(new StoryText("", "c:0 squints his eyes suspciously.", new List<Character> { investigatee })));
                        yield return StartCoroutine(vc.DisplayStoryText(new StoryText("", "They look around s:0 , and glance at the body right behind you", null, new List<string> { mp.GetMurderLocation() })));
                    }

                    // if selected location contradicts what they know, increase sus greatly
                    if (!investigatee.MatchesBelievedPlayerLocation(selectedLocationID)) {
                        investigatee.AdjustSusGreatly(true);
                        Debug.Log(investigatee.nickName + "'s sus of the player has greatly raised to " + investigatee.sus);

                        yield return StartCoroutine(vc.DisplayStoryText(new StoryText("", "c:0 raises an eyebrow", new List<Character> { investigatee })));
                        yield return StartCoroutine(vc.DisplayStoryText(new StoryText("", "c:0 thought they heard something different before", new List<Character> { investigatee })));
                    }

                    // Assign the the location
                    investigatee.believedPlayerLocationID = selectedLocationID;

                    break;
                // Asking about occupation
                case 2:
                    // No need to ensure any occupation is in the list. We only care if the 
                    //  investigatee already has their occupation field filled out
                    if (investigatee.believedPlayerOccupationID != "") {
                        Profile believedPlayerOccupationProfile = ProfileLibrary.GetProfile(investigatee.believedPlayerOccupationID);
                        if (!profiles.Contains(believedPlayerOccupationProfile)) {
                            profiles[Random.Range(0,3)] = believedPlayerOccupationProfile;
                        }
                    }


                    // NPC Asks about if the player is any of the above occupations
                    DialogueChoice occupationsQuestion = DialogueChoice.CreateAQuestion(investigatee, null, profiles);
                    yield return StartCoroutine(vc.DisplayPrompt(occupationsQuestion, SelectOption));
                    string selectedOccupationID = occupationsQuestion.GetOptionID(recentlySelectedOption);

                    // If the murder location and/or weapon belongs to the occupation, increase sus slightly
                    if (selectedOccupationID == mp.weaponProfileID) {
                        investigatee.AdjustSusSlightly(true);
                        Debug.Log(investigatee.nickName + "'s sus of the player has slightly raised to " + investigatee.sus);
                    }

                    if (selectedOccupationID == mp.locationProfileID) {
                        investigatee.AdjustSusSlightly(true);
                        Debug.Log(investigatee.nickName + "'s sus of the player has slightly raised to " + investigatee.sus);
                    }

                    // if selected location contradicts what they know, increase sus greatly
                    if (!investigatee.MatchesBelievedPlayerOccupation(selectedOccupationID)) {
                        investigatee.AdjustSusGreatly(true);
                        Debug.Log(investigatee.nickName + "'s sus of the player has greatly raised to " + investigatee.sus);
                    }

                    // Assign the the occupation
                    investigatee.believedPlayerOccupationID = selectedOccupationID;

                    break;
                // Error?
                default:
                    break;
            }

            yield return StartCoroutine(vc.DisplayStoryText(new StoryText("", "c:0 will remember this", new List<Character> { investigatee })));
            // End of investigation round
        }
    }

    public IEnumerator Trial() {
        // Tensions rise even higher as you fall deeper and deeper into your lies

        // Out of the blue, x addresses you in front of the group!


        // get x character who is sus of the character
        // get a profile item Y that you told someone you like / are
        // They ask "don't you like y?" 
            // Yes => everyone thinks you like this item
            // No => contradicts anyone who thought you did, greatly increases sus

        // Who will you pin the blame on?
        // Select a character

        // If selected character is associated with weapon or place, you may lose sus

        // You blame <blank>

        // People's reactions

        // Calculation

        // If people confirm:
            // The group believes you.
            // <blank> goodbye
            // Onto next round
        // Else:
            // The group does not believe you. They think you did it.
            // This is the end.
            // <you> goodbye
            // Game over. Restart?
                // Yes -> restart
                // No -> quit
         

        yield return null;
    }

    public IEnumerator AdjustSusSlightly(Character c, bool increase)
    {
        c.AdjustSusSlightly(increase);

        if (increase)
            yield return StartCoroutine(vc.DisplayStoryText(new StoryText(StoryText.X_AGREES, new List<Character> { c })));
        else
            yield return StartCoroutine(vc.DisplayStoryText(new StoryText(StoryText.X_DISAGREES, new List<Character> { c })));
    }

    public IEnumerator AdjustSusModerately(Character c, bool increase)
    {
        c.AdjustSusModerately(increase);

        if (increase) {
            yield return StartCoroutine(vc.DisplayStoryText(new StoryText("", "c:0 nods reassuredly", new List<Character> { c })));
        } else {
            yield return StartCoroutine(vc.DisplayStoryText(new StoryText("", "c:0 seems a bit suspicious", new List<Character> { c })));
        }
    }

    public IEnumerator AdjustSusGreatly(Character c, bool increase)
    {
        c.AdjustSusGreatly(increase);
        if (increase)
            yield return StartCoroutine(vc.DisplayStoryText(new StoryText("", "c:0 trusts you completely", new List<Character> { c })));
        else
            yield return StartCoroutine(vc.DisplayStoryText(new StoryText("", "c:0 doesn't believe a single word you said.", new List<Character> { c })));

    }

    public void SelectOption(int selectedOption) {
        this.recentlySelectedOption = selectedOption;
    }
}
