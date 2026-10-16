using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowAnimator : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<SpriteRenderer>().color = new Color(GetComponent<SpriteRenderer>().color.r, GetComponent<SpriteRenderer>().color.g, GetComponent<SpriteRenderer>().color.b, GameObject.FindGameObjectWithTag("Player").GetComponent<SpriteRenderer>().color.a);
    }
}
