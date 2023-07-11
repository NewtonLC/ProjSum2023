using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RoundTimer : MonoBehaviour
{
    //Text object that displays the current round time
    public TMP_Text timeDisplay;

    //Variables that hold important numbers
    private float currTime;
    private int TIME_PER_ROUND;
    
    //Holds the state of the round timer. Meant to be altered by other scripts.
    // "Ticking" --> Timer going down
    // "Paused"  --> Timer stopped
    // "Reset"   --> Reset timer, then switch to "Ticking"
    static public string timerState;

    // Start is called before the first frame update
    void Start()
    {
        TIME_PER_ROUND = 10;
    }

    // Update is called once per frame
    void Update()
    {
        if (currTime < TIME_PER_ROUND && string.Equals(timerState, "Ticking")){ //If Round timer is up
            //Disable all answer buttons, and show the right answer as green and wrong answers as red.
            timerState = "Paused";
            currTime = 0;
        }



        //RoundTimer...
        //STARTS AT 10 SECONDS
        //Stops when an answer is pressed.
        //Resets when New Problem button is clicked.
        //If it hits 0 on its own...
            //Reset
        
        //Update the timer and text
        if (string.Equals(timerState, "Ticking")){
            currTime += Time.deltaTime;
        }
        timeDisplay.text = "Time Remaining: " + Mathf.Ceil(TIME_PER_ROUND-currTime).ToString();
    }
}
