using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class CharacterLibrary {


    public static Dictionary<string, Character> LoadCharacters() {
        // Create all characters and add them to a list
        List<Character> allCharacters = new List<Character>();
        allCharacters.Add(new Character("Jerry", "Jerrance Alnerbol", "Jerry", "hex"));
        allCharacters.Add(new Character("Terry", "Terrance Boojigooba", "Terry", "hex"));
        allCharacters.Add(new Character("Larry", "Larrance Diglehopper", "Larry", "hex"));
        allCharacters.Add(new Character("Barry", "Barry Buhuju", "Barry", "hex"));
        allCharacters.Add(new Character("Claire", "Clairance LongNose", "Claire", "hex"));
        allCharacters.Add(new Character("Harry", "Harrance Pooterance", "Harry", "hex"));
        allCharacters.Add(new Character("Merry", "Merrance LittleLamb", "Merry", "hex"));
        allCharacters.Add(new Character("Garry", "Gerrance Modular", "Garry", "hex"));
        allCharacters.Add(new Character("Perry", "Perrance Platypal", "Perry", "hex"));
        // More loads


        Dictionary<string, Character> characters = new Dictionary<string, Character>();

        for (int i = 0; i < 7; i++) {
            int r = Random.Range(0, allCharacters.Count);
            var randomCharacter = allCharacters[r];

            // add this random character to the story, and remove them from the set
            characters[randomCharacter.characterID] = randomCharacter;
            allCharacters.RemoveAt(r);
        }

        return characters;
    }


    public static void AssignAffinities(List<Character> characters) {
        // Create a list of random indeces. Guarentees index != a[index]
        List<int> charIndx = Enumerable.Range(0, characters.Count).ToList();
        for (int i = characters.Count - 1; i > 0; i--) {
            int r = Random.Range(0, i);
            var temp = charIndx[i];
            charIndx[i] = charIndx[r];
            charIndx[r] = temp;
        }

        // Give every npc a random person they love
        for (int i = 0; i < characters.Count - 1; i++) {
            characters[i].loves = characters[charIndx[i]].characterID;
        }

        // Create a list of random indeces. Guarentees index != a[index]
        charIndx = Enumerable.Range(0, characters.Count).ToList();
        for (int i = characters.Count - 1; i > 0; i--) {
            int r = Random.Range(0, i);
            var temp = charIndx[i];
            charIndx[i] = charIndx[r];
            charIndx[r] = temp;
        }

        // Give every npc a random person they hate
        for (int i = 0; i < characters.Count - 1; i++) {
            characters[i].hates = characters[charIndx[i]].characterID;
        }
    }
}
