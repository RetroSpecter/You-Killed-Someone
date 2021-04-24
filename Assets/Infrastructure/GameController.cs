using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {


    int recentlySelectedOption;

    // Start is called before the first frame update
    void Start() {

        // Initiate view
        // 
    }

    public IEnumerator PlayGame() {
        yield return StartCoroutine(Murder());

    }

    public IEnumerator Murder() {
        // You murdered someoe

        // Give Choice: Who
        // Give Choice: where
        // Give Choice: how
        // Give Choice: did you discover it?

        // Update game state
        yield return null;
    }

    public IEnumerator Investigation() {
        // Someone announces a murder occurred (is it you? if not, who?)

        // Everyone gathers
        // Character gets an entrance line
        // character reacts to death


        // we tell player to investigate
        // Who do want to talk to?

        int totalInvestigations = 3;
        for (int i = 0; i < totalInvestigations; i++) {

            yield return null;
        }


    }



    public void SelectOption(int selectedOption) {
        this.recentlySelectedOption = selectedOption;
    }
}
