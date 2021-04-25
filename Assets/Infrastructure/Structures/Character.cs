using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character {

    // PERSONALITY

    // Names
    public string characterID;
    public string fullName;
    public string nickName;

    public const string playerID = "player";


    // Text
    public string hexColor;


    // PROFILE FIELDS

    // both of these are ID's of another character
    public string loves;
    public string hates;

    public Profile profile;


    // STATE FIELDS

    // A value from 0 - 100 (not bound)
    // Higher = worse
    // Lower = better
    public int sus;
    public bool alive;



    public Character(string characterID, string fullName, string nickName, string hexColor) {
        this.characterID = characterID;
        this.fullName = fullName;
        this.nickName = nickName;
        this.hexColor = hexColor;

        this.alive = true;
    }

    // given a profileID, returns whether that is this character's profile
    public bool MatchesCharacterProfile(string profileID) {
        return this.profile.profileID == profileID;
    }
}


public class Profile {
    public string profileID;
    public string occupation;
    public string preferredTool;
    public string preferredLocation;

    public Profile(string profileID, string occupation, string preferredTool, string preferredLocation) {
        this.profileID = profileID;
        this.occupation = occupation;
        this.preferredTool = preferredTool;
        this.preferredLocation = preferredLocation;
    }
}