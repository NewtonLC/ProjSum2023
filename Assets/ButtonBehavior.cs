using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//Manages the button behaviors for answering questions

public class ButtonBehavior : MonoBehaviour{
    //Text and button objects
    public TMP_Text textProb;
    public TMP_Text[] buttonTexts;

    //List of operators
    //static public List<string> operators = new List<string>();


    static public Dictionary<string, bool> operators = new Dictionary<string, bool>(){
	    {"+", false},
        {"-", false},
        {"*", false},
        {"/", false},
        {"%", false},
    };

    static public List<string> activeOperators = operators.Where(kv => kv.Value).Select(kv => kv.Key).ToList();

    //Switch variable to determine which button has the right answer
    static public int correctButton = 0;
    static public bool userAnswerIsCorrect = false;                 //Used in RoundTimer for marking user answers.
    static public string problemOperator;
    public string[] answers = new string[] {"A","B","C","D"};
    public Button[] buttons;

    //Variables to determine the range of numbers that get displayed
    private const int EASY_BASE_NUM = 5;
    private const int MEDIUM_BASE_NUM = 9;
    private const int HARD_BASE_NUM = 13;
    private const int NUM_RANGE = 3;        //Ex: For Base of 6, Range of 4, numbers can be anywhere from 2 - 10.

    //Switch variable used to call newProblem
    static public bool resetProb;

    // Start is called before the first frame update
    void Start(){
        newProblem();
    }

    void Update(){
        if(resetProb){
            newProblem();
        }
    }

    public void userAnswer(string buttonID){
        
        if (string.Equals(buttonID, answers[correctButton])){
            Debug.Log("Right Answer");
            userAnswerIsCorrect = true;
            ScoreManager.numProblemsCorrect++; 
            incrementOperator(ScoreManager.numProblemsCorrectPerOperator);
        }
        else {
            Debug.Log("Wrong Answer");
            userAnswerIsCorrect = false;
            ScoreManager.playerLives--;
        }

        //Reduce the number of problems by 1 if the gameMode is set to "problems"
        ScoreManager.numProblems--;
        ScoreManager.numProblemsAnswered++;
        incrementOperator(ScoreManager.numProblemsAnsweredPerOperator);

        //Pause the round timer.
        RoundTimer.timerState = "Reset";
    }

    //Method: Create a new mathematical problem and update the buttons.
    public void newProblem(){
        int int1 = randomNum(ScoreManager.difficulty);  //Currently, difficulty only changes the number range
        int int2 = randomNum(ScoreManager.difficulty);
        //problemOperator = operators[Random.Range(0,operators.Count)].Key;

        //TESTING

        // Check if there are active operators
        if (activeOperators.Count > 0)
        {
            // Step 2: Generate a random index within the range of active operators
            int randomIndex = Random.Range(0, activeOperators.Count);

            // Step 3: Retrieve the randomly selected active operator
            problemOperator = activeOperators[randomIndex];

            Debug.Log("Randomly selected active operator: " + problemOperator);
        }
        else
        {
            Debug.Log("No active operators in the game.");
        }

        //TESTING

        float ans = solveProb(int1, int2, problemOperator);
        Debug.Log(ans);

        //parentheses for negatives
        textProb.text = int1<0?"(" + int1 + ")":int1.ToString();
        textProb.text += " " + problemOperator;
        textProb.text += int2<0?" (" + int2 + ")":" " +int2.ToString();

        
        //Update each of the 4 buttons randomly.
        correctButton = Random.Range(0,buttonTexts.Length);
        float[] takenAnswers = new float[buttonTexts.Length];
        /*
        for (int i = 0; i < buttonTexts.Length; i++){
            if (i == correctButton)
            {
                takenAnswers[i] = ans;
                buttonTexts[i].text = string.Equals(problemOperator, "/") ? ans.ToString("F3") : ans.ToString();
            }
            else
            {
                takenAnswers[i] = findWrongAns(ans, takenAnswers);
                buttonTexts[i].text = string.Equals(problemOperator, "/") ? takenAnswers[i].ToString("F3") : takenAnswers[i].ToString();
            }
        }
        */
        for (int i = 0; i < buttonTexts.Length; i++){
            if (i == correctButton){
                takenAnswers[i] = ans;
                buttonTexts[i].text = ans.ToString("G");
            }
            else{
                takenAnswers[i] = findWrongAns(ans, takenAnswers);
                buttonTexts[i].text = takenAnswers[i].ToString("G");
            }
        }

        resetProb = false;
    }

    //Helper method: Select an integer for newproblem.
    private int randomNum(string difficulty){
        //Call probDifficulty with difficulty to get the base int.
        //Randomly choose an int within the range of the base.
        int num = Random.Range(probDifficulty(difficulty)-NUM_RANGE, probDifficulty(difficulty)+NUM_RANGE+1);

        //Possibility of changing num to negative, based on difficulty level
        switch(difficulty){
            case "medium":
                if (Random.Range(0,8) >= 1)     // 1 in 8 chance of either int being negative in medium
                    num = -num;
                break; 
            case "hard":
                if (Random.Range(0,4) >= 1)     // 1 in 4 chance of either int being negative in hard
                    num = -num;
                break;
            default:
                break;
        }

        return num;
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
    private float solveProb(int int1, int int2, string problemOperator){
        switch(problemOperator){
            case "+":
                return int1+int2;
            case "-":
                return int1-int2;
            case "*":
                return int1*int2;
            case "/":
                return (float)int1/int2;
            case "%":
                return int1%int2;
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

    //Helper Method: Increment one of the operator stats
    //Takes in the operator list that it wants to implement.
    private void incrementOperator(int[] operatorStatsList){
        switch(problemOperator){
            case "+":
                operatorStatsList[0]++;
                break;
            case "-":
                operatorStatsList[1]++;
                break;
            case "*":
                operatorStatsList[2]++;
                break;
            case "/":
                operatorStatsList[3]++;
                break;
            case "%":
                operatorStatsList[4]++;
                break;
        }
    }
}
