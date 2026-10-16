using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainMenuScreen : MonoBehaviour
{
    //Steuert die Highscore-UI.
    [SerializeField] TMP_Text highScoreText;

    // Update is called once per frame
    void Update()
    {
        highScoreText.text = "Highscore: " + PlayerPrefs.GetInt("Highscore").ToString();
    }
}
