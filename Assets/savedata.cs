using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Newtonsoft.Json;

[System.Serializable]
public class GameData
{
    public string session_id;       //Find a way to generate a unique ID for this
    public DateTime timestamp;      //
    public int duration_seconds;    //RoundTimer.totalTimeElapsed
    public Dictionary<string, bool> active_operations;      //ButtonBehavior.operators
    public Dictionary<string, int> total_questions;         //ScoreManager.numQuestionsAnswered
    public Dictionary<string, int> correct_answers;         //ScoreManager.numQuestionsCorrect
    public float accuracy;                                 //ScoreScreenText.getAccuracy()
    public List<ProblemData> problems;
    public int time_per_question_average;                   //ScoreScreenText.getAvgTimePerQuestion()
    public Dictionary<string, int> time_per_question_average_by_operation;      //ScoreScreenText.getAvgTimePerQuestionPerOperator()
    public int time_per_question_average_raw;                   //ScoreScreenText.getAvgTimePerQuestion()
    public Dictionary<string, int> time_per_question_average_by_operation_raw;      //ScoreScreenText.getAvgTimePerQuestionPerOperator()
}

[System.Serializable]
public class ProblemData
{
    public string expression;
    public string user_answer;
    public bool correct;
    public int time_taken_milliseconds;
}

public class savedata : MonoBehaviour
{
    private static savedata _instance;

    //GameData object that represents one game
    public GameData gameData;

    public static savedata Instance {
        get {
            if (_instance == null) {
                // If the instance doesn't exist, try to find it in the scene
                _instance = FindObjectOfType<savedata>();
                
                // If it still doesn't exist, create a new instance
                if (_instance == null) {
                    GameObject singletonObject = new GameObject(typeof(savedata).Name);
                    _instance = singletonObject.AddComponent<savedata>();
                }
            }
            return _instance;
        }
    }

    //TODO: savedata.Instance.*method* in other scripts

    private void Start()
    {
        GameData gameData = new GameData
        {
            session_id = "unique_identifier",
            timestamp = DateTime.UtcNow,
            duration_seconds = 15,
            accuracy = 50,
            active_operations = new Dictionary<string, bool>
            {
                {"+", true},
                {"-", true},
                {"*", false},
                {"/", true},
                {"%", false}
            },
            total_questions = new Dictionary<string, int>
            {
                {"+", 1},
                {"-", 1},
                {"*", 0},
                {"/", 1},
                {"%", 0}
            },
            correct_answers = new Dictionary<string, int>
            {
                {"+", 1},
                {"-", 0},
                {"*", 0},
                {"/", 1},
                {"%", 0}
            },
            problems = new List<ProblemData>
            {
                new ProblemData
                {
                    expression = "7 + 2",
                    user_answer = "9",
                    correct = true,
                    time_taken_milliseconds = 1000
                },
                new ProblemData
                {
                    expression = "10 - 2",
                    user_answer = "89",
                    correct = false,
                    time_taken_milliseconds = 5000
                },
                new ProblemData
                {
                    expression = "1 / 2",
                    user_answer = "0.5",
                    correct = true,
                    time_taken_milliseconds = 9000
                }
            },
            time_per_question_average = 7500,
            time_per_question_average_raw = 5000,
            time_per_question_average_by_operation = new Dictionary<string, int>
            {
                { "+", 1000 },
                { "-", 0 },
                { "*", 0 },
                { "/", 9000 },
                { "%", 0 }
            },
            time_per_question_average_by_operation_raw = new Dictionary<string, int>
            {
                { "+", 1000 },
                { "-", 5000 },
                { "*", 0 },
                { "/", 9000 },
                { "%", 0 }
            }
        };

        SaveToJson();
    }
    
    void Awake() {
        DontDestroyOnLoad(this.gameObject);
    }

    //Method: When the game starts, create a new GameData instance.
    //Starts it off with a unique ID and a timestamp.
    //Accessed by GAME_START buttons
    public void gameStart(){
        gameData = new GameData{
            session_id = System.Guid.NewGuid().ToString(),
            timestamp = DateTime.UtcNow,
            active_operations = ButtonBehavior.operators,
            total_questions = new Dictionary<string, int>
            {
                {"+", 0},
                {"-", 0},
                {"*", 0},
                {"/", 0},
                {"%", 0}
            },
            correct_answers = new Dictionary<string, int>
            {
                {"+", 0},
                {"-", 0},
                {"*", 0},
                {"/", 0},
                {"%", 0}
            },
            problems = new List<ProblemData>{}
        };
    }

    //Method: Saves the information of one ProblemData instance into GameData.problems
    //Accessed by the four answer buttons in the gameplay
    //Update problems list, total questions, correct answers
    public void answerProblem(string exp, string op, float userAns, bool userIsCorrect, float time_secs){
        ProblemData currentProblem = new ProblemData
        {
            expression = exp,
            user_answer = userAns.ToString("G"),
            correct = userIsCorrect,
            time_taken_milliseconds = (int)(time_secs*1000)
        };

        //PrintGameData();
        gameData.problems.Add(currentProblem);
        if(userIsCorrect){
            gameData.correct_answers[op] += 1;
        }
        gameData.total_questions[op] += 1;
    }

    //Method: Adds the rest of gameData's information when game ends, then makes it into a json file.
    public void gameEnd(){
        gameData.duration_seconds = (int)getTotalTimeSpent();
        gameData.accuracy = getAccuracy();
        gameData.time_per_question_average = getAvgTimePerProblem(false);
        gameData.time_per_question_average_raw = getAvgTimePerProblem(true);
        gameData.time_per_question_average_by_operation = getAvgTimePerProblemPerOperator(false);
        gameData.time_per_question_average_by_operation_raw = getAvgTimePerProblemPerOperator(true);

        SaveToJson();
    }

    //Helper Method: Obtains the current game's accuracy
    private float getAccuracy(){
        return (ScoreManager.numProblemsAnswered == 0)?0:(((float)ScoreManager.numProblemsCorrect/ScoreManager.numProblemsAnswered) * 100);
    }

    private int getAvgTimePerProblem(bool isRawScore){
        if (isRawScore){
            return (ScoreManager.numProblemsAnswered == 0)?0:(int)((getTotalTimeSpent() / ScoreManager.numProblemsAnswered)*1000);
        }
        return (ScoreManager.numProblemsCorrect == 0)?0:(int)((getTotalTimeSpent() / ScoreManager.numProblemsCorrect)*1000);
    }

    private Dictionary<string, int> getAvgTimePerProblemPerOperator(bool isRawScore){
        Dictionary<string, int> toReturn = new Dictionary<string, int>()
        {
            { "+", 0 },
            { "-", 0 },
            { "*", 0 },
            { "/", 0 },
            { "%", 0 },
        };

        for (int i = 0;i < ButtonBehavior.activeOperators.Count;i++){
            if (isRawScore){
                toReturn[toReturn.ElementAt(i).Key] = (ScoreManager.numProblemsAnsweredPerOperator[getOperatorIndex(ButtonBehavior.activeOperators[i])] == 0)?0:(int)(getTotalTimeSpentForOperator(getOperatorIndex(ButtonBehavior.activeOperators[i])) / ScoreManager.numProblemsAnsweredPerOperator[getOperatorIndex(ButtonBehavior.activeOperators[i])] * 1000);
            }
            toReturn[toReturn.ElementAt(i).Key] = (ScoreManager.numProblemsCorrectPerOperator[getOperatorIndex(ButtonBehavior.activeOperators[i])] == 0)?0:(int)(getTotalTimeSpentForOperator(getOperatorIndex(ButtonBehavior.activeOperators[i])) / ScoreManager.numProblemsCorrectPerOperator[getOperatorIndex(ButtonBehavior.activeOperators[i])] * 1000);
        }

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

    //Method that returns the total time spent for a given operator
    //Returns the amount of time elapsed from RoundTimer, but if it's 0, then return 0.00001 instead. numProblemsCorrect for this operator will be 0, so the result will still be 0 while avoiding NaN error.
    private float getTotalTimeSpentForOperator(int operatorNum){
        return (RoundTimer.amountTimeElapsedPerOperator[operatorNum] == 0)?0.00001f:RoundTimer.amountTimeElapsedPerOperator[operatorNum];
    }

    public void SaveToJson()
    {
        string jsonData = JsonConvert.SerializeObject(gameData, Formatting.Indented);

        string filePath = System.IO.Path.Combine(Application.persistentDataPath, "game_data.json");

        try
        {
            System.IO.File.WriteAllText(filePath, jsonData);
            Debug.Log("Game data saved successfully.");
        }
        catch (Exception e)
        {
            Debug.LogError("Error saving game data: " + e.Message);
        }
    }

    /************************************************************************************
    private void PrintGameData(){
        if(gameData==null){
            Debug.Log("game data is null");
            return;
        }

        Debug.Log("session_id: " + gameData.session_id);
        Debug.Log("timestamp: " + gameData.timestamp);
        Debug.Log("duration_seconds: " + gameData.duration_seconds);
        string lalala = "";
        for (int i = 0;i < gameData.active_operations.Count;i++){
            lalala += gameData.active_operations.ElementAt(i).Key + ": ";
            lalala += gameData.active_operations.ElementAt(i).Value + ".   ";
        }
        Debug.Log("active_operations: " + lalala);
        lalala = "";
        for (int i = 0;i < gameData.total_questions.Count;i++){
            lalala += gameData.total_questions.ElementAt(i).Key + ": ";
            lalala += gameData.total_questions.ElementAt(i).Value + ".   ";
        }
        Debug.Log("total_questions: " + lalala);
        lalala = "";
        for (int i = 0;i < gameData.correct_answers.Count;i++){
            lalala += gameData.correct_answers.ElementAt(i).Key + ": ";
            lalala += gameData.correct_answers.ElementAt(i).Value + ".   ";
        }
        Debug.Log("correct_answers: " + lalala);
        Debug.Log("accuracy: " + gameData.accuracy);
        Debug.Log("Problems Count: " + gameData.problems.Count);
        Debug.Log("time_per_question_average & raw: " + gameData.time_per_question_average + " " + gameData.time_per_question_average_raw);
    }
    */
}
