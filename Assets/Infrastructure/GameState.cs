using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameState : MonoBehaviour {

    public static GameState Instance;

    public Dictionary<string, Character> characters;

    public void Awake() {
        if (Instance == null) {
            Instance = this;
            this.characters = CharacterLibrary.LoadCharacters();
        }
    }


    public List<Character> GetAliveCharacters() {
        return new List<Character>(this.characters.Values.Where<Character>(
            character => { return character.alive; }
        ));
    }


    public List<DialogueChoiceOption> GetMurderLocations() {
        return new List<DialogueChoiceOption> {
                new DialogueChoiceOption("", "On a box"),
                new DialogueChoiceOption("", "with a fox"),
                new DialogueChoiceOption("", "On a lake"),
                new DialogueChoiceOption("", "With Josh & Drake")
            };
    }

    public List<DialogueChoiceOption> GetMurderMethods() {
        return new List<DialogueChoiceOption> {
                new DialogueChoiceOption("", "With a Knife"),
                new DialogueChoiceOption("", "With your looks"),
                new DialogueChoiceOption("", "Death by very fast train"),
                new DialogueChoiceOption("", "With Josh & Drake")
            };
    }
}