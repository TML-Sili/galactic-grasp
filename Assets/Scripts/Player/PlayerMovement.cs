using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //Skript welches den Spieler stuert, ohne Seil.
    private float horizontalInput, deltaVelocity;
    public float rotationStrength, velocity, maxVelocity, secondsUntilMaxVelocity, startVelocity;
    public static PlayerMovement instance;

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
        startVelocity = velocity;
        deltaVelocity = maxVelocity - velocity;
    }

    // Update is called once per frame
    void Update()
    {
        //Überprüft ob Spieler sich bewegen darf und bewegt ihn anschliessend.
        if (PlayerInfos.instance.canMove)
        {
            if (!PlayerAbilitites.instance.isConnected)
            {
                horizontalInput = -Input.GetAxis("Horizontal");
                transform.Rotate(new Vector3(0, 0, rotationStrength * Time.deltaTime * horizontalInput));
                velocity = Mathf.Min(velocity + Time.deltaTime * deltaVelocity / secondsUntilMaxVelocity, maxVelocity);

            }
            transform.Translate(Vector3.right * Time.deltaTime * velocity);
        }
    }
}
