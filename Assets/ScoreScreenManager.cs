using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

//Script that manages all of the interactable buttons on the score screen

public class ScoreScreenManager : MonoBehaviour
{
    //Number of operators
    private const int NUM_OPERATORS = 5;

    //Variables that hold objects?
    public TMP_Text[] gameModes;
    public TMP_Text[] difficulties;
    public TMP_Text[] operators;
    public TMP_Text[] amounts;
    public Button[] amountButtons;

    //Variable that checks if the user is at a menu
    static public bool enterScoreMenu = false;
    static public bool enterMainMenu = true;

    void Update(){
        if ((enterMainMenu && string.Equals(SceneManager.GetActiveScene().name, "MainMenuScreen")) || (enterScoreMenu && string.Equals(SceneManager.GetActiveScene().name, "ScoreScreen"))){
            Debug.Log(SceneManager.GetActiveScene().name);
            loadGameModes();
            loadDifficulties();
            loadOperators();
            loadAmounts();        //TODO: MAKE THIS AN ACTUAL METHOD
            enterMainMenu = false;
            enterScoreMenu = false;
        }
    }

    //Method that returns to main menu
    public void toMainMenu(){
        SceneManager.LoadScene("MainMenuScreen");
        Debug.Log(ScoreManager.gameMode);
        enterMainMenu = true;
    }

    //Method that restarts the game, passing in 3 arguments for the 3 different settings
    public void gameStart(){
        if(ButtonBehavior.operators.Count == 0){
            Debug.Log("Select an operator first!");
            return;
        }

        SceneManager.LoadScene("GameplayScreen");
        RoundTimer.timerState = "Ticking";
        ScoreManager.numSeconds = AmountSelector.AMOUNT_TIMES[AmountSelector.amount];
        ScoreManager.numProblems = AmountSelector.AMOUNT_PROBLEMS[AmountSelector.amount];
        ScoreManager.playerLives = 3;
        ScoreManager.numProblemsAnswered = 0;
        ScoreManager.numProblemsCorrect = 0;
        RoundTimer.amountTimeElapsed = 0;
        RoundTimer.AnswerTimes.Clear();

        for (int i = 0;i < NUM_OPERATORS;i++){
            ScoreManager.numProblemsAnsweredPerOperator[i] = 0;
            ScoreManager.numProblemsCorrectPerOperator[i] = 0;
            RoundTimer.amountTimeElapsedPerOperator[i] = 0;
        }

        string opers = returnOps();
        Debug.Log("RESTART: DIFFICULTY, " + ScoreManager.difficulty + ". MODE, " + ScoreManager.gameMode + ". OPERATORS, " + opers);
    }

    //Method that shares with friends/posts on facebook
    public void toShare(){
        Debug.Log("SHARED WITH FRIENDSSSSS ON FACEBOOEOKKKKK");
    }

    //Method that changes the mode(and alters button states)
    public void changeMode(string gMode){
        ScoreManager.gameMode = gMode;
        Debug.Log(ScoreManager.gameMode);  
    }

    //Method that changes the difficulty(and alters button states)
    public void changeDifficulty(string diff){
        ScoreManager.difficulty = diff;
        Debug.Log(ScoreManager.difficulty); 
    }

    //Method that changes the operators(and alters button states)
    public void changeOperators(string op){
        //if operators contains op, remove it. Otherwise, add it.
        if (ButtonBehavior.operators.Contains(op)){
            ButtonBehavior.operators.Remove(op);
        }
        else{
            ButtonBehavior.operators.Add(op);
        }
        Debug.Log(returnOps());
    }

    //TEMP HELPER METHOD: RETURN OPERATORS
    private string returnOps(){
        string opers = "";
        foreach(string op in ButtonBehavior.operators){
            opers += op;
        }
        return opers;
    }

    //Method: Calls the button select based on given TMP object
    public void findSelect(TMP_Text txt){
        if (containsText(txt, gameModes)){
            boldSelect(txt, gameModes);
        }
        if (containsText(txt, difficulties)){
            boldSelect(txt, difficulties);
        }
        if (containsText(txt, amounts)){
            boldSelect(txt, amounts);
        }
    }

    //Helper Method: Sets the argument button to bold text, changes any other bold texts to regular
    private void boldSelect(TMP_Text boldTxt, TMP_Text[] txt_list){
        foreach(TMP_Text txt in txt_list){
            if ((txt.fontStyle & FontStyles.Bold) != 0)
                txt.fontStyle ^= FontStyles.Bold;
                txt.fontSize = 14;
        }

        boldTxt.fontStyle = FontStyles.Bold;
        boldTxt.fontSize = 16;
    }

    //Helper Method: Checks if a buttonText is in a given array
    private bool containsText(TMP_Text returnTxt, TMP_Text[] txt_list){
        foreach(TMP_Text txt in txt_list){
            if (returnTxt.GetInstanceID() == txt.GetInstanceID()){
                return true;
            }
        }
        return false;
    }

    //Alters button: Sets argument button to bold text if regular, set to regular if bold.
    //Only to be used on operators selection
    public void buttonBold(TMP_Text buttText){
        buttText.fontStyle ^= FontStyles.Bold;
        if ((buttText.fontStyle & FontStyles.Bold) != 0){          //Currently ON
            buttText.fontSize += 8;
        }
        else{                                                      //Currently OFF
            buttText.fontSize -= 8;
        }
    }

    //Method that loads gameMode selector buttons
    private void loadGameModes(){
        switch(ScoreManager.gameMode){
            case "time":
                boldSelect(gameModes[0], gameModes);
                break;
            case "problems":
                boldSelect(gameModes[1], gameModes);
                break;
            case "zen":
                boldSelect(gameModes[2], gameModes);
                break;
            case "vs":
                boldSelect(gameModes[3], gameModes);
                break;
            default:
                break;
        }
    }

    //Method that loads difficulty selector buttons
    private void loadDifficulties(){
        switch(ScoreManager.difficulty){
            case "easy":
                boldSelect(difficulties[0], difficulties);
                break;
            case "medium":
                boldSelect(difficulties[1], difficulties);
                break;
            case "hard":
                boldSelect(difficulties[2], difficulties);
                break;
            default:
                break;
        }
    }

    //Method that loads operator selector buttons
    private void loadOperators(){
        foreach(string op in ButtonBehavior.operators){
            switch(op){
                case "+":
                    buttonBold(operators[0]);
                    break;
                case "-":
                    buttonBold(operators[1]);
                    break;
                case "*":
                    buttonBold(operators[2]);
                    break;
                case "/":
                    buttonBold(operators[3]);
                    break;
                case "%":
                    buttonBold(operators[4]);
                    break;
                default:
                    break;
            }
        }
    }

    //Method that loads amount selector buttons
    public void loadAmounts(){
        switch(ScoreManager.gameMode){
            case "time":
                changeAmounts(AmountSelector.AMOUNT_TIMES);
                boldSelect(amounts[AmountSelector.amount], amounts);
                break;
            case "problems":
                changeAmounts(AmountSelector.AMOUNT_PROBLEMS);
                boldSelect(amounts[AmountSelector.amount], amounts);
                break;
            default:
                hideAmounts();
                break;
        }
    }

    //Helper Method that hides the amount selectors
    private void hideAmounts(){
        foreach(Button amount in amountButtons){
            amount.gameObject.SetActive(false);
        }
    }

    //Helper Method that changes the amount selectors' text
    private void changeAmounts(int[] amoList){
        for (int i = 0;i < amountButtons.Length;i++){
            amountButtons[i].gameObject.SetActive(true);
            amounts[i].text = amoList[i].ToString();
            Debug.Log(amoList[i].ToString());
        }
    }
}
