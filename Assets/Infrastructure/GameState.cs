using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameState : MonoBehaviour {

    public static GameState Instance;

    public Dictionary<string, Character> characters;
    private MurderProfile round1;

    //debug fields
    public string deadCharacter, weapon, location, discoverer;


    public void Awake() {
        if (Instance == null) {
            Instance = this;
            this.characters = CharacterLibrary.LoadCharacters();
            ProfileLibrary.AssignProfiles(this.characters.Values.ToList());
        }
    }


    public void RegisterMurderProfile(MurderProfile mp) {
        this.round1 = mp;
        deadCharacter = mp.murderedCharacterID;
        weapon = ProfileLibrary.GetWeapon(mp.weaponProfileID);
        location = ProfileLibrary.GetLocation(mp.locationProfileID);
        discoverer = mp.bodyDiscovererID;
    }

    public static Character GetCharacter(string characterID) {
        return Instance.characters[characterID];
    }


    public List<Character> GetAliveCharacters() {
        return new List<Character>(this.characters.Values.Where<Character>(
            character => { return character.alive; }
        ));
    }

    public HashSet<string> GetAliveProfileID() {
        return new HashSet<string>(this.GetAliveCharacters().Select(
            character => { return character.profile.profileID; }
        ));
    }


    public List<DialogueChoiceOption> GetMurderWeapons(bool ensureOneLiving = false) {
        return ProfileLibrary.GetFourProfiles(ensureOneLiving).Select<Profile, DialogueChoiceOption>(
            profile => {  return new DialogueChoiceOption(profile.profileID , new StoryText(profile.profileID, "w:0", null, null, new List<string>() { profile.preferredTool })); }
        ).ToList();
    }

    public List<DialogueChoiceOption> GetMurderLocations(bool ensureOneLiving = false) {
        return ProfileLibrary.GetFourProfiles(ensureOneLiving).Select<Profile, DialogueChoiceOption>(
            //profile => { return new DialogueChoiceOption(profile.profileID, profile.preferredLocation); }
            profile => { return new DialogueChoiceOption(profile.profileID, new StoryText(profile.profileID, "s:0", null, new List<string>() { profile.preferredLocation }, null)); }
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