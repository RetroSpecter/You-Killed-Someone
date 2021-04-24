using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameState : MonoBehaviour {

    public static GameState Instance;

    public Dictionary<string, Character> characters;
    private MurderProfile round1;


    public void Awake() {
        if (Instance == null) {
            Instance = this;
            this.characters = CharacterLibrary.LoadCharacters();
            ProfileLibrary.AssignProfiles(this.characters.Values.ToList());
        }
    }


    public void RegisterMurderProfile(MurderProfile mp) {
        this.round1 = mp;
    }


    public List<Character> GetAliveCharacters() {
        return new List<Character>(this.characters.Values.Where<Character>(
            character => { return character.alive; }
        ));
    }


    public List<DialogueChoiceOption> GetMurderWeapons() {
        return ProfileLibrary.GetFourProfiles().Select<Profile, DialogueChoiceOption>(
            profile => {  return new DialogueChoiceOption(new StoryText(profile.profileID, "w:0", null, null, new List<string>() { profile.preferredTool })); }
        ).ToList();
    }

    public List<DialogueChoiceOption> GetMurderLocations() {
        return ProfileLibrary.GetFourProfiles().Select<Profile, DialogueChoiceOption>(
            //profile => { return new DialogueChoiceOption(profile.profileID, profile.preferredLocation); }
            profile => { return new DialogueChoiceOption(new StoryText(profile.profileID, "s:0", null, new List<string>() { profile.preferredLocation }, null)); }
        ).ToList();
    }


    public void KillCharacter(string murderedCharacterID) {
        var character = this.characters[murderedCharacterID];
        character.alive = false;
    }

    public Character GetMostRecentlyKilled() {
        return this.round1.GetMurderedCharacter();
    }
}


// Create a murder profile