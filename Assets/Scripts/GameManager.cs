using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    //Handelt das Spielgeschehen und spawned die Asteroiden am Anfang.
    [HideInInspector] public bool gameRunning;
    [SerializeField]private GameObject spawnProtector, Pole;
    [SerializeField] private int amountPoles;
    [SerializeField] private float minDistancePoles, shiftPole;
    [SerializeField] private TextMeshProUGUI startCountdownText;
    public AudioSource shootSound, explosionSound, turnAroundSound, GameOverSound, shootRopeSound, buttonSound, coinSound, countdownSound, connectSound;
    public static GameManager instance;
    public Vector2 spawnPosition;
    public int pointsPerEnemy;
    public float minX, maxX, minY, maxY;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        amountPoles -= 4;
        gameRunning = false;
        SetUpMap();
        SetUpGame();
    }

    private void SetUpMap()
    {
        //plaziert die Poles auf der Map zufällig;
        for (int poles = 0; poles < amountPoles; poles++)
        {
            GameObject newPole = Instantiate(Pole, Vector3.zero, Quaternion.identity);
            float minDistance = 200;
            do
            {
                minDistance = 200;
                Vector2 posPole = new Vector2(UnityEngine.Random.Range(minX + shiftPole, maxX - shiftPole), UnityEngine.Random.Range(minY + shiftPole, maxY - shiftPole));
                newPole.transform.position = posPole;
                foreach (GameObject pole in GameObject.FindGameObjectsWithTag("pole"))
                {
                    float distance = PlayerAbilitites.instance.DistanceCalc2D(pole.transform.position, posPole);
                    if (distance < minDistance & distance != 0f)
                    {
                        minDistance = distance;
                    }
                }
            } while (minDistance < minDistancePoles);
        }
        
    }
    void SetUpGame()
    {
        //startet die Coroutine, welches Spiel startet.
        StartCoroutine(setPlayer());
    }
    IEnumerator setPlayer()
    {
        //Coroutine, welches den Spieler platziert und den Countdown managet.
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.transform.position = spawnPosition;
        player.transform.Rotate(0, 0, UnityEngine.Random.Range(0f, 360f));
        int TimeUntilStart = 3;
        startCountdownText.text = (TimeUntilStart).ToString();
        countdownSound.Play();
        for (int countdown = 0; countdown < 4; countdown++)
        {
            yield return new WaitForSeconds(1f);
            TimeUntilStart--;
            startCountdownText.text = TimeUntilStart.ToString();
            countdownSound.Play();
            if(TimeUntilStart <= 0)
            {
                startCountdownText.text = "GO!";
            }
        }
        startCountdownText.gameObject.SetActive(false);
        gameRunning = true;
        PlayerInfos.instance.UpdateMovability();
    }
}
