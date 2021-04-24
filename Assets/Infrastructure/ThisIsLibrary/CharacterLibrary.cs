using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CharacterLibrary {


    public static Dictionary<string, Character> LoadCharacters() {
        Dictionary<string, Character> characters = new Dictionary<string, Character>();

        var jerry = new Character("Jerry", "Jerrance Alnerbol", "Jerry", "Jerry", "you");
        var jerry2 = new Character("Jerry2", "Jerrance Alnerbol2", "Jerry2", "Jerry", "you");
        var jerry3 = new Character("Jerry3", "Jerrance Alnerbol3", "Jerry3", "Jerry", "you");
        var jerry4 = new Character("Jerry4", "Jerrance Alnerbol4", "Jerry4", "Jerry", "you");
        var jerry5 = new Character("Jerry5", "Jerrance Alnerbol5", "Jerry5", "Jerry", "you");
        var jerry6 = new Character("Jerry6", "Jerrance Alnerbol6", "Jerry6", "Jerry", "you");
        var jerry7 = new Character("Jerry7", "Jerrance Alnerbol7", "Jerry7", "Jerry", "you");
        // More loads

        characters[jerry.characterID] = jerry;
        characters[jerry2.characterID] = jerry2;
        characters[jerry3.characterID] = jerry3;
        characters[jerry4.characterID] = jerry4;
        characters[jerry5.characterID] = jerry5;
        characters[jerry6.characterID] = jerry6;
        characters[jerry7.characterID] = jerry7;
        // more assigns

        return characters;
    }

}
