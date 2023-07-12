using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

public class ButtonBehavior : MonoBehaviour
{
    //Text and button objects
    public TMP_Text textProb;
    public TMP_Text[] buttonTexts;

    //List of operators
    static public List<string> operators = new List<string>();

    //Switch variable to determine which button has the right answer
    private int corrButton = 0;
    public string[] answers = new string[] {"A","B","C","D"};

    //Variables to determine the range of numbers that get displayed
    private const int EASY_BASE_NUM = 5;
    private const int MEDIUM_BASE_NUM = 9;
    private const int HARD_BASE_NUM = 13;
    private const int NUM_RANGE = 3;        //Ex: For Base of 6, Range of 4, numbers can be anywhere from 2 - 10.

    // Start is called before the first frame update
    void Start()
    {
        operators.Add("+");
        //operators.Add("-");
        //operators.Add("*");
        //operators.Add("/");
        newProblem();
    }

    public void userAnswer(string buttonID){
        if (string.Equals(buttonID, answers[corrButton])){
            Debug.Log("Right Answer");
            ScoreManager.playerScore += RoundTimer.roundScore;
        }
        else {
            Debug.Log("Wrong Answer");
        }

        //Pause the round timer.
        RoundTimer.timerState = "Paused";
    }

    //Method: Create a new mathematical problem and update the buttons.
    public void newProblem(){
        if (operators.Count == 0){      //If no operators are selected, clear the field.
            clearField();
            return;
        }

        int int1 = Random.Range(probDifficulty(MenuSelector.difficulty)-NUM_RANGE,probDifficulty(MenuSelector.difficulty)+NUM_RANGE+1);  //Currently, difficulty only changes the number range
        int int2 = Random.Range(probDifficulty(MenuSelector.difficulty)-NUM_RANGE,probDifficulty(MenuSelector.difficulty)+NUM_RANGE+1);
        string probOperator = operators[Random.Range(0,operators.Count)];
        float ans = solveProb(int1, int2, probOperator);
        //Put two integers and an operator between them in the TEXT.
        textProb.text = int1 + " " + probOperator + " " + int2;

        //Update each of the 4 buttons randomly.
        corrButton = Random.Range(0,buttonTexts.Length);
        float[] takenAnswers = new float[buttonTexts.Length];
        for(int i = 0;i < buttonTexts.Length;i++){
            if (i == corrButton){
                takenAnswers[i] = ans;
                buttonTexts[i].text = "" + ans;
            }
            else {
                takenAnswers[i] = findWrongAns(ans, takenAnswers);
                buttonTexts[i].text = "" + takenAnswers[i];
            }
        }

        RoundTimer.timerState = "Reset";
    }

    //Helper method: Decides number based on difficulty setting
    private int probDifficulty(string difficulty){
        switch(difficulty){
            case "easy":
                return EASY_BASE_NUM;
            case "medium":
                return MEDIUM_BASE_NUM;
            case "hard":
                return HARD_BASE_NUM;
        }
        return 0;
    }

    //Helper method: Clears info off the field
    private void clearField(){
        textProb.text = "No operator selected!";
        foreach (TMP_Text button in buttonTexts){
            button.text = "";
        }
    }

    //Helper Method: Round the number to three digits
    private float roundNum(float num){
        return Mathf.Round(num * 1000.0f) * 0.001f;
    }

    //Helper Method: Solve the problem.
    private float solveProb(int int1, int int2, string probOperator){
        switch(probOperator){
            case "+":
                return int1+int2;
            case "-":
                return int1-int2;
            case "*":
                return int1*int2;
            case "/":
                return (float)int1/int2;
        }
        return 0;
    }

    //Helper Method: Find a wrong answer based on the right answer that isn't already on one of the buttons.
    private float findWrongAns(float ans, float[] takenAnswers){
        float wrongAns = randomWrong(ans);
        int breakingPoint = 0;
        while (takenAnswers.Contains(wrongAns)){
            breakingPoint++;
            wrongAns = randomWrong(ans);
            if(breakingPoint > 100){
                //Debug.Log("BREAKING OUT OF WHILE LOOP");
                break;
            }
        }
        return wrongAns;
    }

    //Helper Helper Method: Randomize Wrong answer
    private float randomWrong(float ans){
        if (Random.Range(0,2) == 0){
            return (ans + Random.Range(1,Mathf.Max(4,(int)ans/2)));
        }
        return (ans + Random.Range(Mathf.Min(-3,-(int)ans/2),-1));
    }
}
