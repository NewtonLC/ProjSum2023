using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RoundTimer : MonoBehaviour
{
    //Text object that displays the current round time
    public TMP_Text progDisplay;
    public TMP_Text timerStateDisplay;

    //Variables that hold important numbers
    private float TIME_PER_ROUND = 15;
    private float currTime = 0;
    static public int roundScore;

    //Array that holds the 4 answer buttons - Disable them when timer is paused
    public Button[] buttons;
    
    //Holds the state of the round timer. Meant to be altered by other scripts.
    // "Ticking" --> Timer going down
    // "Reset"   --> Reset timer, then switch to "Ticking"
    static public string timerState;

    //AnswerRing GameObject, for displaying the correct answer
    public GameObject AnswerRing;

    void Start(){
    
    }

    // Update is called once per frame
    void Update()
    {
        if (string.Equals(timerState, "Reset")){
            revealAnswers();
            ButtonBehavior.resetProb = true;    //Switch variable to call newProblem() in ButtonBehavior script
            currTime = 0;                       //Resets the timer.
            timerState = "Ticking";
        }
        
        //Update the timer and text
        if (string.Equals(timerState, "Ticking")){
            currTime += Time.deltaTime;
            ScoreManager.numSeconds -= Time.deltaTime;
        }
        roundScore = Mathf.Max((int)Mathf.Ceil(TIME_PER_ROUND-currTime), 0);
        if (string.Equals(ScoreManager.gameMode, "problems")){
            progDisplay.text = "Problems Remaining: " + ScoreManager.numProblems.ToString();
        }
        else if (string.Equals(ScoreManager.gameMode, "time")){
            progDisplay.text = "Time Remaining: " + ((int)Mathf.Ceil(ScoreManager.numSeconds)).ToString();
        }
        //TODO: REMOVE LATER
        timerStateDisplay.text = timerState;
    }

    //Helper Method: Reveal the answer
    //Spawn an AnswerRing object on the location of the correct button
    private void revealAnswers(){
        float xPos = buttons[ButtonBehavior.corrButton].transform.position.x;
        float yPos = buttons[ButtonBehavior.corrButton].transform.position.y;

        Vector3 SpawnPos = new Vector3(xPos, yPos, 0);
        GameObject answer = Instantiate(AnswerRing, SpawnPos, Quaternion.identity);
    }
}
