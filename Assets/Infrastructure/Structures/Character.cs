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

    public bool alive;
    // A value from 0 - 100 (not bound)
    // Higher = worse
    // Lower = better
    public int sus;

    // Each of these are a profileID of the associated field
    public string believedPlayerOccupationID;
    public string believedPlayerToolID;
    public string believedPlayerLocationID;




    public Character(string characterID, string fullName, string nickName, string hexColor) {
        this.characterID = characterID;
        this.fullName = fullName;
        this.nickName = nickName;
        this.hexColor = hexColor;

        this.alive = true;
        this.sus = 50;
    }

    // given a profileID, returns whether it is this character's profile
    public bool HoldsCharacterProfile(string profileID) {
        return this.profile.profileID == profileID;
    }

    // Given a profileID, returns whether this lines up with what this character 
    //  thinks the player's occupation is
    public bool MatchesBelievedPlayerOccupation(string profileID) {
        return this.believedPlayerOccupationID == ""
            || this.believedPlayerOccupationID == profileID;
    }

    // Given a profileID, returns whether this lines up with what this character
    //  thinks the player's occupation is
    public bool MatchesBelievedPlayerTool(string profileID) {
        return this.believedPlayerToolID == ""
            || this.believedPlayerToolID == profileID;
    }

    // Given a profileID, returns whether this lines up with what this character 
    //  thinks the player's occupation is
    public bool MatchesBelievedPlayerLocation(string profileID) {
        return this.believedPlayerLocationID == ""
            || this.believedPlayerLocationID == profileID;
    }



    public void AdjustSusSlightly(bool increase) {
        if (increase) {
            sus += 25;
        } else {
            sus -= 25;
        }
    }

    public void AdjustSusModerately(bool increase) {
        if (increase) {
            sus += 50;
        } else {
            sus -= 50;
        }
    }
    
    public void AdjustSusGreatly(bool increase) {
        if (increase) {
            sus += 75;
        } else {
            sus -= 75;
        }
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