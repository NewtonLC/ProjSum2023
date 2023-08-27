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
    static public float amountTimeElapsed = 0;
    static public float[] amountTimeElapsedPerOperator = new float[] {0,0,0,0,0};

    //List of the times of each user answer. This will be used in creating the AnswerDots for the line graph at the end.
    static public List<float> AnswerTimes = new List<float>();

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
            markAnswerTime();
            ButtonBehavior.resetProb = true;    //Switch variable to call newProblem() in ButtonBehavior script
            timerState = "Ticking";
        }
        
        //Update the timer and text
        //TODO: Update per operator
        if (string.Equals(timerState, "Ticking")){
            incrementTime();
        }
        updateText();
        //TODO: REMOVE LATER
        timerStateDisplay.text = timerState;
    }

    //Method that increments time by one tick
    private void incrementTime(){
        amountTimeElapsed += Time.deltaTime*5;
        switch(ButtonBehavior.problemOperator){
            case "+":
                amountTimeElapsedPerOperator[0] += Time.deltaTime;
                break;
            case "-":
                amountTimeElapsedPerOperator[1] += Time.deltaTime;
                break;
            case "*":
                amountTimeElapsedPerOperator[2] += Time.deltaTime;
                break;
            case "/":
                amountTimeElapsedPerOperator[3] += Time.deltaTime;
                break;
            case "%":
                amountTimeElapsedPerOperator[4] += Time.deltaTime;
                break;
        }
    }

    private void updateText(){
        RectTransform progDisplayTransform = progDisplay.GetComponent<RectTransform>();
        switch(ScoreManager.gameMode){
            case "problems":
                progDisplay.text = "Problems Remaining: " + ScoreManager.numProblems.ToString();
                progDisplayTransform.localPosition = new Vector3(0, 70, 0);
                break;
            case "time":
                progDisplay.text = "Time Remaining: " + ((int)Mathf.Ceil(ScoreManager.numSeconds-amountTimeElapsed)).ToString();
                progDisplayTransform.localPosition = new Vector3(0, 70, 0);
                break;
            case "zen":
                progDisplay.text = "Questions Solved: " + ScoreManager.numProblemsCorrect + "\nQuestions Attempted: " + ScoreManager.numProblemsAnswered;
                progDisplayTransform.localPosition = new Vector3(0, 70, 0);
                progDisplayTransform.sizeDelta = new Vector2(280, 80);
                break;
            case "survival":
                progDisplay.text = "";
                progDisplayTransform.localPosition = new Vector3(-60, 70, 0);
                break;
            default:
                break;
        }
    }

    //Method: Marks one answer time. If it is correct, it marks the time with a positive number. Otherwise, it marks the time with a negative number.
    private void markAnswerTime(){
        AnswerTimes.Add((ButtonBehavior.userAnswerIsCorrect)?amountTimeElapsed:-amountTimeElapsed);
    }

    //Helper Method: Reveal the answer
    //Spawn an AnswerRing object on the location of the correct button
    private void revealAnswers(){
        float xPos = buttons[ButtonBehavior.correctButton].transform.position.x;
        float yPos = buttons[ButtonBehavior.correctButton].transform.position.y;

        Vector3 SpawnPos = new Vector3(xPos, yPos, 0);
        GameObject answer = Instantiate(AnswerRing, SpawnPos, Quaternion.identity);
    }
}
