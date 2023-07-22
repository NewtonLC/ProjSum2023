using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//Script that manages all of the interactable buttons on the score screen

public class ScoreScreenManager : MonoBehaviour
{
    //Variables that hold whatever values you want
    private string difficulty;
    private string gameMode;
    private List<string> operators = new List<string>();

    //Variables that hold objects?
    public TMP_Text[] gameModes;
    public TMP_Text[] difficulties;

    //Method that returns to main menu
        //Switches to another scene that just reads "MAIN MENU" and that's it...
    public void toMainMenu(){
        Debug.Log("Return to main menu");
    }

    //Method that restarts the game, passing in 3 arguments for the 3 different settings
    public void toRestart(){
        string opers = returnOps();
        Debug.Log("RESTART: DIFFICULTY, " + difficulty + ". MODE, " + gameMode + ". OPERATORS, " + opers);
    }

    //Method that shares with friends/posts on facebook
    public void toShare(){
        Debug.Log("SHARED WITH FRIENDSSSSS ON FACEBOOEOKKKKK");
    }

    //Method that changes the mode(and alters button states)
    public void changeMode(string gMode){
        gameMode = gMode;
        Debug.Log(gameMode);

        //TODO: Alter button state; make bolded if not bolded, remove bold from other buttons, etc.
    }

    //Method that changes the difficulty(and alters button states)
    public void changeDifficulty(string diff){
        difficulty = diff;
        Debug.Log(difficulty);

        //TODO: Alter button state; make bolded if not bolded, remove bold from other buttons, etc.
    }

    //Method that changes the operators(and alters button states)
    public void changeOperators(string op){
        //if operators contains op, remove it. Otherwise, add it.
        if (operators.Contains(op)){
            operators.Remove(op);
        }
        else{
            operators.Add(op);
        }
        Debug.Log(returnOps());

        //TODO: Alter button state; make bolded if not bolded, remove bold if bolded.
    }

    //TEMP HELPER METHOD: RETURN OPERATORS
    private string returnOps(){
        string opers = "";
        foreach(string op in operators){
            opers += op;
        }
        return opers;
    }

    //Alters button: Sets the argument button to bold text, changes any other bold texts to regular
    public void difficultySelect(TMP_Text buttText){
        foreach(TMP_Text txt in difficulties){
            if ((txt.fontStyle & FontStyles.Bold) != 0)
                txt.fontStyle ^= FontStyles.Bold;
        }

        buttText.fontStyle = FontStyles.Bold;
    }

    //Alters button: FOR SOME REASON YOU CAN'T PASS MULTIPLE ARGUMENTS ON BUTTON CLICK IN INSPECTOR SO JUST HAVE TWO METHODS, ONE FOR MODE, ONE FOR DIFFICULTY
    public void modeSelect(TMP_Text buttText){
        foreach(TMP_Text txt in gameModes){
            if ((txt.fontStyle & FontStyles.Bold) != 0)
                txt.fontStyle ^= FontStyles.Bold;
        }

        buttText.fontStyle = FontStyles.Bold;
    }

    //Alters button: Sets argument button to bold text if regular, set to regular if bold.
    //Only to be used on operators selection
    public void buttonBold(TMP_Text buttText){
        buttText.fontStyle ^= FontStyles.Bold;
        if ((buttText.fontStyle & FontStyles.Bold) != 0){          //Currently ON
            buttText.fontSize += 4;
        }
        else{                                                      //Currently OFF
            buttText.fontSize -= 4;
        }
    }
}
