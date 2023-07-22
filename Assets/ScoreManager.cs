using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    //Static variables to hold the player's stats and information
    static public int playerScore;
    static public int playerLives = 3;
    static public string difficulty = "easy";
    static public string gameMode = "time";
    static public List<string> operators = new List<string>();

    //Variables to handle game-end scenarios(besides playerLives == 0)
    static public int numProblems = 15;
    static public float numSeconds = 30;

    //Text to display player's score and lives
    public TMP_Text scoreDisplay;
    public TMP_Text livesDisplay;

    // Update is called once per frame
    void Update()
    {
        scoreDisplay.text = "Score: " + playerScore.ToString();
        livesDisplay.text = "Lives: " + playerLives.ToString();

        if (playerLives == 0 && !ScoreScreenText.gameOver){
            //Player loses the game
            Debug.Log("YOU LOSE");

            //Game end, with stats.
            gameEnd();
        }

        if (string.Equals(gameMode, "problems") && numProblems == 0){
            Debug.Log("You answered " + (numProblems) + " problems!");

            gameEnd();
        }

        if (string.Equals(gameMode, "time") && numSeconds < 0){
            Debug.Log(((int)numSeconds).ToString() + " seconds have passed!");

            gameEnd();
        }
    }

    //Method to process game start.
    void gameStart(){
        
    }

    // Method to process game end.
    // Called when...
        // Player time runs out
        // Player loses all lives
        // Player answers all necessary questions
    void gameEnd(){
        //Load the Score Screen scene
        SceneManager.LoadScene("ScoreScreen");
        ScoreScreenText.gameOver = true;
    }
}
