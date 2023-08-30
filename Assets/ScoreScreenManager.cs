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
    public TMP_Text[] TMP_gameModes;
    public TMP_Text[] TMP_difficulties;
    public TMP_Text[] TMP_operators;
    public TMP_Text[] TMP_amounts;
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
        if(!ButtonBehavior.operators.Any(kv => kv.Value)){
            Debug.Log("Select an operator first!");
            return;
        }

        //Load activeOperators with the true values of operators
        loadActiveOperators();

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
        ButtonBehavior.operators[op] = !ButtonBehavior.operators[op];
        loadActiveOperators();
        Debug.Log(returnOps());
    }

    //TEMP HELPER METHOD: RETURN OPERATORS
    private string returnOps(){
        string opers = "";
        foreach(string op in ButtonBehavior.activeOperators){
            opers += op;
        }
        return opers;
    }

    //Method: Calls the button select based on given TMP object
    public void findSelect(TMP_Text txt){
        if (containsText(txt, TMP_gameModes)){
            boldSelect(txt, TMP_gameModes);
        }
        if (containsText(txt, TMP_difficulties)){
            boldSelect(txt, TMP_difficulties);
        }
        if (containsText(txt, TMP_amounts)){
            boldSelect(txt, TMP_amounts);
        }
    }

    //Helper Method: Loads activeOperators in ButtonBehavior
    //By clearing activeOperators before reloading it in order, this guarantees that activeOperators is always in order.
    private void loadActiveOperators(){
        ButtonBehavior.activeOperators.Clear();
        for(int i = 0;i < ButtonBehavior.operators.Count;i++){
            if (ButtonBehavior.operators.ElementAt(i).Value){
                ButtonBehavior.activeOperators.Add(ButtonBehavior.operators.ElementAt(i).Key);
            }
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
                boldSelect(TMP_gameModes[0], TMP_gameModes);
                break;
            case "problems":
                boldSelect(TMP_gameModes[1], TMP_gameModes);
                break;
            case "zen":
                boldSelect(TMP_gameModes[2], TMP_gameModes);
                break;
            case "vs":
                boldSelect(TMP_gameModes[3], TMP_gameModes);
                break;
            default:
                break;
        }
    }

    //Method that loads difficulty selector buttons
    private void loadDifficulties(){
        switch(ScoreManager.difficulty){
            case "easy":
                boldSelect(TMP_difficulties[0], TMP_difficulties);
                break;
            case "medium":
                boldSelect(TMP_difficulties[1], TMP_difficulties);
                break;
            case "hard":
                boldSelect(TMP_difficulties[2], TMP_difficulties);
                break;
            default:
                break;
        }
    }

    //Method that loads operator selector buttons
    private void loadOperators(){
        foreach(string op in ButtonBehavior.activeOperators){
            switch(op){
                case "+":
                    buttonBold(TMP_operators[0]);
                    break;
                case "-":
                    buttonBold(TMP_operators[1]);
                    break;
                case "*":
                    buttonBold(TMP_operators[2]);
                    break;
                case "/":
                    buttonBold(TMP_operators[3]);
                    break;
                case "%":
                    buttonBold(TMP_operators[4]);
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
                boldSelect(TMP_amounts[AmountSelector.amount], TMP_amounts);
                break;
            case "problems":
                changeAmounts(AmountSelector.AMOUNT_PROBLEMS);
                boldSelect(TMP_amounts[AmountSelector.amount], TMP_amounts);
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
            TMP_amounts[i].text = amoList[i].ToString();
            Debug.Log(amoList[i].ToString());
        }
    }
}
