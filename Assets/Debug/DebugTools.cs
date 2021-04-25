using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugTools : MonoBehaviour
{
    public static DebugTools instance;

    private void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked;
        Application.targetFrameRate = 60;
        if (instance == null)
        {
            instance = this;
        } else {
            Destroy(this.gameObject);
        }
    }
    void Update()
    {


        if (Input.GetKeyDown(KeyCode.P)) Time.timeScale = (Time.timeScale == 0) ? 1 : 0;

        if (Input.GetKeyDown(KeyCode.Escape)) Application.Quit();

        //if (Input.GetKeyDown(KeyCode.UpArrow)) GoToNextLevel(1);
    }

    void GoToNextLevel(int num) {
        print("go");
        int targetLevel = Application.loadedLevel + num;
        targetLevel %= Application.levelCount;
        Application.LoadLevel(targetLevel);
    }
}
