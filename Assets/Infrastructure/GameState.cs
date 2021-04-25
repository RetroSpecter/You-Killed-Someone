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

    [SerializeField]
    public InspectorCharacter[] debugs;


    public void Awake() {
        if (Instance == null) {
            Instance = this;
            this.characters = CharacterLibrary.LoadCharacters();
            ProfileLibrary.AssignProfiles(this.characters.Values.ToList());
            CharacterLibrary.AssignAffinities(characters.Values.ToList());
            debugs = new InspectorCharacter[7];

        }
    }


    int updateClock = 0;
    public void Update() {
        updateClock++;
        if (updateClock % 30 == 0) {
            List<Character> blegs = characters.Values.ToList();
            for (int i = 0; i < blegs.Count; i++) {
                var bleg = blegs[i];
                debugs[i] = new InspectorCharacter(
                    bleg.characterID,
                    bleg.nickName,
                    bleg.profile.occupation,
                    GetCharacter(bleg.loves).nickName,
                    GetCharacter(bleg.hates).nickName,
                    bleg.sus,
                    bleg.believedPlayerOccupationID,
                    bleg.believedPlayerToolID,
                    bleg.believedPlayerLocationID
                );
            }
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
        if (characterID == null || characterID == "")
            return CharacterLibrary.PLAYER;

        return Instance.characters[characterID];
    }


    public List<Character> GetAliveCharacters() {
        return new List<Character>(this.characters.Values.Where<Character>(
            character => { return character.alive; }
        ));
    }

    public List<Character> GetCharacters()
    {
        return new List<Character>(this.characters.Values);
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

    public MurderProfile GetMostRecentMurderProfile() {
        return this.round1;
    }
}

[System.Serializable]
public struct InspectorCharacter {

    public string characterID;
    public string characterNickName;
    public string occupation;
    public string loves;
    public string hates;


    public int sus;
    public string believedPlayerOccupationID;
    public string believedPlayerToolID;
    public string believedPlayerLocationID;

    public InspectorCharacter(string id, string nickname, string occupation,
        string loves, string hates, int sus, string bpo, string bpt, string bpl) {
        this.characterID = id;
        this.characterNickName = nickname;
        this.occupation = occupation;
        this.loves = loves;
        this.hates = hates;
        this.sus = sus;
        this.believedPlayerOccupationID = bpo;
        this.believedPlayerToolID = bpt;
        this.believedPlayerLocationID = bpl;
    }

}

// Create a murder profile