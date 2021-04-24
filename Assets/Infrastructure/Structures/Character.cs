using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character {

    // Names
    public string characterID;
    public string fullName;
    public string nickName;

    // ID's of another character
    public string loves;
    public string hates;

    // A value from 0 - 100 (not bound)
    // Higher = worse
    // Lower = better
    public int sus;

    public Character(string characterID, string fullName, string nickName, string loves, string hates) {
        this.characterID = characterID;
        this.fullName = fullName;
        this.nickName = nickName;
        this.loves = loves;

        if (hates == "you") {
            this.sus = 75;
        } else if (loves == "you") {
            this.sus = 25;
        } else {
            this.sus = 50;
        }
    }


}
