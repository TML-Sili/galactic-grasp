using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    [SerializeField] private float velocity, despawnTime;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, despawnTime);
    }

    // Update is called once per frame
    void Update()
    {
        moveBullet();
    }

    private void moveBullet()
    {
        transform.Translate(Vector3.right * velocity * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!(collision.tag == "Player" || collision.tag == "left" || collision.tag == "right" || collision.tag == "SpawnProtection" || collision.tag == "coin"))
        {
            Destroy(gameObject, .01f);
        }
    }
}
