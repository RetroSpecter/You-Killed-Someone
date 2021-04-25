using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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

    public static Profile GetProfile(string profileID) {
        return GetAllProfileMappings()[profileID];
    }

    // Given a profile id, returns a weapon 
    public static string GetWeapon(string profileID) {
        return GetAllProfileMappings()[profileID].preferredTool;
    }

    public static string GetLocation(string profileID) {
        return GetAllProfileMappings()[profileID].preferredLocation;
    }

    // Returns a list of 4 randomly selected profiles
    // If ensure living is true, then at least one of these profiles will match
    //  a currently alive character
    public static List<Profile> GetFourProfiles(bool ensureLiving = false) {
        List<Profile> allProfiles = GetAllProfiles();
        List<Profile> selectedProfiles = new List<Profile>();
        HashSet<string> livingProfiles = GameState.Instance.GetAliveProfileID();

        bool matchesLiving = false;
        for (int i = 0; i < 4; i++) {
            int r = Random.Range(0, allProfiles.Count);
            var randomProfile = allProfiles[r];

            // assign a random profile to a character and remove that profile
            selectedProfiles.Add(randomProfile);
            allProfiles.RemoveAt(r);

            matchesLiving |= (livingProfiles.Contains(randomProfile.profileID));
        }

        if (ensureLiving) {
            // Keep pulling profiles until you get one that is living
            while (!matchesLiving) {
                selectedProfiles.RemoveAt(0);

                int r = Random.Range(0, allProfiles.Count);
                var randomProfile = allProfiles[r];

                // assign a random profile to a character and remove that profile
                selectedProfiles.Add(randomProfile);
                allProfiles.RemoveAt(r);

                matchesLiving |= (livingProfiles.Contains(randomProfile.profileID));
            }
        }

        return selectedProfiles;
    }


    // Returns a list of all provi =
    private static List<Profile> GetAllProfiles() {
        return new List<Profile> {
            new Profile("scb", "Space Cowboy", "laser guns", "on the moon"),
            new Profile("chef", "5 Star Chef", "knives", "in the kitchen"),
            new Profile("phot", "Professional Photographer", "a camera", "outside"),
            new Profile("zoo", "Part Time Zoo Keeper", "trained elephants", "at the circus"),
            new Profile("plantboi", "Gardener", "a rusty hoe", "in the garden"),
            new Profile("doc", "Doctor", "a stethoscope", "at the hospital"),
            new Profile("asas", "Assasin", "poison", "in a dark allyway")
        };
    }


    private static Dictionary<string, Profile> profileMappings;
    private static Dictionary<string, Profile> GetAllProfileMappings() {
        if (profileMappings == null) {
            profileMappings = GetAllProfiles().ToDictionary(
                profile => { return profile.profileID; }
            );
        }
        return profileMappings;
    }
}
