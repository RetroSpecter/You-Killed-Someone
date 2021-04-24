using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameState : MonoBehaviour {

    public static GameState Instance;

    public Dictionary<string, Character> characters;

    public void Awake() {
        if (Instance == null) {
            Instance = this;
            this.characters = CharacterLibrary.LoadCharacters();
            ProfileLibrary.AssignProfiles(this.characters.Values.ToList());
        }
    }


    public List<Character> GetAliveCharacters() {
        return new List<Character>(this.characters.Values.Where<Character>(
            character => { return character.alive; }
        ));
    }


    public List<DialogueChoiceOption> GetMurderWeapons() {
        return ProfileLibrary.GetFourProfiles().Select<Profile, DialogueChoiceOption>(
            profile => { return new DialogueChoiceOption(profile.profileID, profile.preferredTool); }
        ).ToList();
    }

    public List<DialogueChoiceOption> GetMurderLocations() {
        return ProfileLibrary.GetFourProfiles().Select<Profile, DialogueChoiceOption>(
            profile => { return new DialogueChoiceOption(profile.profileID, profile.preferredLocation); }
        ).ToList();
    }

}