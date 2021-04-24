using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


// A dialogue choice represents the information suited for when the player needs
//  to make some sort of choice.
public class DialogueChoice {

    private DialogueChoiceType type;
    public string prompt;
    public string choiceid;
    public List<DialogueChoiceOption> options;


    // Constant Dialogue ID's
    public static StoryText WHO_YOU_KILLED = new StoryText("whoYouKilled", "who did " +  StoryText.YOU + " kill" );
    public static StoryText MURDER_LOCATION = new StoryText("whereYouKilled", "where did " + StoryText.YOU + " kill" );
    public static StoryText MURDER_WEAPON = new StoryText("howYouKilled", "how did " + StoryText.YOU + " kill" );
    public static StoryText DISCOVER_BODY = new StoryText("youDiscoveredBody", "did " + StoryText.YOU + " \"discover\" the body" );

    // Yes No constructor
    public DialogueChoice(StoryText prompt) {
        this.type = DialogueChoiceType.yesNo;
        this.prompt = prompt.processText();
        this.choiceid = prompt.textID;
        this.options = new List<DialogueChoiceOption> {
            new DialogueChoiceOption(DialogueChoiceOption.YES),
            new DialogueChoiceOption(DialogueChoiceOption.NO)
        };
    }

    // Two Square constructor
    public DialogueChoice(StoryText prompt, DialogueChoiceOption firstOption, DialogueChoiceOption secondOption) {
        this.type = DialogueChoiceType.twoSquare;
        this.prompt = prompt.processText();
        this.choiceid = prompt.textID;
        this.options = new List<DialogueChoiceOption> { firstOption, secondOption };
    }

    // Four Square constructor
    public DialogueChoice(StoryText prompt, List<DialogueChoiceOption> textOptions) {
        this.type = DialogueChoiceType.fourSquare;
        this.prompt = prompt.processText();
        this.choiceid = prompt.textID;
        this.options = textOptions;
    }

    // Character Select constructor
    public DialogueChoice(StoryText prompt, List<Character> characters) {
        this.type = DialogueChoiceType.characterSelect;
        this.prompt = prompt.processText();
        this.choiceid = prompt.textID;
        // Turn each character into a dialogue choice
        this.options = new List<DialogueChoiceOption>(characters.Select<Character, DialogueChoiceOption>(
            character => {
                return new DialogueChoiceOption("<color=yellow>" + character.characterID + "</color>");
            }
        ));
    }


    public bool isYesNo() {
        return this.type == DialogueChoiceType.yesNo;
    }

    public bool isTwo() {
        return this.type == DialogueChoiceType.twoSquare;
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
        twoSquare,
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

    public static StoryText WEAPON = new StoryText("weapon", "w:0" );
    public static StoryText SCENERY = new StoryText("scenery", "s:0" );
    public static StoryText CHARACTER = new StoryText("weapon", "c:0" );

    public DialogueChoiceOption(StoryText text)
    {
        this.optionID = text.textID;
        this.optionText = text.processText();
    }

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

public class CharacterText {
    public Character character;
    public CharacterTextType textType;
    public string text;
    public List<string> characters;
    public List<string> scenery;
    public List<string> weapon;


    public CharacterText(Character speaker, CharacterTextType textType) {
        this.character = speaker;
        this.textType = textType;

        switch (textType) {
            case CharacterTextType.bodyDiscovery:
                // Oh no!
                break;
            case CharacterTextType.self:
                // Character introduction
                break;
            case CharacterTextType.positivieReaction:
                // generic "sounds reasonable"
                break;
            case CharacterTextType.negativeReaction:
                // generic "hmmm...."
                break;
            case CharacterTextType.question:
                // What do you like?
                break;
            default:
                Debug.LogError("No Recognized Type provided");
                break;
        }
    }

    public enum CharacterTextType {
        bodyDiscovery,
        self,
        positivieReaction,
        negativeReaction,
        question
    }






    private string processText() {
        string ret = "";

        string[] words = text.Split(' ');

        for (int i = 0; i < words.Length; i++) {
            string word = words[i];

            if (word.StartsWith("c:")) {
                int val = int.Parse(word.Split(':')[1]);
                ret += characters[val] + " ";
            } else if (word.StartsWith("s:")) {
                int val = int.Parse(word.Split(':')[1]);
                ret += characters[val] + " ";
            } else if (word.StartsWith("w:")) {
                int val = int.Parse(word.Split(':')[1]);
                ret += characters[val] + " ";
            } else {
                ret += words[i] + " ";
            }
        }

        return ret;
    }
}



// Text created 
public class StoryText {
    public string textID;
    public string text;
    public List<string> characters;
    public List<string> scenery;
    public List<string> weapon;

    public StoryText(StoryText storyText, List<string> characters = null, List<string> scenery = null, List<string> weapon = null) {
        this.textID = storyText.textID;
        this.text = storyText.text;

        this.characters = characters;
        this.scenery = scenery;
        this.weapon = weapon;
    }

    public StoryText(string textID, string text, List<string> characters = null, List<string> scenery = null, List<string> weapon = null) {
        this.textID = textID;
        this.text = text;

        this.characters = characters;
        this.scenery = scenery;
        this.weapon = weapon;
    }

    public string processText() {
        string ret = "";

        string[] words = text.Split(' ');

        for (int i = 0; i < words.Length; i++) {
            string word = words[i];

            if (word.StartsWith("c:")) {
                int val = int.Parse(word.Split(':')[1]);
                ret += characters[val] + " ";
            } else if (word.StartsWith("s:")) {
                int val = int.Parse(word.Split(':')[1]);
                ret += characters[val] + " ";
            } else if (word.StartsWith("w:")) {
                int val = int.Parse(word.Split(':')[1]);
                ret += characters[val] + " ";
            } else {
                ret += words[i] + " ";
            }
        }

        return ret;
    }

    public static string YOU = "YOU";

    public static StoryText YOU_KILLED_SOMEONE = new StoryText("killed",  "c:0 killed someone" , new List<string> { YOU });
    public static StoryText YOU_KILLED_X_WITH_Y_AT_Z = new StoryText("killedXYZ", "You killed %s with %s" );
    public static StoryText X_FINDS_THE_BODY = new StoryText("findBody", "%s finds the body" );

    public static StoryText X_LIKES_Y = new StoryText("likes", "%s likes %s" );
    public static StoryText X_HATES_Y = new StoryText("hates", "%s hates %s" );

    public static StoryText WHAT_DO_YOU_WANT_TO_DO_WITH = new StoryText("ask", "What do you want to do with %s" );
    public static StoryText ASK_X_ABOUT_THEMSELVES = new StoryText("ask", "Ask %s about themselves" );
    public static StoryText TELL_X_ABOUT_SOMETHING = new StoryText("tell", "Tell %s something" );
    public static StoryText X_WILL_REMEMBER_THAT = new StoryText("remember", "%s will remember that" );

    public static StoryText X_AGREES = new StoryText("agrees", "%s agrees" );
    public static StoryText X_DISAGREES = new StoryText("disagrees",  "%s is not convinced" );

    public static StoryText X_WANTS_TO_KNOW = new StoryText("questionWeapon", "%s wants to know " );
    public static StoryText X_WANTS_TO_KNOW_WEAPON = new StoryText("questionWeapon", "What weapon do you like?" );
    public static StoryText X_WANTS_TO_KNOW_PLACE = new StoryText("questionPlace", "What place do you like?" );

    public static StoryText X_SAYS_Y = new StoryText("xSaysY", "%s says %s" );
    public static StoryText X_IS_KILLED = new StoryText("xIsKilled", "The group kills %s" );

    public static StoryText THE_PLOT_THICKENS = new StoryText("plotThickens", "The plot thickens" );
    public static StoryText YOU_WIN = new StoryText("win","Everyone is dead. You have fullfilled your mission" );
    public static StoryText YOU_FAIL = new StoryText("win", "You have failed" );
}

