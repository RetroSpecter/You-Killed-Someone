using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CharacterLibrary {


    public static Dictionary<string, Character> LoadCharacters() {
        // Create all characters and add them to a list
        List<Character> allCharacters = new List<Character>();
        allCharacters.Add(new Character("Jerry", "Jerrance Alnerbol", "Jerry", "Jerry", "you"));
        allCharacters.Add(new Character("Jerry2", "Jerrance Alnerbol2", "Jerry2", "Jerry", "you"));
        allCharacters.Add(new Character("Jerry3", "Jerrance Alnerbol3", "Jerry3", "Jerry", "you"));
        allCharacters.Add(new Character("Jerry4", "Jerrance Alnerbol4", "Jerry4", "Jerry", "you"));
        allCharacters.Add(new Character("Jerry5", "Jerrance Alnerbol5", "Jerry5", "Jerry", "you"));
        allCharacters.Add(new Character("Jerry6", "Jerrance Alnerbol6", "Jerry6", "Jerry", "you"));
        allCharacters.Add(new Character("Jerry7", "Jerrance Alnerbol7", "Jerry7", "Jerry", "you"));
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

}
