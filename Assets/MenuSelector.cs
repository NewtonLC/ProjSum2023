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
    public void SelectOperator(string op) {
        switch (op) {
            case "+":
                ChangeButtonColor(plusOp, onColor);
                ChangeOperator("+");
                break;
            case "-":
                ChangeButtonColor(minusOp, onColor);
                ChangeOperator("-");
                break;
            case "*":
                ChangeButtonColor(multiplyOp, onColor);
                ChangeOperator("*");
                break;
        }
    }

    public void SelectDifficulty(string diff) {
        // Set the previous difficulty's button to OFF color
        ChangeButtonColor(GetDifficultyButton(ScoreManager.difficulty), offColor);

        // Set the new difficulty's button to ON color
        ChangeButtonColor(GetDifficultyButton(diff), onColor);

        ScoreManager.difficulty = diff;
    }

    // Method: Change the color of the button
    private void ChangeButtonColor(Button button, Color color) {
        button.GetComponent<Image>().color = color;
    }

    // Helper Method: Get the button associated with the difficulty
    private Button GetDifficultyButton(string diff) {
        switch (diff) {
            case "easy":
                return easyDiff;
            case "medium":
                return mediumDiff;
            case "hard":
                return hardDiff;
            default:
                return null; // Return null or handle an invalid difficulty input appropriately
        }
    }

    //Helper Method: Edit the operators list
    public void ChangeOperator(string op){
        if (ButtonBehavior.operators.Contains(op)){     //Remove operator
            ButtonBehavior.operators.Remove(op);
    }
        else {                                          //Add operator
            ButtonBehavior.operators.Add(op);
        }
    }
}
