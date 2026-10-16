using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    //Regelt das steuern des Spielers, bei berührung von Seil und Asteroid, für Asteroid.
    private GameObject targetPlayer, rope;
    [SerializeField] private List<Sprite> possibleMeteroids;
    private float rotationsPerSecond;

    private void Start()
    {
        rotationsPerSecond = Random.Range(-.05f, .05f);
        GetComponent<SpriteRenderer>().sprite = possibleMeteroids[Random.Range(0, possibleMeteroids.Count)];
        gameObject.transform.Rotate(0, 0, UnityEngine.Random.Range(0f, 360f));

    }

    private void Update()
    {
        gameObject.transform.Rotate(0f, 0f, 360 * rotationsPerSecond * Time.deltaTime);
    }
    // Start is called before the first frame update
    private void OnTriggerStay2D(Collider2D collision)
    {
        //Überprüft ob Pole von Seil berührt wird
        targetPlayer = GameObject.FindGameObjectWithTag("Player");
        if (collision.tag == "left")
        {
            //Rotiert spieler nach links, bei Berührung mit linkem Seil.
            rope = PlayerAbilitites.instance.leftR;
            RotatePlayer(1f);
        }
        else if (collision.tag == "right")
        {
            //Rotiert Spieler nach rechts, bei Berührung mit rechtem Seil.
            rope = PlayerAbilitites.instance.rightR;
            RotatePlayer(-1f);
        }
        if (collision.tag == "right" || collision.tag == "left")
        {
            //Falls Seil mit Asteroid verbunden ist wird Variable isConnected auf true gesetzt.
            PlayerAbilitites.instance.isConnected = true;
            rope.transform.position = (transform.position + targetPlayer.transform.position) / 2f;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "left" || collision.tag == "right")
        {
            //Wird Berührung zwischen Seil und Asteroid beendet, wird isConnected auf False gesetzt.
            PlayerAbilitites.instance.isConnected = false;
            PlayerAbilitites.instance.resetRopePosition();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            //Bei Berührung von Ateroid und Spieler, wird Spieler getötet.
            PlayerInfos.instance.kill();
        }
        if (collision.tag == "right" || collision.tag == "left")
        {
            GameManager.instance.connectSound.Play();
        }
    }
    void RotatePlayer(float direction)
    {
        //Berechnet den Grad, der sich spieler drehen muss um um den Pole zu fliegen. Zu Dem wird der spieler ein bisschen nach ausen gesetzt, damit er nicht immer näher an das Seil kommt.
        Vector2 distanceVector = targetPlayer.transform.position - transform.position;
        float distance = Mathf.Sqrt(Mathf.Pow(distanceVector.x, 2) + Mathf.Pow(distanceVector.y, 2));
        float umfang = distance * 2 * Mathf.PI; //Kreisumfang von Spieler um Pole herumzufliegen.
        float rotationpower = (360f / (umfang / targetPlayer.GetComponent<PlayerMovement>().velocity)); //Grad die Spieler rotieren muss pro Sekunde
        float shiftPlayer = (rotationpower / 360f) * (Mathf.Pow(GetComponent<CircleCollider2D>().bounds.size.x / 2 - GetComponent<CircleCollider2D>().radius * .05f , 2) * Mathf.PI); //bewegung von Spieler nach unten, damit er nicht an Pole annähert
        targetPlayer.transform.Rotate(0f, 0f,rotationpower * direction *  Time.deltaTime );
        targetPlayer.transform.Translate(Vector3.down * shiftPlayer * direction * Time.deltaTime);
        PlayerAbilitites.instance.distancePole = Mathf.Sqrt(Mathf.Pow(distanceVector.x, 2) + Mathf.Pow(distanceVector.y, 2)) + .3f;
    }

}
