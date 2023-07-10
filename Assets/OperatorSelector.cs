using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OperatorSelector : MonoBehaviour
{
    //Keep track of the button objects. When the button is pressed, change the color to indicate ON/OFF.
        //Find out how to change button color on click
        //On click, remove or add the operator string from operators in ButtonBehavior
    public Color offColor;
    public Color onColor;
    public Button plusOp;
    public Button minusOp;
    public Button multiplyOp;
    //public Button divisionOp;
    
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

        foreach( string x in ButtonBehavior.operators) {
            Debug.Log( x.ToString());
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
