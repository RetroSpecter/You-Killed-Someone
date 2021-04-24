using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class CharacterLibrary {


    public static Dictionary<string, Character> LoadCharacters() {
        // Create all characters and add them to a list
        List<Character> allCharacters = new List<Character>();
        allCharacters.Add(new Character("Jerry", "Jerrance Alnerbol", "Jerry", "hex"));
        allCharacters.Add(new Character("Jerry2", "Jerrance Alnerbol2", "Jerry2", "hex"));
        allCharacters.Add(new Character("Jerry3", "Jerrance Alnerbol3", "Jerry3", "hex"));
        allCharacters.Add(new Character("Jerry4", "Jerrance Alnerbol4", "Jerry4", "hex"));
        allCharacters.Add(new Character("Jerry5", "Jerrance Alnerbol5", "Jerry5", "hex"));
        allCharacters.Add(new Character("Jerry6", "Jerrance Alnerbol6", "Jerry6", "hex"));
        allCharacters.Add(new Character("Jerry7", "Jerrance Alnerbol7", "Jerry7", "hex"));
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
