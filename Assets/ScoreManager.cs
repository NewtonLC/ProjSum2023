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
    static public string gameMode;
    static public List<string> operators = new List<string>();

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
