using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerAbilitites : MonoBehaviour
{
    //Skript, welches die Fähigkeiten des Spielers steuert, für Spieler.
    [SerializeField] int maxBullets;
    [SerializeField] float reloadMultiplierRope, bulletAddTime, ropeLength, timerRope;
    [SerializeField] private GameObject bullet;
    [HideInInspector] public bool isConnected;
    public float distancePole, remainingRopeTime, maxRopeTime, remainingRopeTimeRounded;
    public GameObject leftR, rightR; //linkes und rechtes Seil.
    public static PlayerAbilitites instance;
    public int turnarounds;
    private float ropeInput, timeUntilReloadRope; //Input für Seile
    private int amountBullets;
    private bool bulletReloading, reloadingRope, ropeActive; //True falls Bullets nachgeladen werden oder Zeit für Seil nachgeladen wird.
    public TextMeshProUGUI remainingRopeTimeText, bulletsAmountText, turnAroundText;


    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        timeUntilReloadRope = timerRope;
        remainingRopeTime = maxRopeTime;
        amountBullets = maxBullets;
        bulletReloading = false;
        reloadingRope = false;
        ropeActive = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.gameRunning)
        {
            shootManager();
            UpdateBulletsAmountText();
            UpdateRemainingRopeTimeText();
            UpdateTurnAroundText();
        }
        if (PlayerInfos.instance.canMove)
        {
            turnManager();
            ropeManager();
            cooldowns();
        }
    }

    private void shootManager()
    {
        //Steuert das Schiessen des Spielers.
        if (Input.GetButtonDown("Fire") & amountBullets > 0 & GameManager.instance.gameRunning)
        {
            Instantiate(bullet, transform.position, transform.rotation);
            GameManager.instance.shootSound.Play();
            amountBullets--;
        }
    }
    private void turnManager()
    {
        //Steuert die Umdrehfähigkeit des Spielers.
        if (Input.GetButtonDown("Turn") & turnarounds > 0)
        {
            PlayerInfos.instance.canMove = false;
            StartCoroutine(turnaroundAnimation());
            turnarounds--;
        }
    }

    IEnumerator turnaroundAnimation()
    {
        //Animiert die Umdreh-fähigkeit des Spielers.
        float startRotation = transform.localRotation.z * 180;
        float rotated = 0;
        GameManager.instance.turnAroundSound.Play();
        while (rotated < 180f)
        {
            transform.Rotate(0f, 0f, Time.deltaTime * 130);
            rotated += Time.deltaTime * 130;
            yield return new WaitForEndOfFrame();
        }
        PlayerInfos.instance.canMove = true;
    }

    void ropeManager()
    {
        //Steuert, welches Seil aktiv ist.
        ropeInput = Input.GetAxis("FireRope");
        {
            switch (ropeInput)
            {
                case > 0.5f:
                    ropeActive = true;
                    rightR.SetActive(true);
                    leftR.SetActive(false);

                    break;
                case < -0.5f:
                    ropeActive = true;
                    rightR.SetActive(false);
                    leftR.SetActive(true);
                    break;
                default:
                    ropeActive = false;
                    rightR.SetActive(false);
                    leftR.SetActive(false);
                    break;
            }
            if (remainingRopeTime <= 0f)
            {
                ropeActive = false;
                rightR.SetActive(false);
                leftR.SetActive(false);
            }
        }
        if (isConnected)
        {
            //passt die Seillänge an
            GameObject CurrentRope = gameObject;
            switch (ropeInput)
            {
                case > 0.5f:
                    CurrentRope = rightR;
                    break;
                case < -0.5f:
                    CurrentRope = leftR;
                    break;
            }
            CurrentRope.transform.localScale = new Vector2(CurrentRope.transform.localScale.x, distancePole);
        }
    }
    public float DistanceCalc2D(Vector2 vec1, Vector2 vec2)
    {
        //Berechnet die Distanz in 2d, zwischen zwei vektoren.
        float distance = Mathf.Sqrt(Mathf.Pow((vec1.x - vec2.x), 2) + Mathf.Pow((vec1.y - vec2.y), 2));
        return distance;
    }

    public void resetRopePosition()
    {
        //Bringt die Seile zurück an die ursprüngliche Position.
        rightR.transform.localPosition = new Vector2(0, - ropeLength / 2);
        leftR.transform.localPosition = new Vector2(0, ropeLength / 2);
        rightR.transform.localScale = new Vector2(rightR.transform.localScale.x, ropeLength);
        leftR.transform.localScale = new Vector2(leftR.transform.localScale.x, ropeLength);
    }
    void cooldowns()
    {
        //Steuert, wenn welche Fähigkeit beriet ist um zu benutzen.
        if(amountBullets < maxBullets & !bulletReloading)
        {
            StartCoroutine(addBullets());
        }
        if(remainingRopeTime < maxRopeTime)
        {
            if (!ropeActive & !reloadingRope)
            {
                timeUntilReloadRope -= Time.deltaTime;
                if (timeUntilReloadRope < 0f)
                {
                    StartCoroutine(addTimeRope());
                    reloadingRope = true;
                }
            }
            if(ropeActive)
            {
                timeUntilReloadRope = timerRope;
            }
        }
        if (ropeActive)
        {
            remainingRopeTime = Mathf.Max(0, remainingRopeTime - Time.deltaTime);
        }
    }

    IEnumerator addBullets()
    {
        //Fügt dem Spieler die schüsse hinzu.
        bulletReloading = true;
        yield return new WaitForSeconds(bulletAddTime);
        if(amountBullets < maxBullets)
        {
            amountBullets ++;
        }
        if (amountBullets < maxBullets)
        {
            StartCoroutine(addBullets());
        }
        else
        {
            bulletReloading = false;
        }
    }

    IEnumerator addTimeRope()
    {
        //Fügt dem Spieler Seilzeit hinzu.
        yield return new WaitForEndOfFrame();
        remainingRopeTime = MathF.Min(remainingRopeTime + Time.deltaTime * reloadMultiplierRope, maxRopeTime);
        if(!(remainingRopeTime > maxRopeTime || ropeActive))
        {
            StartCoroutine(addTimeRope());
        }
        else
        {
            reloadingRope = false;
        }
    }

    public void UpdateRemainingRopeTimeText()
    {
        //UI Texte updaten
        remainingRopeTimeRounded = (int)Math.Round(remainingRopeTime);
        remainingRopeTimeText.text = remainingRopeTimeRounded + " sec";
    }

    public void UpdateBulletsAmountText()
    {
        //UI Texte updaten
        bulletsAmountText.text = "" + amountBullets;
    }

    public void UpdateTurnAroundText()
    {
        //UI Texte updaten
        turnAroundText.text = "" + turnarounds; 
    }
}
