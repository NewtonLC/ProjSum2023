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
    //Variables that hold objects?
    public TMP_Text[] gameModes;
    public TMP_Text[] difficulties;
    public TMP_Text[] operators;

    //Variable that checks if the user is at a menu
    static public bool enterMenu = true;

    void Update(){
        if (enterMenu){
            loadGameModes();
            loadDifficulties();
            loadOperators();
            enterMenu = false;
        }
    }

    //Method that returns to main menu
        //Switches to another scene that just reads "MAIN MENU" and that's it...
    public void toMainMenu(){
        Debug.Log("Return to main menu");
        SceneManager.LoadScene("MainMenuScreen");
    }

    //Method that restarts the game, passing in 3 arguments for the 3 different settings
    public void gameStart(){
        if(ButtonBehavior.operators.Count == 0){
            Debug.Log("Select an operator first!");
            return;
        }

        SceneManager.LoadScene("GameplayScreen");
        RoundTimer.timerState = "Ticking";
        ScoreManager.playerScore = 0;
        ScoreManager.playerLives = 3;

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

        //TODO: Alter button state; make bolded if not bolded, remove bold from other buttons, etc.
    }

    //Method that changes the difficulty(and alters button states)
    public void changeDifficulty(string diff){
        ScoreManager.difficulty = diff;
        Debug.Log(ScoreManager.difficulty);

        //TODO: Alter button state; make bolded if not bolded, remove bold from other buttons, etc.
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

        //TODO: Alter button state; make bolded if not bolded, remove bold if bolded.
    }

    //TEMP HELPER METHOD: RETURN OPERATORS
    private string returnOps(){
        string opers = "";
        foreach(string op in ButtonBehavior.operators){
            opers += op;
        }
        return opers;
    }

    //Alters button: Sets the argument button to bold text, changes any other bold texts to regular
    public void difficultySelect(TMP_Text buttText){
        foreach(TMP_Text txt in difficulties){
            if ((txt.fontStyle & FontStyles.Bold) != 0)
                txt.fontStyle ^= FontStyles.Bold;
                txt.fontSize = 14;
        }

        buttText.fontStyle = FontStyles.Bold;
        buttText.fontSize = 16;
    }

    //Alters button: FOR SOME REASON YOU CAN'T PASS MULTIPLE ARGUMENTS ON BUTTON CLICK IN INSPECTOR SO JUST HAVE TWO METHODS, ONE FOR MODE, ONE FOR DIFFICULTY
    public void modeSelect(TMP_Text buttText){
        foreach(TMP_Text txt in gameModes){
            if ((txt.fontStyle & FontStyles.Bold) != 0)
                txt.fontStyle ^= FontStyles.Bold;
                txt.fontSize = 14;
        }

        buttText.fontStyle = FontStyles.Bold;
        buttText.fontSize = 16;
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
                modeSelect(gameModes[0]);
                break;
            case "problems":
                modeSelect(gameModes[1]);
                break;
            case "zen":
                modeSelect(gameModes[2]);
                break;
            case "vs":
                modeSelect(gameModes[3]);
                break;
            default:
                break;
        }
    }

    //Method that loads difficulty selector buttons
    private void loadDifficulties(){
        switch(ScoreManager.difficulty){
            case "easy":
                difficultySelect(difficulties[0]);
                break;
            case "medium":
                difficultySelect(difficulties[1]);
                break;
            case "hard":
                difficultySelect(difficulties[2]);
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
}
