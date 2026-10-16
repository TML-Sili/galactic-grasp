using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerInfos : MonoBehaviour
{
    //Skript, welches Infos über den Spieler enthält, für Player.
    [SerializeField] private float killCountdown, secondsPerPoint;
    [HideInInspector] public bool canMove;
    [SerializeField] private GameObject explosionObject, GOScreen;
    public static PlayerInfos instance;
    public int lives, points;
    public TextMeshProUGUI scoreText, livesText;
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
        canMove = false;
        points = 0;
        StartCoroutine(AddPoints());
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.gameRunning)
        {
            OutOfZone();
            UpdateLives();
            UpdateScore();
        }
    }

    private void OutOfZone()
    {
        //Überprüft ob Spieler über den Rand geflogen ist und tötet ihn.
        Vector2 Position = transform.position;
        GameManager Gm = GameManager.instance;
        if (Position.x < Gm.minX || Position.x > Gm.maxX || Position.y < Gm.minY || Position.y > Gm.maxY)
        {
            kill();
        }
    }

    public void UpdateMovability()
    {
        //überschreibt die Variable, ob Spieler sich bewegen kann.
        if (GameManager.instance.gameRunning)
        {
            canMove = true;
        }
        else
        {
            canMove = false;
        }
    }

    public void killedEnemy()
    {
        //wird aufgerufen wenn Gegner getötet wird.
        PlayerAbilitites.instance.remainingRopeTime = Mathf.Max(PlayerAbilitites.instance.maxRopeTime / 2, PlayerAbilitites.instance.remainingRopeTime);
        points += GameManager.instance.pointsPerEnemy;
    }
    public void kill()
    {
        //Tötet Spieler
        PlayerAbilitites.instance.leftR.SetActive(false);
        PlayerAbilitites.instance.rightR.SetActive(false);
        lives--;
        if (lives <= 0)
        {
            GameOver();
            return;
        }
        PlayerMovement.instance.velocity = PlayerMovement.instance.startVelocity;
        canMove = false;
        GameManager.instance.explosionSound.Play();
        CameraMover.instance.CameraShake(.3f, 2f);
        Color playerColor = gameObject.GetComponent<SpriteRenderer>().color;
        transform.position = GameManager.instance.spawnPosition;
        PlayerAbilitites.instance.leftR.SetActive(false);
        PlayerAbilitites.instance.rightR.SetActive(false);
        transform.Rotate(0, 0, UnityEngine.Random.Range(0f, 360f));
        gameObject.GetComponent<SpriteRenderer>().color = new Color(playerColor.r, playerColor.g, playerColor.b, 0f);
        GameManager.instance.gameRunning = false;
        StartCoroutine(KillAnimation());
    }

    IEnumerator KillAnimation()
    {
        ///Animiert das sterben(erblühen der Farbe nach tot).
        Color playerColor = gameObject.GetComponent<SpriteRenderer>().color;
        gameObject.GetComponent<SpriteRenderer>().color = new Color(playerColor.r, playerColor.g, playerColor.b, Mathf.Min(playerColor.a + Time.deltaTime / killCountdown, 1f));
        if (playerColor.a == 1f)
        {
            canMove = true;
            GameManager.instance.gameRunning = true;
        }
        else
        {
            yield return new WaitForEndOfFrame();
            StartCoroutine(KillAnimation());
        }
    }

    IEnumerator AddPoints()
    {
        //fügt jede 3. Sekunde ein punkt hinzu, falls spiel am leben ist.
        yield return new WaitForSeconds(secondsPerPoint);
        if (GameManager.instance.gameRunning)
        {
            points += 1;
        }
        StartCoroutine(AddPoints());
    }

    void GameOver()
    {
        //Wird aufgerufen, wenn Spieler gestorben ist.
        if (points > PlayerPrefs.GetInt("Highscore"))
        {
            PlayerPrefs.SetInt("Highscore", points);
            GOScreen.GetComponent<GameOverScreen>().HighscoreAchieved = true;
        }
        GameManager.instance.gameRunning = false;
        UpdateMovability();
        StartCoroutine(GameOverAnimation());
    }

    IEnumerator GameOverAnimation()
    {
        //Animiert und steuert Sound, wenn Spiel Game Over ist.
        GameObject Explosion = Instantiate(explosionObject, transform.position, Quaternion.identity);
        Destroy(Explosion, 1f);
        GetComponent<SpriteRenderer>().color = Color.clear;
        CameraMover.instance.CameraShake(2f, 1.5f);
        GameManager.instance.explosionSound.Play();
        yield return new WaitForSeconds(2f);
        GameManager.instance.GameOverSound.Play();
        yield return new WaitForSeconds(1f);
        UiManager.instance.LoadMenu("GameOverScreen");
        Time.timeScale = 0f;
    }

    public void UpdateScore()
    {
        //Ändert den ScoreText.
        scoreText.text = "Score: " + points;
    }

    public void UpdateLives()
    {
        //Ändert den LiveText.
        livesText.text = "Lives: " + lives;
    }
}
