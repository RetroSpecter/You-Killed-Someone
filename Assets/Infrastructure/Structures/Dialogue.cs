using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


// A dialogue choice represents the information suited for when the player needs
//  to make some sort of choice.
public class DialogueChoice {

    private DialogueChoiceType type;
    public string choiceID;
    public string prompt;
    public List<DialogueChoiceOption> options;


    // Constant Dialogue ID's
    public const string WHO_YOU_KILLED = "who did you kill";
    public const string MURDER_LOCATION = "where did you kill";
    public const string MURDER_METHOD = "how did you kill";

    // Yes No constructor
    public DialogueChoice(string choiceID, string prompt) {
        this.type = DialogueChoiceType.yesNo;
        this.choiceID = choiceID;
        this.prompt = prompt;
        this.options = new List<DialogueChoiceOption> {
            new DialogueChoiceOption(DialogueChoiceOption.YES),
            new DialogueChoiceOption(DialogueChoiceOption.NO)
        };

    }

    // Four Square constructor
    public DialogueChoice(string choiceID, string prompt, List<DialogueChoiceOption> textOptions) {
        this.type = DialogueChoiceType.yesNo;
        this.choiceID = choiceID;
        this.prompt = prompt;
        this.options = textOptions;
    }

    // Character Select constructor
    public DialogueChoice(string choiceID, string prompt, List<Character> characters) {
        this.type = DialogueChoiceType.characterSelect;
        this.choiceID = choiceID;
        this.prompt = prompt;
        // Turn each character into a dialogue choice
        this.options = new List<DialogueChoiceOption>(characters.Select<Character, DialogueChoiceOption>(
            character => {
                return new DialogueChoiceOption(character.characterID);
            }
        ));
    }


    public bool isYesNo() {
        return this.type == DialogueChoiceType.yesNo;
    }

    public bool isFour() {
        return this.type == DialogueChoiceType.fourSquare;
    }

    public bool isCharacterSelect() {
        return this.type == DialogueChoiceType.characterSelect;
    }


    // Three options for dialogue choices:
    //  - Yes or No
    //  - Choose one of four options
    //  - Select a character
    private enum DialogueChoiceType {
        yesNo,
        fourSquare,
        characterSelect
    }
}


// A dialogue choice option is a single option (text) in a choice
public class DialogueChoiceOption {
    public string optionID;
    public string optionText;

    public const string YES = "Yes";
    public const string NO = "No";

    public DialogueChoiceOption(string text) {
        this.optionID = text;
        this.optionText = text;
    }

    public DialogueChoiceOption(string optionID, string optionText) {
        this.optionID = optionID;
        this.optionText = optionText;
    }
}




// Text created 
public class StoryText {
    public string textID;
    public string[] text;

    public StoryText(string textID, string[] text) {
        this.textID = textID;
        this.text = text;
    }



    
    public static StoryText YOU_KILLED_SOMEONE = new StoryText("killed", new string[] { "You killed someone" });
}

