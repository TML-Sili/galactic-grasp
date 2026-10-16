using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateBackground : MonoBehaviour
{
    //Animationsskript, welches ein Objekt auf der Z-achse strength-mal pro sekunde um 360° rotiert.
    [SerializeField] float strength;
    // Update is called once per frame
    void Update()
    {
        //Rotiert das Objekt.
        transform.Rotate(0f, 0f, 360 * Time.deltaTime * strength);
    }
}
