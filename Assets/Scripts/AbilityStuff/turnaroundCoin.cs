using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class turnaroundCoin : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            GameManager.instance.coinSound.Play();
            PlayerAbilitites.instance.turnarounds++;
            Destroy(gameObject);
        }
    }
}
