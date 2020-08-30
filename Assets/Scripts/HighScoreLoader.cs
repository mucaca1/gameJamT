using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScoreLoader : MonoBehaviour
{
    public Text highScoreText;

    void Start() 
    {
        highScoreText.text = "";
        HighScoreEntry[] entries = GameData.GetInstance().GetHighScores();

        for (int i = 0; i < entries.Length; i++)
        {
            if (entries[i] == null)
                highScoreText.text += (i + 1) + ": EMPTY \n";
            else
                highScoreText.text += (i + 1) + ": " + entries[i].name + " " + entries[i].score + "\n";
        }    
    }
}
