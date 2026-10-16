using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{
    //Steuert zwischen den Szenen hin und her, für SceneManager.
    public static SceneHandler instance;
    private void Awake()
    {
        instance = this;
    }
    //Wechselt zwischen den Szenen hin und her. (handhabt sie)
    public void Restart()
    {
        //Das aktuelle Level wird neu gestartet.
        Time.timeScale = 1f;
        SceneManager.LoadScene("SampleScene");
    }

    public void GoToMainMenu()
    {
        //Das Hauptmenü wird geladen.
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
    public void QuitGame()
    {
        //Beendet das Spiel
        Application.Quit();
    }

    public void GoToControls()
    {
        //Das Steuerungsmenu wird geladen.
        Time.timeScale = 1f;
        SceneManager.LoadScene("ControlsScreen");
    }
}