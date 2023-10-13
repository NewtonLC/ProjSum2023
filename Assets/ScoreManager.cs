using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class ScoreManager : MonoBehaviour {
    //Savedata script
    savedata savedataScript;

    //Static variables to hold the player's stats and information
    static public int numProblemsAnswered;
    static public int[] numProblemsAnsweredPerOperator = new int[] {0,0,0,0,0};
    static public int numProblemsCorrect;
    static public int[] numProblemsCorrectPerOperator = new int[] {0,0,0,0,0};
    static public int playerLives = 3;
    static public string difficulty = "easy";
    static public string gameMode = "time";
    //Access operators list through ButtonBehavior.operators

    //Variables to handle game-end scenarios(besides playerLives == 0)
    static public int numProblems = 15;
    static public float numSeconds = 30;

    //Text to display player's lives. This will only display on "Survival" mode
    public TMP_Text livesDisplay;

    void Start(){
        savedataScript = GameObject.FindGameObjectWithTag("SaveData").GetComponent<savedata>();
    }

    // Update is called once per frame
    void Update() {
        livesDisplay.text = string.Equals(gameMode,"survival")?"Lives: " + playerLives.ToString():"";

        if (string.Equals(gameMode, "survival") && playerLives == 0 && !ScoreScreenText.gameOver){
            //Player loses the game
            Debug.Log("YOU LOSE");

            //Game end, with stats.
            gameEnd();
        }

        if (string.Equals(gameMode, "problems") && numProblems == 0){
            Debug.Log("You answered " + (numProblemsAnswered) + " problems!");

            gameEnd();
        }

        if (string.Equals(gameMode, "time") && numSeconds < RoundTimer.amountTimeElapsed){
            Debug.Log(((int)numSeconds).ToString() + " seconds have passed!");

            gameEnd();
        }
    }

    // Method to process clicking the QUIT button.
    // In zen mode, the QUIT button ends the game normally.
    // In other modes, the QUIT button returns to the main menu.
    public void gameQuit(){
        switch(gameMode){
            case "zen":
                gameEnd();
                break;
            default:
                toMainMenu();
                break;
        }
    }

    //Method that returns to main menu
    void toMainMenu(){
        SceneManager.LoadScene("MainMenuScreen");
        Debug.Log(gameMode);
        ScoreScreenManager.enterMainMenu = true;
    }

    // Method to process game end.
    // Called when...
        // Player time runs out
        // Player loses all lives
        // Player answers all necessary questions
    void gameEnd(){
        //Load the Score Screen scene
        //savedataScript.gameEnd();
        savedata.Instance.gameEnd();
        SceneManager.LoadScene("ScoreScreen");
        ScoreScreenText.gameOver = true;
        ScoreScreenManager.enterScoreMenu = true;
    }
}
