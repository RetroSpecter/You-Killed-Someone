using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CharacterLibrary {


    public static Dictionary<string, Character> LoadCharacters() {
        Dictionary<string, Character> characters = new Dictionary<string, Character>();

        var jerry = new Character("Jerry", "Jerrance Alnerbol", "Jerry", "Jerry", "you");
        // More loads

        characters[jerry.ID] = jerry;
        // more assigns

        return characters;
    }

}
