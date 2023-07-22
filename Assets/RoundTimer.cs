using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RoundTimer : MonoBehaviour
{
    //Text object that displays the current round time
    public TMP_Text timeDisplay;
    public TMP_Text timerStateDisplay;

    //Variables that hold important numbers
    private float currTime = 0;
    private int TIME_PER_ROUND;
    static public int roundScore;

    //Array that holds the 4 answer buttons - Disable them when timer is paused
    public Button[] buttons;
    
    //Holds the state of the round timer. Meant to be altered by other scripts.
    // "Ticking" --> Timer going down
    // "Reset"   --> Reset timer, then switch to "Ticking"
    static public string timerState;

    //AnswerRing GameObject, for displaying the correct answer
    public GameObject AnswerRing;

    // Start is called before the first frame update
    void Start()
    {
        TIME_PER_ROUND = 15;
    }

    // Update is called once per frame
    void Update()
    {
        if (currTime > TIME_PER_ROUND && string.Equals(timerState, "Ticking")){ //If Round timer is up
            timerState = "Reset";
        }

        if (string.Equals(timerState, "Reset")){
            revealAnswers();
            ButtonBehavior.resetProb = true;    //Switch variable to call newProblem() in ButtonBehavior script
            currTime = 0;                       //Resets the timer.
            timerState = "Ticking";
        }
        
        //Update the timer and text
        if (string.Equals(timerState, "Ticking")){
            currTime += Time.deltaTime;
        }
        roundScore = (int)Mathf.Ceil(TIME_PER_ROUND-currTime);
        timeDisplay.text = "Time Remaining: " + roundScore.ToString();
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
