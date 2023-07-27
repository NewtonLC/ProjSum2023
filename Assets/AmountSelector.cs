using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AmountSelector : MonoBehaviour
{
    //Set amounts of time and problems
    static public int[] AMOUNT_TIMES = new int[] {15,30,45,60,90};
    static public int[] AMOUNT_PROBLEMS = new int[] {10,20,30,40,50};

    //The "amount" of either time or problems. This will be referenced by other scripts, namely ScoreScreenManager.
    static public int amount = 0;

    //The five amount-button objects, for hiding/showing/altering
    public Button[] amountButtons;
    public TMP_Text[] amountTexts;

    //Method to change the amount. Buttons that call this method also call 
    public void changeAmount(int newAmo){
        amount = newAmo;
    }

    //Method to hide/alter the amount buttons(depending on which mode is selected)
    public void editAmounts(string userEdit){
        switch(userEdit){
            case "time":
                for (int i = 0;i < amountButtons.Length;i++){
                    amountButtons[i].gameObject.SetActive(true);
                    amountTexts[i].text = AMOUNT_TIMES[i].ToString();
                }
                break;
            case "problems":
                for (int i = 0;i < amountButtons.Length;i++){
                    amountButtons[i].gameObject.SetActive(true);
                    amountTexts[i].text = AMOUNT_PROBLEMS[i].ToString();
                }
                break;
            default:
                foreach (Button amount in amountButtons){
                    amount.gameObject.SetActive(false);
                }
                break;
        }
    }
}
