using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MurderProfile {

    public string murderedCharacterID;
    public string weaponProfileID;
    public string locationProfileID;
    public string bodyDiscovererID;



    public MurderProfile() { }

    public Character GetMurderedCharacter() {
        if (this.murderedCharacterID == "") {
            Debug.LogError("This Murder Profile's murdered character has not been assigned");
        }
        return GameState.Instance.characters[murderedCharacterID];
    }

    public string GetMurderWeapon() {
        if (this.weaponProfileID == "") {
            Debug.LogError("This Murder Profile's murder weapon has not been assigned");
        }
        return ProfileLibrary.GetWeapon(this.weaponProfileID);
    }

    public string GetMurderLocation() {
        if (this.locationProfileID == "") {
            Debug.LogError("This Murder Profile's murder location has not been assigned");
        }
        return ProfileLibrary.GetLocation(this.locationProfileID);
    }

    // Returns null if you discovered the body
    public Character GetBodyDiscoverer() {
        if (this.locationProfileID == "") {
            Debug.LogError("This Murder Profile's body Discoverer has not been assigned");
        }

        if (bodyDiscovererID == Character.playerID) {
            return null;
        }
        return GameState.Instance.characters[bodyDiscovererID];
    }
}


