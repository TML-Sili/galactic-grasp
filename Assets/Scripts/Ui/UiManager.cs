using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiManager : MonoBehaviour
{
    //Steuert das Ui, für UiManager.
    private bool paused;
    public static UiManager instance;
    [SerializeField] private GameObject infosUI;
    [SerializeField] private List<GameObject> allMenus = new List<GameObject>(); //pauseBildschirm, GameOverBildschirm, u.s.w.
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        paused = false;
    }
    public void Update()
    {
        ///Überprüft, ob ESC-Tast gedrückt wird und aktiviert/deaktiviert pausenMenu.
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameManager.instance.buttonSound.Play();
            Pause();
        }
        if (GameManager.instance.gameRunning)
        {
            infosUI.SetActive(true);
            foreach(GameObject arrow in GameObject.FindGameObjectsWithTag("Arrow"))
            {
                arrow.GetComponent<SpriteRenderer>().enabled = true;
            }
        }
        else
        {
            infosUI.SetActive(false);
            foreach (GameObject arrow in GameObject.FindGameObjectsWithTag("Arrow"))
            {
                arrow.GetComponent<SpriteRenderer>().enabled = false;
            }
        }
    }
    public void Pause()
    {
        //aktiviert oder deaktiviert das Pausenmenu.
        if (paused)
        {
            //beendet das Pausieren des Spieles.
            paused = false;
            GameManager.instance.gameRunning = true; ;
            Time.timeScale = 1f;
            CloseAllMenus();
        }
        else if (!paused && GameManager.instance.gameRunning)
        {
            //pausiert das Spiel
            paused = true;
            GameManager.instance.gameRunning = false;
            Time.timeScale = 0f;
            LoadMenu("PauseScreen");
        }
    }
    public void LoadMenu(string menuName)
    {
        //Ladet das Panel mit dem menuName-Namen.
        CloseAllMenus();
        foreach (GameObject menu in allMenus)
        {
            if (menu.name == menuName)
            {
                menu.SetActive(true);
            }
        }
    }

    public void CloseAllMenus()
    {
        //Schliesst alle panels
        foreach (GameObject menu in allMenus)
        {
            menu.SetActive(false);
        }
    }

    public void playButtonSound()
    {
        //Spielt Button-click sound.
        GameManager.instance.buttonSound.Play();
    }
}
