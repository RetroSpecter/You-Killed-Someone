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
        yield return StartCoroutine(Trial());

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
        yield return StartCoroutine(vc.DisplayStoryText(new StoryText("", "tension fills the atmopshere")));

        yield return StartCoroutine(vc.DisplayStoryText(new StoryText(StoryText.THE_PLOT_THICKENS, null, null, null, new TextSettings(1.5f, false, 2, false))));

        int totalInvestigations = 3;
        for (int i = 0; i < totalInvestigations; i++) {
            // Everyone is muttering among themselves
            //yield return StartCoroutine(vc.DisplayStoryText(new StoryText("", "Everyone is cautiously eyeing each other")));

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
                //yield return StartCoroutine(vc.DisplayStoryText(new StoryText("", "c:0 really likes s:0", new List<Character> { investigatee }, new List<string> { investigatee.loves })));
                //yield return StartCoroutine(vc.DisplayStoryText(new StoryText("", "and despises s:0", null, new List<string> { investigatee.hates })));


                // their occupation
                // their favorite object
                // who they like
                // who they dislike

            }
            else {

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
                    yield return StartCoroutine(AdjustSusSlightly(investigatee, false));
                } else {
                    yield return StartCoroutine(AdjustSusModerately(investigatee, true));
                }
            }



            // QUESTION
            // QUESTION
            // QUESTION
            // QUESTION
            // QUESTION
            // QUESTION
            // QUESTION



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

            bool moderate = false, greatly = false;
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


                    // TODO: Add custom messages for specific situations
                    // if selected weapon contradicts what they know, increase sus greatly
                    if (!investigatee.MatchesBelievedPlayerTool(selectedWeaponID)) {
                        //investigatee.AdjustSusGreatly(true);
                        greatly = true;
                    }

                    // If selected weapon is murder weapon, increase sus moderately
                    if (selectedWeaponID == mp.weaponProfileID) {
                        //investigatee.AdjustSusModerately(true);
                        moderate = true;
                    }


                    // TODO: DEPRICATE:
                    // If the investigatee's suspiciion rose
                    if (moderate && !greatly) {
                        yield return StartCoroutine(AdjustSusModerately(investigatee, true));
                    } else if (greatly) {
                        if (moderate) {
                            investigatee.AdjustSusModerately(true);
                        }
                        yield return StartCoroutine(AdjustSusGreatly(investigatee, true));
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



                    // TODO: Add custom messages for specific situations
                    // if selected location contradicts what they know, increase sus greatly
                    if (!investigatee.MatchesBelievedPlayerLocation(selectedLocationID)) {
                        greatly = true;
                        //investigatee.AdjustSusGreatly(true);
                        //Debug.Log(investigatee.nickName + "'s sus of the player has greatly raised to " + investigatee.sus);
                    }

                    // If selected location is murder location, increase sus moderately
                    if (selectedLocationID == mp.locationProfileID) {
                        moderate = true;
                        //investigatee.AdjustSusModerately(true);
                        //Debug.Log(investigatee.nickName + "'s sus of the player has moderately raised to " + investigatee.sus);
                    }

                    // TODO: Depricate
                    // If the investigatee's suspiciion rose
                    if (moderate && !greatly) {
                        yield return StartCoroutine(AdjustSusModerately(investigatee, true));
                    } else if (greatly) {
                        if (moderate) {
                            investigatee.AdjustSusModerately(true);
                        }
                        yield return StartCoroutine(AdjustSusGreatly(investigatee, true));
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





                    // TODO: Add custom messages for specific situations
                    // if selected location contradicts what they know, increase sus greatly
                    if (!investigatee.MatchesBelievedPlayerOccupation(selectedOccupationID)) {
                        greatly = true;
                        //investigatee.AdjustSusGreatly(true);
                        //Debug.Log(investigatee.nickName + "'s sus of the player has greatly raised to " + investigatee.sus);
                    }


                    bool slightly1 = false, slightly2 = false;
                    // If the murder location and/or weapon belongs to the occupation, increase sus slightly
                    if (selectedOccupationID == mp.weaponProfileID) {
                        slightly1 = true;
                        //investigatee.AdjustSusSlightly(true);
                        //Debug.Log(investigatee.nickName + "'s sus of the player has slightly raised to " + investigatee.sus);
                    }
                    if (selectedOccupationID == mp.locationProfileID) {
                        slightly2 = true;
                        //investigatee.AdjustSusSlightly(true);
                        //Debug.Log(investigatee.nickName + "'s sus of the player has slightly raised to " + investigatee.sus);
                    }

                    


                    // TODO: Depricate
                    // If the investigatee's suspiciion rose
                    if (slightly1 ^ slightly2 && !greatly) {
                        yield return StartCoroutine(AdjustSusSlightly(investigatee, true));
                    } else if (slightly1 && slightly2 && !greatly) {
                        yield return StartCoroutine(AdjustSusModerately(investigatee, true));
                    } else if (greatly) {
                        if (slightly1 ^ slightly2) {
                            investigatee.AdjustSusSlightly(true);
                        } else if (slightly1 && slightly2) {
                            investigatee.AdjustSusModerately(true);
                        }
                        yield return StartCoroutine(AdjustSusGreatly(investigatee, true));
                    }

                    // Assign the the occupation
                    investigatee.believedPlayerOccupationID = selectedOccupationID;

                    break;
                // Error?
                default:
                    break;
            }

            // End of investigation round
        }
    }

    public IEnumerator Trial() {
        // Tensions rise even higher as you fall deeper and deeper into your lies
        yield return StartCoroutine(vc.DisplayStoryText(new StoryText("",
            "Tensions are rising as you fall deeper and deeper into your lies", new List<Character> { CharacterLibrary.PLAYER })));


        // get x character who is sus of the character
        // get a profile item Y that you told someone you like / are
        Character blamer;
        List<Character> allChars = GameState.Instance.GetAliveCharacters();
        //yield return StartCoroutine(AdjustSusGreatly(allChars[2], true));
        //yield return StartCoroutine(AdjustSusSlightly(allChars[4], false));
        allChars.Sort( (x, y) => { return y.sus - x.sus; });
        //foreach(var c in allChars) {
        //    Debug.Log(c.nickName + ": " + c.sus);
        //}


        // Out of the blue, x addresses you in front of the group!

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
            yield return StartCoroutine(vc.DisplayStoryText(new StoryText("", "c:0 seems a bit suspicious of c:1", new List<Character> { c, CharacterLibrary.PLAYER })));
        else
            yield return StartCoroutine(vc.DisplayStoryText(new StoryText("", "c:0 seems a little less warry of you", new List<Character> { c })));
    }

    public IEnumerator AdjustSusModerately(Character c, bool increase)
    {
        c.AdjustSusModerately(increase);

        if(increase)
            yield return StartCoroutine(vc.DisplayStoryText(new StoryText("", "c:0 is clearly uneasy", new List<Character> { c })));
        else
            yield return StartCoroutine(vc.DisplayStoryText(new StoryText("", "c:0 feel c:1 relax after hearing that", new List<Character> { CharacterLibrary.PLAYER, c })));


    }

    public IEnumerator AdjustSusGreatly(Character c, bool increase)
    {
        c.AdjustSusGreatly(increase);
        if (increase)
            yield return StartCoroutine(vc.DisplayStoryText(new StoryText("", "c:0 doesn't believe a single word you said.", new List<Character> { c })));
        else
            yield return StartCoroutine(vc.DisplayStoryText(new StoryText("", "c:0 trusts you completely", new List<Character> { c })));

    }

    public void SelectOption(int selectedOption) {
        this.recentlySelectedOption = selectedOption;
    }
}
