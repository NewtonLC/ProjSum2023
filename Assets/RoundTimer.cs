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
    private float currTime;
    private int TIME_PER_ROUND;
    static public int roundScore;

    //Array that holds the 4 answer buttons - Disable them when timer is paused
    public Button[] buttons;

    //On round end, the correct answer becomes rightColor and the wrong answers become wrongColor.
    //On round start, all buttons return to the default color(probably white?)
    public Color defaultColor;
    public Color wrongColor;
    public Color rightColor;
    
    //Holds the state of the round timer. Meant to be altered by other scripts.
    // "Ticking" --> Timer going down
    // "Paused"  --> Timer stopped
    // "Reset"   --> Reset timer, then switch to "Ticking"
    static public string timerState;

    // Start is called before the first frame update
    void Start()
    {
        TIME_PER_ROUND = 15;
    }

    // Update is called once per frame
    void Update()
    {
        if (currTime > TIME_PER_ROUND && string.Equals(timerState, "Ticking")){ //If Round timer is up
            //Disable all answer buttons, and show the right answer as green and wrong answers as red.
            disableButtons();
            timerState = "Paused";
        }

        if (string.Equals(timerState, "Reset")){
            currTime = 0;
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

    //Method: Disables all buttons so they cannot be pressed.
    //Activates when any answer button is pressed or time runs out.
    public void disableButtons(){
        foreach (Button button in buttons){
            button.interactable = false;
        }
        //Reveal button answers
        revealAnswers();
    }

    //Method: Enables all buttons to allow them to be pressed.
    //Activates when New Problem button is pressed.
    public void enableButtons(){
        resetAnswers();
        foreach (Button button in buttons){
            button.interactable = true;
        }
    }

    //Helper Method: Reveals all answers
    private void revealAnswers(){
        foreach (Button button in buttons){
            if (button == buttons[ButtonBehavior.corrButton]){
                button.GetComponent<Image>().color = rightColor;
            }
            else{
                button.GetComponent<Image>().color = wrongColor;
            }
        }
    }

    //Helper Method: Resets all answers
    private void resetAnswers(){
        foreach (Button button in buttons){
            button.GetComponent<Image>().color = defaultColor;
        }
    }
}
