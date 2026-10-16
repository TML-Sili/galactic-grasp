using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class GameOverScreen : MonoBehaviour
{
    //Steuert die Highscore-UI.
    [SerializeField] TMP_Text highScoreText, currentScoreText;
    [SerializeField] GameObject HighscoreObject;
    [HideInInspector] public bool HighscoreAchieved = false;

    private void OnEnable()
    {
        //Überprüft, ob Highscore geknackt wurde und Aktiviert das passende GameObjekt.
        if (HighscoreAchieved)
        {
            HighscoreObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        highScoreText.text = "" + PlayerPrefs.GetInt("Highscore").ToString();
        currentScoreText.text = "" + PlayerInfos.instance.points.ToString();
    }
}
