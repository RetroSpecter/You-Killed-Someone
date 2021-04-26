using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {


    public ViewController vc;
    private int recentlySelectedOption;

    // Start is called before the first frame update
    void Start() {

        // Initiate view?

        StartCoroutine(PlayGame());
    }

    public IEnumerator PlayGame() {
        yield return StartCoroutine(vc.DisplayStoryText(new StoryText(StoryText.YOU_KILLED_SOMEONE, new List<Character> { CharacterLibrary.PLAYER }, null, null, new TextSettings(0.5f, false, 0, true))));

        // ROUND 1
        yield return StartCoroutine(Murder());
        yield return StartCoroutine(Investigation());
        yield return StartCoroutine(Trial());
        // Round 1 Over
        yield return StartCoroutine(vc.DisplayStoryText(new StoryText("victory", "c:0 got away with the first murder", new List<Character> { CharacterLibrary.PLAYER })));
        GameState.Instance.currentRound++;

        // Preface Round 2
        yield return new WaitForSeconds(0.5f);
        yield return StartCoroutine(vc.DisplayStoryText(new StoryText("", "As much as you resist, your bloodlust gets the best of you again.")));
        yield return StartCoroutine(vc.DisplayStoryText(new StoryText(StoryText.YOU_KILLED_SOMEONE, new List<Character> { CharacterLibrary.PLAYER }, null, null, new TextSettings(0.5f, false, 0, true))));
        yield return StartCoroutine(vc.DisplayStoryText(new StoryText("", "However, everyone will remember what c:0 told them before.", new List<Character> { CharacterLibrary.PLAYER })));
        yield return StartCoroutine(vc.DisplayStoryText(new StoryText("", "Don't dig yourself too deep")));

        // ROUND 2
        yield return StartCoroutine(Murder());
        yield return StartCoroutine(Investigation());
        yield return StartCoroutine(Trial());
        // Round 2 over
        yield return StartCoroutine(vc.DisplayStoryText(new StoryText("victory", "c:0 got away with the second murder", new List<Character> { CharacterLibrary.PLAYER })));
        GameState.Instance.currentRound++;

        // Preface round 3
        yield return new WaitForSeconds(0.5f);
        yield return StartCoroutine(vc.DisplayStoryText(new StoryText("", "As much as you resist, your bloodlust gets the best of you again.")));
        yield return StartCoroutine(vc.DisplayStoryText(new StoryText(StoryText.YOU_KILLED_SOMEONE, new List<Character> { CharacterLibrary.PLAYER }, null, null, new TextSettings(0.5f, false, 0, true))));
        yield return StartCoroutine(vc.DisplayStoryText(new StoryText("", "However, everyone still remembers what c:0 told them before.", new List<Character> { CharacterLibrary.PLAYER })));
        yield return StartCoroutine(vc.DisplayStoryText(new StoryText("", "Don't dig yourself too deep")));

        // ROUND 3
        yield return StartCoroutine(Murder());
        yield return StartCoroutine(Investigation());
        yield return StartCoroutine(Trial());

        // Round 3 over
        yield return StartCoroutine(vc.DisplayStoryText(new StoryText("victory", "c:0 got away with the third murder", new List<Character> { CharacterLibrary.PLAYER })));


        yield return StartCoroutine(vc.DisplayStoryText(new StoryText("", "As much as you resist, your bloodlust gets the best of you again.")));
        yield return StartCoroutine(vc.DisplayStoryText(new StoryText(StoryText.YOU_KILLED_SOMEONE, new List<Character> { CharacterLibrary.PLAYER }, null, null, new TextSettings(0.5f, false, 0, true))));
        yield return StartCoroutine(vc.DisplayStoryText(new StoryText("", "c:0 didn't see it coming", new List<Character> { GameState.Instance.GetAliveCharacters()[0] })));

        // You won the game
        yield return new WaitForSeconds(0.5f);
        yield return StartCoroutine(vc.DisplayStoryText(new StoryText("", "There is no one left")));
        yield return StartCoroutine(vc.DisplayStoryText(new StoryText("", "Everyone is dead")));

        yield return new WaitForSeconds(1f);
        yield return StartCoroutine(vc.DisplayStoryText(new StoryText("", "Do it again?")));
        yield return StartCoroutine(vc.DisplayStoryText(new StoryText("", "Restarting the game...")));

        // reload scene
        SceneManager.LoadScene(0);
    }

    public IEnumerator Murder() {
        yield return null;

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


        yield return new WaitForSeconds(1f);
        // You murdered _ with _
        yield return StartCoroutine(vc.DisplayStoryText(new StoryText(StoryText.YOU_KILLED_X_WITH_Y_AT_Z, new List<Character>() { CharacterLibrary.PLAYER, mp.GetMurderedCharacter() }, new List<string>() { mp.GetMurderLocation() }, new List<string>() { mp.GetMurderWeapon() }, new TextSettings(0.25f, false, 2, true))));

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
        yield return new WaitForSeconds(1.5f);
        yield return StartCoroutine(vc.DisplayStoryText(new StoryText("", "NO ONE MUST KNOW", null, null, null, new TextSettings(0, false, 5, true))));

        yield return new WaitForSeconds(1f);
        int totalInvestigations = 3;
        for (int i = 0; i < totalInvestigations; i++) {
            // Everyone is muttering among themselves
            yield return StartCoroutine(vc.DisplayStoryText(new StoryText("", "Everyone is cautiously eyeing each other")));
            yield return StartCoroutine(vc.DisplayStoryText(new StoryText("", "c:0 can take s:0 actions", new List<Character> { CharacterLibrary.PLAYER }, new List<string> { (3 - i) + "" })));


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

                    // Search through the list of profiles for the murder weapon and the believed weapon
                    bool containsMurderWeapon = false, containsBelievedTool = false;
                    foreach (var p in profiles) {
                        containsMurderWeapon |= p.profileID == murderWeaponProfile.profileID;
                        containsBelievedTool |= p.profileID == investigatee.believedPlayerToolID;
                    }

                    // Ensure the murder weapon is in this list
                    if (!containsMurderWeapon) {
                        profiles[0] = murderWeaponProfile;
                    }
                    // If investigatee already has a field filled for this, ensure it is in there too
                    if (investigatee.believedPlayerToolID != "") {
                        Profile believedPlayerToolProfile = ProfileLibrary.GetProfile(investigatee.believedPlayerToolID);
                        if (!containsBelievedTool) {
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
                        investigatee.AdjustSusModerately(true);
                        Debug.Log(investigatee.nickName + "'s sus of the player has moderately raised to " + investigatee.sus);

                        yield return StartCoroutine(vc.DisplayStoryText(new StoryText("", "c:0 squints their eyes suspciously.", new List<Character> { investigatee })));
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

                    // Register Question
                    GameState.Instance.RegisterAskedQuestion(investigatee, questionType, weaponsQuestion.GetOptionID(recentlySelectedOption));

                    break;
                // Asking about favorite place
                case 1:
                    Profile murderLocationProfile = ProfileLibrary.GetProfile(mp.locationProfileID);

                    // Search through the list of profiles for the murder place and the believed place
                    bool containsMurderPlace = false, containsBelievedPlace = false;
                    foreach (var p in profiles) {
                        containsMurderPlace |= p.profileID == murderLocationProfile.profileID;
                        containsBelievedPlace |= p.profileID == investigatee.believedPlayerLocationID;
                    }

                    // Ensure the murder location is in this list
                    if (!containsMurderPlace) {
                        profiles[0] = murderLocationProfile;
                    }
                    // If investigatee already has a field filled for this, ensure it is in there too
                    if (investigatee.believedPlayerLocationID != "") {
                        Profile believedPlayerLocationProfile = ProfileLibrary.GetProfile(investigatee.believedPlayerLocationID);
                        if (!containsBelievedPlace) {
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
                        investigatee.AdjustSusModerately(true);
                        Debug.Log(investigatee.nickName + "'s sus of the player has moderately raised to " + investigatee.sus);

                        yield return StartCoroutine(vc.DisplayStoryText(new StoryText("", "c:0 squints their eyes.", new List<Character> { investigatee })));
                        yield return StartCoroutine(vc.DisplayStoryText(new StoryText("", "They look around s:0 , and glance at the body right behind you", null, new List<string> { mp.GetMurderLocation() })));
                        yield return StartCoroutine(vc.DisplayStoryText(new StoryText("", "c:0 suspicon of you has risen.", new List<Character> { investigatee })));
                    }

                    // if selected location contradicts what they know, increase sus greatly
                    if (!investigatee.MatchesBelievedPlayerLocation(selectedLocationID)) {
                        investigatee.AdjustSusGreatly(true);
                        Debug.Log(investigatee.nickName + "'s sus of the player has greatly raised to " + investigatee.sus);

                        yield return StartCoroutine(vc.DisplayStoryText(new StoryText("", "c:0 raises an eyebrow", new List<Character> { investigatee })));
                        yield return StartCoroutine(vc.DisplayStoryText(new StoryText("", "c:0 thought they heard something different before", new List<Character> { investigatee })));
                        yield return StartCoroutine(vc.DisplayStoryText(new StoryText("", "c:0 suspicon of you has risen.", new List<Character> { investigatee })));
                    }

                    // Assign the the location
                    investigatee.believedPlayerLocationID = selectedLocationID;
                    
                    // Register Question
                    GameState.Instance.RegisterAskedQuestion(investigatee, questionType, locationsQuestion.GetOptionID(recentlySelectedOption));

                    break;
                // Asking about occupation
                case 2:
                    // No need to ensure any occupation is in the list. We only care if the 
                    //  investigatee already has their occupation field filled out
                    if (investigatee.believedPlayerOccupationID != "") {

                        // Search through the list of profiles for the the believed weapon
                        bool containsBelievedOccupation = false;
                        foreach (var p in profiles) {
                            containsBelievedOccupation |= p.profileID == investigatee.believedPlayerOccupationID;
                        }


                        Profile believedPlayerOccupationProfile = ProfileLibrary.GetProfile(investigatee.believedPlayerOccupationID);
                        if (!containsBelievedOccupation) {
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

                        yield return StartCoroutine(vc.DisplayStoryText(new StoryText("", "c:0 stares you down", new List<Character> { investigatee })));
                        yield return StartCoroutine(vc.DisplayStoryText(new StoryText("", "c:0 thought they heard something different before", new List<Character> { investigatee })));
                        yield return StartCoroutine(vc.DisplayStoryText(new StoryText("", "c:0 suspicon of you has risen slightly.", new List<Character> { investigatee })));
                    }


                    bool slightly1 = false, slightly2 = false;
                    // If the murder location and/or weapon belongs to the occupation, increase sus slightly
                    if (selectedOccupationID == mp.weaponProfileID) {
                        slightly1 = true;

                        yield return StartCoroutine(vc.DisplayStoryText(new StoryText("", "c:0 looks at the w:0", new List<Character> { investigatee }, null, new List<string> { mp.GetMurderWeapon() })));
                        yield return StartCoroutine(vc.DisplayStoryText(new StoryText("", "wouldn't it make sense that a w:0 would like this", null, new List<string> { ProfileLibrary.GetProfile(selectedOccupationID).occupation })));
                    }

                    if (selectedOccupationID == mp.locationProfileID) {
                        slightly2 = true;


                        if (!slightly1)
                        {
                            yield return StartCoroutine(vc.DisplayStoryText(new StoryText("", "c:0 ponders about s:0", new List<Character> { investigatee }, new List<string> { mp.GetMurderLocation() })));
                            yield return StartCoroutine(vc.DisplayStoryText(new StoryText("", "doesn't it make sense that a w:0 would like this?", null, new List<string> { ProfileLibrary.GetProfile(selectedOccupationID).occupation })));
                        } else {
                            yield return StartCoroutine(vc.DisplayStoryText(new StoryText("", "doesn't it also make sense that a w:0 would like this place?", null, new List<string> { ProfileLibrary.GetProfile(selectedOccupationID).occupation })));
                        }
                    }

                    

                    // If the investigatee's suspiciion rose
                    if (slightly1 ^ slightly2 && !greatly) {
                        investigatee.AdjustSusSlightly(true);
                        yield return StartCoroutine(vc.DisplayStoryText(new StoryText("", "c:0 is slightly more suspicious.", new List<Character> { investigatee })));
                    } else if (slightly1 && slightly2 && !greatly) {
                        investigatee.AdjustSusModerately(true);
                        yield return StartCoroutine(vc.DisplayStoryText(new StoryText("", "c:0  suspicion has risen greatly.", new List<Character> { investigatee })));
                    } else if (greatly) {
                        if (slightly1 ^ slightly2) {
                            investigatee.AdjustSusSlightly(true);
                            yield return StartCoroutine(vc.DisplayStoryText(new StoryText("", "c:0 is slightly more suspicious.", new List<Character> { investigatee })));
                        }
                        else if (slightly1 && slightly2) {
                            investigatee.AdjustSusModerately(true);
                            yield return StartCoroutine(vc.DisplayStoryText(new StoryText("", "c:0 is slightly suspicious.", new List<Character> { investigatee })));
                        }
                        investigatee.AdjustSusGreatly( true);
                        yield return StartCoroutine(vc.DisplayStoryText(new StoryText("", "c:0 is uber sus of you now.", new List<Character> { investigatee })));
                    }

                    // Assign the the occupation
                    investigatee.believedPlayerOccupationID = selectedOccupationID;

                    // Register Question
                    GameState.Instance.RegisterAskedQuestion(investigatee, questionType, occupationsQuestion.GetOptionID(recentlySelectedOption));

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
        var mp = GameState.Instance.GetMostRecentMurderProfile();

        // get x character who is sus of the character
        // get a profile item Y that you told someone you like / are
        Character accuser;
        List<Character> allChars = GameState.Instance.GetAliveCharacters();
        allChars.Sort( (x, y) => { return y.sus - x.sus; });
        accuser = allChars[Random.Range(0, allChars.Count / 2)];

        AskedQuestion askedQuestion = GameState.Instance.GetRandomQuestion();
        string playerAnswer = askedQuestion.answerID;
        int questionType = askedQuestion.GetQuestionType();


        // Out of the blue, x addresses you in front of the group!
        yield return StartCoroutine(vc.DisplayStoryText(new StoryText("",
            "Out of the blue, c:0 addresses c:1 in front of the group!", new List<Character> { accuser, CharacterLibrary.PLAYER })));

        // They ask "don't you like y?"
        StoryText storyQuestion;
        if (questionType == 0) {
            storyQuestion = new StoryText("", "c:0 asks \"Hey, don't c:1 like w:0",
                new List<Character> { accuser, CharacterLibrary.PLAYER }, null, new List<string> { askedQuestion.GetAnswerText() });
        } else if (questionType == 1) {
            storyQuestion = new StoryText("", "c:0 asks \"Hey, don't c:1 like hanging out s:0",
                new List<Character> { accuser, CharacterLibrary.PLAYER }, new List<string> { askedQuestion.GetAnswerText() });
        } else {
            storyQuestion = new StoryText("", "c:0 asks \"Hey, aren't c:1 a s:0",
                new List<Character> { accuser, CharacterLibrary.PLAYER }, new List<string> { askedQuestion.GetAnswerText() });
        }
        DialogueChoice dontYouQuestion = new DialogueChoice(storyQuestion);
        yield return StartCoroutine(vc.DisplayPrompt(dontYouQuestion, SelectOption));


        // TODO: make it so if a character has a preconceived notion of you, they won't ask you a question

        // Yes => everyone thinks you like this item
        // No => contradicts anyone who thought you did, greatly increases sus
        if (dontYouQuestion.GetOptionID(recentlySelectedOption) == DialogueChoiceOption.YES.textID) {
            foreach (var c in GameState.Instance.GetAliveCharacters()) {
                // Check for a contradiction between player's answer and their answer just now
                // regardless of contradiction, change the chracter's belief
                if (questionType == 0) {
                    if (!c.MatchesBelievedPlayerTool(playerAnswer))
                        yield return StartCoroutine(AdjustSusGreatly(c, true));
                    c.believedPlayerToolID = playerAnswer;
                } else if (questionType == 1) {
                    if (!c.MatchesBelievedPlayerLocation(playerAnswer))
                        yield return StartCoroutine(AdjustSusGreatly(c, true));
                    c.believedPlayerLocationID = playerAnswer;
                } else if (questionType == 2) {
                    if (!c.MatchesBelievedPlayerOccupation(playerAnswer))
                        yield return StartCoroutine(AdjustSusGreatly(c, true));
                    c.believedPlayerOccupationID = playerAnswer;
                }
            }
        } else {
            // Check if any character thought you did, greatly increase their sus and remove
            foreach(var c in GameState.Instance.GetAliveCharacters()) {
                if (questionType == 0 && c.believedPlayerToolID == playerAnswer) {
                    yield return StartCoroutine(AdjustSusGreatly(c, true));
                    c.believedPlayerToolID = "";
                } else if (questionType == 1 && c.believedPlayerLocationID == playerAnswer) {
                    yield return StartCoroutine(AdjustSusGreatly(c, true));
                    c.believedPlayerLocationID = "";
                } else if (questionType == 2 && c.believedPlayerOccupationID == playerAnswer) {
                    yield return StartCoroutine(AdjustSusGreatly(c, true));
                    c.believedPlayerOccupationID = "";
                }
            }
        }
        yield return StartCoroutine(vc.DisplayStoryText(new StoryText("", "Everyone now thinks this.")));

        yield return new WaitForSeconds(0.5f);

        // Tensions rise even higher as you fall deeper and deeper into your lies
        yield return StartCoroutine(vc.DisplayStoryText(new StoryText("",
            "Tensions are rising as you fall deeper and deeper into your lies", new List<Character> { CharacterLibrary.PLAYER })));

        // Who will you pin the blame on?
        DialogueChoice blameChoice = new DialogueChoice(new StoryText("", "c:0 need an escape. Who will c:0 blame for the murder?",
            new List<Character> { CharacterLibrary.PLAYER }), GameState.Instance.GetCharacters());
        yield return StartCoroutine(vc.DisplayPrompt(blameChoice, SelectOption));
        Character characterBlamed = GameState.GetCharacter(blameChoice.GetOptionID(recentlySelectedOption));


        // You blame <blank>
        yield return StartCoroutine(vc.DisplayStoryText(new StoryText("",
            "c:0 tell everyone that c:1 killed c:2", new List<Character> { CharacterLibrary.PLAYER, characterBlamed, GameState.Instance.GetMostRecentlyKilled() })));

        bool incorrect = false;
        // People's reactions
        // If selected character is associated with weapon or place, you lose sus
        if (characterBlamed.profile.profileID == mp.weaponProfileID)
        {
            foreach (var c in GameState.Instance.GetAliveCharacters())
            {
                c.AdjustSusSlightly(false);
            }

            yield return StartCoroutine(vc.DisplayStoryText(new StoryText("", "Everyone thinks that makes sense, given c:0 likes w:0",
                new List<Character> { characterBlamed }, null, new List<string> { ProfileLibrary.GetWeapon(mp.weaponProfileID) })));
        }
        else if (characterBlamed.profile.profileID == mp.locationProfileID)
        {
            foreach (var c in GameState.Instance.GetAliveCharacters())
            {
                c.AdjustSusSlightly(false);
            }

            yield return StartCoroutine(vc.DisplayStoryText(new StoryText("", "Everyone thinks that makes sense, given c:0 likes hanging out s:0",
                new List<Character> { characterBlamed }, new List<string> { ProfileLibrary.GetWeapon(mp.locationProfileID) })));
        }
        else if (characterBlamed.profile.profileID == mp.weaponProfileID && characterBlamed.profile.profileID == mp.locationProfileID)
        {
            foreach (var c in GameState.Instance.GetAliveCharacters())
            {
                c.AdjustSusModerately(false);
            }

            yield return StartCoroutine(vc.DisplayStoryText(new StoryText("", "Everyone thinks that makes sense, given c:0 works s:0 and uses w:0",
                new List<Character> { characterBlamed }, new List<string> { ProfileLibrary.GetWeapon(mp.locationProfileID) },
                new List<string> { ProfileLibrary.GetWeapon(mp.weaponProfileID) })));
        }
        else {
            incorrect = true;
        }


        // Calculation

        int failureSusLevel = 100;
        int numCharsTooSus = 0;
        foreach (var c in GameState.Instance.GetAliveCharacters()) {
            if (c.sus >= failureSusLevel)
                numCharsTooSus++;
        }

        bool failure = numCharsTooSus >= GameState.Instance.GetAliveCharacters().Count / 2;

        if (failure) {
            // The group does not believe you. They think you did it.
            yield return StartCoroutine(vc.DisplayStoryText(new StoryText("",
                "But nobody believes c:0 - they have seen through the lies and turn on c:0", new List<Character> { CharacterLibrary.PLAYER })));

            yield return StartCoroutine(vc.DisplayStoryText(GetRandomExecutionText(CharacterLibrary.PLAYER)));

            // GAME OVER!!!
            yield return StartCoroutine(vc.DisplayStoryText(DialogueChoice.GAME_OVER));

            // reload scene
            SceneManager.LoadScene(0);

        } else {
            if (incorrect) {
                // The group believes you.
                yield return StartCoroutine(vc.DisplayStoryText(new StoryText("",
                    "The fact's don't quite line up, but everyone takes you on your word.")));

                yield return StartCoroutine(vc.DisplayStoryText(new StoryText("",
                    "The group turns turn on c:1", new List<Character> { CharacterLibrary.PLAYER, characterBlamed })));
            }
            else
            {
                // The group believes you.
                yield return StartCoroutine(vc.DisplayStoryText(new StoryText("",
                    "The group believes what c:0 say and turn on c:1", new List<Character> { CharacterLibrary.PLAYER, characterBlamed })));
            }


            // <blank> goodbye
            yield return StartCoroutine(vc.DisplayStoryText(GetRandomExecutionText(characterBlamed)));
        }



        yield return null;
    }


    public StoryText GetRandomExecutionText(Character dead) {
        List<StoryText> executionTexts = new List<StoryText> {
            new StoryText("", "c:0 will be sleeping with the fishes tonight", new List<Character> { dead }),
            new StoryText("", "c:0 was kicked out to fend for themselves", new List<Character> { dead }),
            new StoryText("", "c:0 won't be bothering anyone anytime soon", new List<Character> { dead }),
            new StoryText("", "And that was the last anyone saw of c:0", new List<Character> { dead }),
            new StoryText("", "c:0 was ejected", new List<Character> { dead }),
            new StoryText("", "c:0 was voted off the island", new List<Character> { dead }),
        };

        return executionTexts[Random.Range(0, executionTexts.Count)];
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

        if (increase)
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
