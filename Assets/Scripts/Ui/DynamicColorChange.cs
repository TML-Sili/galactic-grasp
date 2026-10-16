using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicColorChange : MonoBehaviour
{
    //Animationsskript, welches einen dynamischen Farbenwechselt simuliert.
    [SerializeField] List<Color> colors;
    [SerializeField] private float transitionTime;
    private Color currentColor;
    private int targetColorIndex;
    // Start is called before the first frame update
    void Start()
    {
        targetColorIndex = 1;
    }
    private void OnEnable()
    {
        //Startet das animieren, falls GameObject aktiviert wird und überprüft ob es ein Seil ist um passenden Sound abzuspielen.
        StartCoroutine(changeColor(transitionTime));
        if((gameObject.tag == "left" || gameObject.tag == "right") && PlayerAbilitites.instance.remainingRopeTime > .1f)
        {
            GameManager.instance.shootRopeSound.Play();
        }
    }
    public IEnumerator changeColor(float transitionTime)
    {
        //Coroutine, welche innerhalb von transitionTime dynamisch von der einen zur nächsten Farbe wechselt. 
        currentColor = GetComponent<SpriteRenderer>().color;
        Vector3 difference = new Vector3(colors[targetColorIndex].r - currentColor.r, colors[targetColorIndex].g - currentColor.g, colors[targetColorIndex].b - currentColor.b);
        float elapsedTime = 0f;
        while (elapsedTime < transitionTime)
        {
            GetComponent<SpriteRenderer>().color = Color.Lerp(currentColor, colors[targetColorIndex], elapsedTime / transitionTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        targetColorIndex = (targetColorIndex + 1) % colors.Count;
        StartCoroutine(changeColor(transitionTime));
    }
}
