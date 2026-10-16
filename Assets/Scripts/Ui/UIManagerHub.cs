using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManagerHub : MonoBehaviour
{
    //Steuert das Ui, für UiManager hub, basiert stark aud UiManager.
    public static UIManagerHub instance;
    [SerializeField] private AudioSource buttonSound;
    [SerializeField] private List<GameObject> allMenus = new List<GameObject>(); //pauseBildschirm, GameOverBildschirm, u.s.w.
    private void Awake()
    {
        instance = this;
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
        buttonSound.Play();
    }
}
