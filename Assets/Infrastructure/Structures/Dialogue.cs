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
    public const string MURDER_WEAPON = "how did you kill";
    public const string DISCOVER_BODY = "did you discover the body";

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
        this.type = DialogueChoiceType.fourSquare;
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

    public override string ToString() {
        return this.optionText;
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

    public static string YOU = "YOU";

    public static StoryText X_KILLED_SOMEONE = new StoryText("killed", new string[] { "{1} killed someone" });
    public static StoryText YOU_KILLED_X_WITH_Y_AT_Z = new StoryText("killedXYZ", new string[] { "You killed %s with %s" });
    public static StoryText X_FINDS_THE_BODY = new StoryText("findBody", new string[] { "%s finds the body" });

    public static StoryText X_LIKES_Y = new StoryText("likes", new string[] { "%s likes %s" });
    public static StoryText X_HATES_Y = new StoryText("hates", new string[] { "%s hates %s" });

    public static StoryText WHAT_DO_YOU_WANT_TO_DO_WITH = new StoryText("ask", new string[] { "What do you want to do with %s" });
    public static StoryText ASK_X_ABOUT_THEMSELVES = new StoryText("ask", new string[] { "Ask %s about themselves" });
    public static StoryText TELL_X_ABOUT_SOMETHING = new StoryText("tell", new string[] { "Tell %s something" });
    public static StoryText X_WILL_REMEMBER_THAT = new StoryText("remember", new string[] { "%s will remember that" });

    public static StoryText X_AGREES = new StoryText("agrees", new string[] { "%s agrees" });
    public static StoryText X_DISAGREES = new StoryText("disagrees", new string[] { "%s is not convinced" });

    public static StoryText X_WANTS_TO_KNOW = new StoryText("questionWeapon", new string[] { "%s wants to know " });
    public static StoryText X_WANTS_TO_KNOW_WEAPON = new StoryText("questionWeapon", new string[] { "What weapon do you like?" });
    public static StoryText X_WANTS_TO_KNOW_PLACE = new StoryText("questionPlace", new string[] { "What place do you like?" });

    public static StoryText X_SAYS_Y = new StoryText("xSaysY", new string[] { "%s says %s" });
    public static StoryText X_IS_KILLED = new StoryText("xIsKilled", new string[] { "The group kills %s" });

    public static StoryText THE_PLOT_THICKENS = new StoryText("plotThickens", new string[] { "The plot thickens" });
    public static StoryText YOU_WIN = new StoryText("win", new string[] { "Everyone is dead. You have fullfilled your mission" });
    public static StoryText YOU_FAIL = new StoryText("win", new string[] { "You have failed" });
}

