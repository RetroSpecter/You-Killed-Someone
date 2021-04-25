using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


// A dialogue choice represents the information suited for when the player needs
//  to make some sort of choice.
public class DialogueChoice {

    private DialogueChoiceType type;
    public StoryText prompt;
    public string choiceid;
    public List<DialogueChoiceOption> options;


    // Constant Dialogue ID's
    public static StoryText WHO_YOU_KILLED = new StoryText("whoYouKilled", "who did c:0 kill", new List<string> { StoryText.YOU });
    public static StoryText MURDER_LOCATION = new StoryText("whereYouKilled", "where did c:0 kill", new List<string> { StoryText.YOU });
    public static StoryText MURDER_WEAPON = new StoryText("howYouKilled", "how did c:0 kill", new List<string> { StoryText.YOU });
    public static StoryText DISCOVER_BODY = new StoryText("youDiscoveredBody", "did c:0 \"discover\" the body", new List<string> { StoryText.YOU });

    public static StoryText WHO_TO_TALK_TO = new StoryText("talkToWhom", "Who will c:0 talk to?", new List<string> { StoryText.YOU });

    // Yes No constructor
    public DialogueChoice(StoryText prompt) {
        this.type = DialogueChoiceType.yesNo;
        this.prompt = prompt;
        this.choiceid = prompt.textID;
        this.options = new List<DialogueChoiceOption> {
            new DialogueChoiceOption("yes", DialogueChoiceOption.YES),
            new DialogueChoiceOption("no", DialogueChoiceOption.NO)
        };
    }

    // Two Square constructor
    public DialogueChoice(StoryText prompt, DialogueChoiceOption firstOption, DialogueChoiceOption secondOption) {
        this.type = DialogueChoiceType.twoSquare;
        this.prompt = prompt;
        this.choiceid = prompt.textID;
        this.options = new List<DialogueChoiceOption> { firstOption, secondOption };
    }

    // Four Square constructor
    public DialogueChoice(StoryText prompt, List<DialogueChoiceOption> textOptions) {
        this.type = DialogueChoiceType.fourSquare;
        this.prompt = prompt;
        this.choiceid = prompt.textID;
        this.options = textOptions;
    }

    // Character Select constructor
    public DialogueChoice(StoryText prompt, List<Character> characters) {
        this.type = DialogueChoiceType.characterSelect;
        this.prompt = prompt;
        this.choiceid = prompt.textID;
        // Turn each character into a dialogue choice
        this.options = new List<DialogueChoiceOption>(characters.Select<Character, DialogueChoiceOption>(
            character => {
                return new DialogueChoiceOption(character.characterID, new StoryText("c:0", "c:0", new List<string>() { character.fullName }));
            }
        ));
    }

    public static DialogueChoice CreateAskOrTellChoice(Character beingTalkedto) {
        StoryText talkOrTell = new StoryText("AskOrTell", "Will c:0 ask or tell c:1?", new List<string> { StoryText.YOU , beingTalkedto.fullName});
        return new DialogueChoice(talkOrTell, DialogueChoiceOption.ASK, DialogueChoiceOption.TELL);
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

    public string GetOptionID(int option) {
        return this.options[option].optionID;
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
    public StoryText optionText;

    public static StoryText YES = new StoryText("yes", "yes");
    public static StoryText NO = new StoryText("no", "no");

    public static DialogueChoiceOption ASK
        = new DialogueChoiceOption("ask", new StoryText("ASK", "Ask"));
    public static DialogueChoiceOption TELL
        = new DialogueChoiceOption("tell", new StoryText("TELL", "Tell"));

    public static StoryText WEAPON = new StoryText("weapon", "w:0" );
    public static StoryText SCENERY = new StoryText("scenery", "s:0" );
    public static StoryText CHARACTER = new StoryText("weapon", "c:0" );

    public DialogueChoiceOption(string optionID, StoryText text)
    {
        this.optionID = optionID;
        this.optionText = text;
    }

    public override string ToString() {
        return this.optionText.ProcessText();
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
    public List<string> weapons;

    public StoryText(StoryText storyText, List<string> characters = null, List<string> scenery = null, List<string> weapon = null) {
        this.textID = storyText.textID;
        this.text = storyText.text;

        this.characters = characters;
        this.scenery = scenery;
        this.weapons = weapon;
    }

    public StoryText(string textID, string text, List<string> characters = null, List<string> scenery = null, List<string> weapon = null) {
        this.textID = textID;
        this.text = text;

        this.characters = characters;
        this.scenery = scenery;
        this.weapons = weapon;
    }

    public string ProcessText() {
        string ret = "";

        string[] words = text.Split(' ');

        for (int i = 0; i < words.Length; i++) {
            string word = words[i];

            if (word.StartsWith("c:")) {
                int val = int.Parse(word.Split(':')[1]);
                ret += characters[val] + " ";
            } else if (word.StartsWith("s:")) {
                int val = int.Parse(word.Split(':')[1]);
                ret += scenery[val] + " ";
            } else if (word.StartsWith("w:")) {
                int val = int.Parse(word.Split(':')[1]);
                ret += weapons[val] + " ";
            } else {
                ret += words[i] + " ";
            }
        }

        return ret;
    }

    public List<Color> MapColor() {
        List<Color> ret = new List<Color>();
        string[] words = text.Split(' ');

        for (int i = 0; i < words.Length; i++)
        {
            string word = words[i];

            if (word.StartsWith("c:")) {
                int val = int.Parse(word.Split(':')[1]);
                foreach (char c in characters[val])
                    ret.Add(Color.red);
            } else if (word.StartsWith("s:")) {
                int val = int.Parse(word.Split(':')[1]);

                foreach(char c in scenery[val])
                    ret.Add(Color.green);
            } else if (word.StartsWith("w:")) {
                int val = int.Parse(word.Split(':')[1]);

                foreach(char c in weapons[val])
                    ret.Add(Color.yellow);
            } else {
                foreach(char c in word)
                    ret.Add(Color.white);
            }

            ret.Add(Color.white); // space
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

