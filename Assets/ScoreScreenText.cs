using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//Script that manages all of the text values that appear on the score screen

public class ScoreScreenText : MonoBehaviour
{
    //Variables that hold important values
    static public bool gameOver = false;
    private const int NUM_OPERATORS = 5;

    //The line graph with dots going through it
    public GameObject TimelineMarker;
    public TMP_Text TimelineMarkerText;
    public GameObject AnswerDot;
    private Image answerDotImageRenderer;
    public Sprite correctDot;
    public Sprite incorrectDot;

    //Variables that hold text objects
    public TMP_Text textNumProblemsAccuracy;
    public TMP_Text textNumProblemsAccuracyPerOperator;     //Create a tuple/list/hash of five integers, for numProbscorrect and numProbsAttempted
    public TMP_Text textNumProblemsPerMinute;           
    public TMP_Text textNumProblemsPerMinutePerOperator;    //NumProblsCorrect for each operator * (60 / (Amount of time))
    public TMP_Text textAvgTimePerProblem;                  //(Total time spent on all problems, which == Amount of time) / ScoreManager.numProblemsCorrect
    public TMP_Text textAvgTimePerProblemPerOperator;       //(Retrieve total time spent on all problems of each operator) / (NumProblsCorrect for each operator)
    public TMP_Text textTotalTimeSpent;                     //(Amount of time)
    public TMP_Text textTotalTimeSpentPerOperator;          //(Retrieve total time spent on all problems of each operator)
    public TMP_Text textPerson1;
    public TMP_Text textPerson2;
    public TMP_Text textPerson3;
    
    //Holds sprites for the life display
    public GameObject lifeDisplay;
    private SpriteRenderer lifeDisplaySpriteRenderer;
    public Sprite threeLives;
    public Sprite twoLives;
    public Sprite oneLife;
    public Sprite zeroLives;

    void Start(){
        lifeDisplaySpriteRenderer = lifeDisplay.GetComponent<SpriteRenderer>();
        answerDotImageRenderer = AnswerDot.GetComponent<Image>();
    }

    void Update(){
        if(gameOver == true){
            displayStats();
            gameOver = false;
        }
    }

    //Display User's score, remaining lives(if applicable), and Life multiplier(if applicable), and User's final score(if applicable)
    void displayStats(){
        textNumProblemsAccuracy.text += "\n" + ScoreManager.numProblemsCorrect + "/" + ScoreManager.numProblemsAnswered + "/" + getAccuracy();
        textNumProblemsPerMinute.text += "\n" + getNumProblemsPerMinute();
        textAvgTimePerProblem.text += "\n" + getAvgTimePerProblem();
        textTotalTimeSpent.text += "\n" + getTotalTimeSpent().ToString("F2") + "s";
        textNumProblemsAccuracyPerOperator.text = getNumProblemsAccuracyPerOperator();
        textNumProblemsPerMinutePerOperator.text = getNumProblemsPerMinutePerOperator();
        textTotalTimeSpentPerOperator.text = getTotalTimeSpentPerOperator();
        textAvgTimePerProblemPerOperator.text = getAvgTimePerProblemPerOperator();

        shrinkText();

        //Debug.Log("Problems answered: " + ScoreManager.numProblemsAnsweredPerOperator[0] + " " + ScoreManager.numProblemsAnsweredPerOperator[1] + " " + ScoreManager.numProblemsAnsweredPerOperator[2] + " " + ScoreManager.numProblemsAnsweredPerOperator[3] + " " + ScoreManager.numProblemsAnsweredPerOperator[4]);
        //Debug.Log("Problems correct: " + ScoreManager.numProblemsCorrectPerOperator[0] + " " + ScoreManager.numProblemsCorrectPerOperator[1] + " " + ScoreManager.numProblemsCorrectPerOperator[2] + " " + ScoreManager.numProblemsCorrectPerOperator[3] + " " + ScoreManager.numProblemsCorrectPerOperator[4]);

        //Debug.Log("Time Spent: " + RoundTimer.amountTimeElapsed + " " + RoundTimer.amountTimeElapsedPerOperator[0] + " " + RoundTimer.amountTimeElapsedPerOperator[1] + " " + RoundTimer.amountTimeElapsedPerOperator[2] + " " + RoundTimer.amountTimeElapsedPerOperator[3] + " " + RoundTimer.amountTimeElapsedPerOperator[4]);

        displayLives();

        markTimeLine();
        loadAnswerDots();
        //TODO: Add high score. you'll need to find a way to save the user's progress.
    }

    void displayLives(){
        switch(ScoreManager.playerLives){
            case 0:
                lifeDisplaySpriteRenderer.sprite = zeroLives;
                break;
            case 1:
                lifeDisplaySpriteRenderer.sprite = oneLife;
                break;
            case 2:
                lifeDisplaySpriteRenderer.sprite = twoLives;
                break;
            case 3:
                lifeDisplaySpriteRenderer.sprite = threeLives;
                break;
        }
    }
    
    //Method: Gets player's accuracy and returns a string.
    private string getAccuracy(){
        return (ScoreManager.numProblemsAnswered == 0)?"0%":(((float)ScoreManager.numProblemsCorrect/ScoreManager.numProblemsAnswered) * 100).ToString("F0") + "%";
    }

    //Method: Gets cumulative number of problems per minute
    private string getNumProblemsPerMinute(){
        return (ScoreManager.numProblemsCorrect * ((float)60 / getTotalTimeSpent())).ToString("F0") + "|" + (ScoreManager.numProblemsAnswered * ((float)60 / getTotalTimeSpent())).ToString("F0");
    }

    //Method: Gets the average amount of time spent on each problem
    private string getAvgTimePerProblem(){
        string toReturn = (ScoreManager.numProblemsCorrect == 0)?"N/A|":(getTotalTimeSpent() / ScoreManager.numProblemsCorrect).ToString("F2") + "|";
        toReturn += (ScoreManager.numProblemsAnswered == 0)?"N/A":(getTotalTimeSpent() / ScoreManager.numProblemsAnswered).ToString("F2") + "s";
        return toReturn;
    }

    //Method: Gets the total number of seconds spent in game
    private float getTotalTimeSpent(){
        switch(ScoreManager.gameMode){
            case "time":
                return ScoreManager.numSeconds;
            default:
                return RoundTimer.amountTimeElapsed;
        }
    }

    //Method that handles the text for all operators' accuracies
    private string getNumProblemsAccuracyPerOperator(){
        if (ButtonBehavior.activeOperators.Count == 1){
            return "";
        }
        string toReturn = "";
        for (int i = 0;i < ButtonBehavior.activeOperators.Count;i++){
            toReturn += ButtonBehavior.activeOperators[i] + ":\t  " + ScoreManager.numProblemsCorrectPerOperator[getOperatorIndex(ButtonBehavior.activeOperators[i])] + "/" + ScoreManager.numProblemsAnsweredPerOperator[getOperatorIndex(ButtonBehavior.activeOperators[i])] + "/" + getAccuracyForOperator(getOperatorIndex(ButtonBehavior.activeOperators[i])) + "\n";
        }
        return toReturn;
    }

    //Method that retrieves the accuracy for a given operator number
    private string getAccuracyForOperator(int operatorNum){
        return (ScoreManager.numProblemsAnsweredPerOperator[operatorNum] == 0)?"0%":(((float)ScoreManager.numProblemsCorrectPerOperator[operatorNum]/ScoreManager.numProblemsAnsweredPerOperator[operatorNum]) * 100).ToString("F0") + "%";
    }

    //Helper Method that returns the index of a given operator
    private int getOperatorIndex(string op){
        switch(op){
            case "+":
                return 0;
            case "-":
                return 1;
            case "*":
                return 2;
            case "/":
                return 3;
            case "%":
                return 4;
        }
        return -1;
    }

    //Method that handles the text for all operators' problems per minute
    private string getNumProblemsPerMinutePerOperator(){
        if (ButtonBehavior.activeOperators.Count == 1){
            return "";
        }
        string toReturn = "";
        for (int i = 0;i < ButtonBehavior.activeOperators.Count;i++){
            //if operator was found in the game
            toReturn += ButtonBehavior.activeOperators[i] + ":\t  " + (ScoreManager.numProblemsCorrectPerOperator[getOperatorIndex(ButtonBehavior.activeOperators[i])] * ((float)60 / getTotalTimeSpentForOperator(getOperatorIndex(ButtonBehavior.activeOperators[i])))).ToString("F0") + "|" + (ScoreManager.numProblemsAnsweredPerOperator[getOperatorIndex(ButtonBehavior.activeOperators[i])] * ((float)60 / getTotalTimeSpentForOperator(getOperatorIndex(ButtonBehavior.activeOperators[i])))).ToString("F0") + "\n";
        }
        return toReturn;
    }

    //Method that handles text for all operators' avg time per problem
    private string getAvgTimePerProblemPerOperator(){
        if (ButtonBehavior.activeOperators.Count == 1){
            return "";
        }
        string toReturn = "";
        for (int i = 0;i < ButtonBehavior.activeOperators.Count;i++){
            toReturn += ButtonBehavior.activeOperators[i] + ":\t";
            toReturn += (ScoreManager.numProblemsCorrectPerOperator[getOperatorIndex(ButtonBehavior.activeOperators[i])] == 0)?"N/A|":(getTotalTimeSpentForOperator(getOperatorIndex(ButtonBehavior.activeOperators[i])) / ScoreManager.numProblemsCorrectPerOperator[getOperatorIndex(ButtonBehavior.activeOperators[i])]).ToString("F2") + "|";
            toReturn += (ScoreManager.numProblemsAnsweredPerOperator[getOperatorIndex(ButtonBehavior.activeOperators[i])] == 0)?"N/A\n":(getTotalTimeSpentForOperator(getOperatorIndex(ButtonBehavior.activeOperators[i])) / ScoreManager.numProblemsAnsweredPerOperator[getOperatorIndex(ButtonBehavior.activeOperators[i])]).ToString("F2") + "s\n";
        }
        return toReturn;
    }

    //Method that returns the total time spent for a given operator
    //Returns the amount of time elapsed from RoundTimer, but if it's 0, then return 0.00001 instead. numProblemsCorrect for this operator will be 0, so the result will still be 0 while avoiding NaN error.
    private float getTotalTimeSpentForOperator(int operatorNum){
        return (RoundTimer.amountTimeElapsedPerOperator[operatorNum] == 0)?0.00001f:RoundTimer.amountTimeElapsedPerOperator[operatorNum];
    }

    //Method that handles the text for all operators' elapsed times
    private string getTotalTimeSpentPerOperator(){
        if (ButtonBehavior.activeOperators.Count == 1){
            return "";
        }
        string toReturn = "";
        for (int i = 0;i < ButtonBehavior.activeOperators.Count;i++){
            toReturn += ButtonBehavior.activeOperators[i] + ":\t    " + getTotalTimeSpentForOperator(getOperatorIndex(ButtonBehavior.activeOperators[i])).ToString("F2") + "\n";
        }
        return toReturn;
    }

    //Method that loads the Answer Dots
    private const int ANSWER_DOT_DISTANCE_FROM_LINE = 20;
    private void loadAnswerDots(){
        float yPos;
        float xPos;
        for (int i = 0;i < RoundTimer.AnswerTimes.Count;i++){
            if (RoundTimer.AnswerTimes[i] > 0){                 //Correct answer
                answerDotImageRenderer.sprite = correctDot;
                yPos = LINE_Y_POSITION + ANSWER_DOT_DISTANCE_FROM_LINE;
            }
            else {                                              //Incorrect answer
                answerDotImageRenderer.sprite = incorrectDot;
                yPos = LINE_Y_POSITION - ANSWER_DOT_DISTANCE_FROM_LINE;
            }
            xPos = findXPos(RoundTimer.AnswerTimes[i]);
            Vector3 SpawnPos = new Vector3(xPos, yPos, 0);
            GameObject dotObject = Instantiate(AnswerDot, SpawnPos, Quaternion.identity);
            //TODO: When AnswerTimes.Length reaches a certain threshold, the dots will decrease in size.
        }
    }

    //Every MARK_TIME seconds, spawn a mark at that point. The mark should have text along with it that tells the time.
    //Then, put a markerTextObject where the start and end are.
    private const float MARK_TIME = 10;
    //Line Y position is -2.231, but for some reason objects spawned 0.097 higher than they were supposed to?
    private const float LINE_Y_POSITION = 120;
    private const float LINE_X_POSITION = 0;
    private const int TEXT_DISTANCE_FROM_LINE = 45;
    private void markTimeLine(){
        for (float i = MARK_TIME;i < getTotalTimeSpent()*0.90f;i += MARK_TIME){     //Change so that it starts at 10 seconds, but if 
            float xPos = findXPos(i);
            Vector3 SpawnPos = new Vector3(xPos, LINE_Y_POSITION, 0);
            GameObject markerObject = Instantiate(TimelineMarker, SpawnPos, Quaternion.identity);
            SpawnPos = new Vector3(xPos, LINE_Y_POSITION-TEXT_DISTANCE_FROM_LINE, 0);
            TMP_Text markerTextObject = Instantiate(TimelineMarkerText, SpawnPos, Quaternion.identity);
            markerTextObject.text = "" + i;
        }
        Vector3 BeginningSpawnPos = new Vector3(findXPos(0), LINE_Y_POSITION-TEXT_DISTANCE_FROM_LINE, 0);
        TMP_Text BeginningMarkerTextObject = Instantiate(TimelineMarkerText, BeginningSpawnPos, Quaternion.identity);
        BeginningMarkerTextObject.text = "0";
        
        Vector3 EndingSpawnPos = new Vector3(findXPos(RoundTimer.amountTimeElapsed), LINE_Y_POSITION-TEXT_DISTANCE_FROM_LINE, 0);
        TMP_Text EndingMarkerTextObject = Instantiate(TimelineMarkerText, EndingSpawnPos, Quaternion.identity);
        EndingMarkerTextObject.text = getTotalTimeSpent().ToString("F2");
        
    }

    //Helper Method that locates the x position of the AnswerDot.
    private const float LINE_LENGTH = 168;
    private float findXPos(float answerTime){
        return ((Mathf.Abs(answerTime)/RoundTimer.amountTimeElapsed) * LINE_LENGTH - (LINE_LENGTH/2)) + LINE_X_POSITION;
    }

    //Method that spaces the bottom text objects depending on how many operators were used.
    private const int TEXT_HEIGHT = 23;
    public List<TMP_Text> textsToShrink = new List<TMP_Text>();
    private void shrinkText(){
        for (int i = 0;i < textsToShrink.Count;i++){
            textsToShrink[i].GetComponent<RectTransform>().sizeDelta -= new Vector2(0,getNumInactiveOperators() * TEXT_HEIGHT);
        }
    }

    //Helper method that returns the number of operators not used
    private int getNumInactiveOperators(){
        return (ButtonBehavior.operators.Count - ButtonBehavior.activeOperators.Count);
    }
}
