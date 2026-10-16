using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLogic : MonoBehaviour
{
    ///Skript, welches die Logik der Gegner bestimmt, für Gegner.
    [SerializeField] int worthPoints;
    private bool canChange;
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "bullet")
        {
            //Tötet Monster, falls getroffen von Bullet.
            PlayerInfos.instance.points += worthPoints;
            GetComponent<CircleCollider2D>().enabled = false;
            GetComponent<EnemyMovement>().canMove = false;
            Color enemyColor = gameObject.GetComponent<SpriteRenderer>().color;
            gameObject.GetComponent<SpriteRenderer>().color = new Color(.8f, 0f, 0f, enemyColor.a);
            GameManager.instance.explosionSound.Play();
            CameraMover.instance.CameraShake(.2f, 1f);
            StartCoroutine(KillAnimation());
        }
        else if(collision.tag == "Player")
        {
            //Spieler wird bei Kollision getötet. 
            PlayerInfos.instance.kill();
        }
        else if(collision.tag == "left" || collision.tag == "right")
        {
            //Gegner wird berührt von einem der Seile, wird getötet und setzt SeilZeit auf 0.
            PlayerInfos.instance.points += worthPoints;
            GetComponent<CircleCollider2D>().enabled = false;
            GetComponent<EnemyMovement>().canMove = false;
            Color enemyColor = gameObject.GetComponent<SpriteRenderer>().color;
            gameObject.GetComponent<SpriteRenderer>().color = new Color(.8f, 0f, 0f, enemyColor.a);
            GameManager.instance.explosionSound.Play();
            CameraMover.instance.CameraShake(.2f, 1f);
            StartCoroutine(KillAnimation());
            PlayerAbilitites.instance.remainingRopeTime = 0f;
        }
        else if(collision.tag == "pole")
        {
            //Wechselt die richtung, bei Kollision mit Asteroid.
            GetComponent<EnemyMovement>().ChangeTarget();
        }
        else if (collision.tag == "Enemy" & canChange)
        {
            //Wechselt die Richtung, bei Kollision mit Gegnern, falls es kann.
            GetComponent<EnemyMovement>().ChangeTarget();
            canChange = false;
            StartCoroutine(canChangeCountdown());
        }
    }

    private IEnumerator canChangeCountdown()
    {
        //Countdown, damit Gegner nicht beginnen zu buggen, steuert wenn sie wechseln können.
        yield return new WaitForSeconds(.1f);
        canChange = true;
    }

    IEnumerator KillAnimation() 
    {
        //Animation, welche bei Tötung des Gegners aufgerufen wird.
        Color enemyColor = gameObject.GetComponent<SpriteRenderer>().color;
        gameObject.GetComponent<SpriteRenderer>().color = new Color(enemyColor.r, enemyColor.g, enemyColor.b, Mathf.Max(enemyColor.a - Time.deltaTime / 1.5f, 0f));
        if (enemyColor.a != 0f)
        {
            yield return new WaitForEndOfFrame();
            StartCoroutine(KillAnimation());
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
