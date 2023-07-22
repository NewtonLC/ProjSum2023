using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuSelector : MonoBehaviour
{
    //Keep track of the button objects. When the button is pressed, change the color to indicate ON/OFF.
    public Color offColor;
    public Color onColor;

    //Operator Selector objects
    public Button plusOp;
    public Button minusOp;
    public Button multiplyOp;
    //public Button divisionOp;

    //Difficulty Selector objects
    public Button easyDiff;
    public Button mediumDiff;
    public Button hardDiff;
    
    //Method: Manages selector button being pressed.
    // - Change the button's color
    // - Add/Remove the operator from operators in ButtonBehavior
    public void selectOperator(string op){
        switch(op){
            case "+":
                changeButtonColor(plusOp);
                changeOperator("+");
                break;
            case "-":
                changeButtonColor(minusOp);
                changeOperator("-");
                break;
            case "*":
                changeButtonColor(multiplyOp);
                changeOperator("*");
                break;
        }
    }

    public void selectDifficulty(string diff){
        switch(ScoreManager.difficulty){     //Set the previous difficulty's button to OFF color
            case "easy":
                changeButtonColor(easyDiff);
                break;
            case "medium":
                changeButtonColor(mediumDiff);
                break;
            case "hard":
                changeButtonColor(hardDiff);
                break;
        }
        ScoreManager.difficulty = diff;
        switch(diff){           //Set the new difficulty's button to ON color
            case "easy":
                changeButtonColor(easyDiff);
                break;
            case "medium":
                changeButtonColor(mediumDiff);
                break;
            case "hard":
                changeButtonColor(hardDiff);
                break;
        }
    }

    //Method: Change the color of the button to reflect on/off
    public void changeButtonColor(Button button){
        if (buttonOn(button)){      //Turn off
            button.GetComponent<Image>().color = offColor;
        }
        else{                       //Turn on
            button.GetComponent<Image>().color = onColor;
        }
    }

    //Helper Method: Check if buttons are on/off based on their color
    private bool buttonOn(Button button){
        return button.GetComponent<Image>().color == onColor;
    }

    //Helper Method: Edit the operators list
    public void changeOperator(string op){
        if (ButtonBehavior.operators.Contains(op)){     //Remove operator
            ButtonBehavior.operators.Remove(op);
        }
        else {                                          //Add operator
            ButtonBehavior.operators.Add(op);
        }
    }
}
