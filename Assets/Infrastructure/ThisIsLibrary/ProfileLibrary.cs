using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ProfileLibrary {
    public static Dictionary<string, Profile> AssignProfiles(List<Character> characters) {

        List<Profile> allProfiles = GetAllProfiles();

        Dictionary<string, Profile> profilesUsed = new Dictionary<string, Profile>();

        for (int i = 0; i < 7; i++) {
            int r = Random.Range(0, allProfiles.Count);
            var randomProfile = allProfiles[r];

            // assign a random profile to a character and remove that profile
            characters[i].profile = randomProfile;
            profilesUsed[randomProfile.profileID] = randomProfile;
            allProfiles.RemoveAt(r);
        }


        return profilesUsed;
    }


    // Returns a list of 4 randomly selected profiles 
    public static List<Profile> GetFourProfiles() {
        List<Profile> allProfiles = GetAllProfiles();
        List<Profile> selectedProfiles = new List<Profile>();

        for (int i = 0; i < 7; i++) {
            int r = Random.Range(0, allProfiles.Count);
            var randomProfile = allProfiles[r];

            // assign a random profile to a character and remove that profile
            selectedProfiles.Add(randomProfile);
            allProfiles.RemoveAt(r);
        }

        return selectedProfiles;
    }


    private static List<Profile> GetAllProfiles() {
        return new List<Profile> {
            new Profile("scb", "Space Cowboy", "laser guns", "on he moon"),
            new Profile("chef", "5 Star Chef", "knives", "in the kitchen"),
            new Profile("phot", "Professional Photographer", "a camera", "outside"),
            new Profile("zoo", "Part Time Zoo Keeper", "trained elephants", "at the circus"),
            new Profile("plantboi", "Gardener", "a rusty hoe", "the garden"),
            new Profile("doc", "ddd", "a stethoscope", "at a hospital"),
            new Profile("bleg7", "Bleg3", "bleg4", "bleg5")
        };
    }
}
