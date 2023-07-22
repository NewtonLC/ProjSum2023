using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//Script that manages all of the text values that appear on the score screen

public class ScoreScreenText : MonoBehaviour
{
    //Variables that hold whatever
    static public bool gameOver = false;

    //Variables that hold text objects
    public TMP_Text scoreBase;
    public TMP_Text lifeMult;
    public TMP_Text scoreFinal;
    public TMP_Text scoreHigh;
    public TMP_Text person1;
    public TMP_Text person2;
    public TMP_Text person3;
    
    //Holds sprites for the life display
    public GameObject lifeDisplay;
    private SpriteRenderer spriteRenderer;
    public Sprite threeLives;
    public Sprite twoLives;
    public Sprite oneLife;
    public Sprite zeroLives;

    void Start(){
        spriteRenderer = lifeDisplay.GetComponent<SpriteRenderer>();
    }

    void Update(){
        if(gameOver == true){
            displayStats();
            gameOver = false;
        }
    }

    //Display User's score, remaining lives(if applicable), and Life multiplier(if applicable), and User's final score(if applicable)
    void displayStats(){
        scoreBase.text += ScoreManager.playerScore.ToString();
        lifeMult.text += ScoreManager.playerLives.ToString() + "x";
        scoreFinal.text += (ScoreManager.playerScore * ScoreManager.playerLives).ToString();
        displayLives();
        //TODO: Add high score. you'll need to find a way to save the user's progress.
    }

    void displayLives(){
        switch(ScoreManager.playerLives){
            case 0:
                spriteRenderer.sprite = zeroLives;
                break;
            case 1:
                spriteRenderer.sprite = oneLife;
                break;
            case 2:
                spriteRenderer.sprite = twoLives;
                break;
            case 3:
                spriteRenderer.sprite = threeLives;
                break;
        }
    }
    //Display User's final score

    //Display User's total high score

    //Display a leaderboard of 3 people
}
