using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character {

    // DISPOSITION FIELDS

    // Names
    public string characterID;
    public string fullName;
    public string nickName;

    // ID's of another character
    public string loves;
    public string hates;

    public Profile profile;

    // STATE FIELDS

    // A value from 0 - 100 (not bound)
    // Higher = worse
    // Lower = better
    public int sus;
    public bool alive;



    public Character(string characterID, string fullName, string nickName, string loves, string hates) {
        this.characterID = characterID;
        this.fullName = fullName;
        this.nickName = nickName;
        this.loves = loves;

        this.alive = true;

        if (hates == "you") {
            this.sus = 75;
        } else if (loves == "you") {
            this.sus = 25;
        } else {
            this.sus = 50;
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