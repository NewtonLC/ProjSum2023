using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    //Variable to hold the player's score
    static public int playerScore;

    //Text to display player's score
    public TMP_Text scoreDisplay;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        scoreDisplay.text = "Score: " + playerScore.ToString();
    }
}
