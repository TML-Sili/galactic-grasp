using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMover : MonoBehaviour
{
    private GameObject player;
    [SerializeField] private float extraSpaceX, extraSpaceY; //
    private float camWidth, camHeight;
    public static CameraMover instance;
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
        camHeight = GetComponent<Camera>().orthographicSize;
        camWidth = GetComponent<Camera>().aspect * camHeight;
    }

    // Update is called once per frame
    void Update()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        MoveCam();
    }
    void MoveCam()
    {
        ///bewegt Kamera und folgt dem Spieler
        transform.position = new Vector3(Mathf.Clamp(player.transform.position.x, GameManager.instance.minX + camWidth, GameManager.instance.maxX - camWidth), Mathf.Clamp(player.transform.position.y, GameManager.instance.minY + camHeight, GameManager.instance.maxY - camHeight), -10);
    }
    public void CameraShake(float strength, float duration)
    {
        //startet die Coroutine, welches ein Kamerawackeln simuliert.
        StartCoroutine(ShakeCoroutine(strength, duration));
    }
   IEnumerator ShakeCoroutine(float strength, float duration)
   {
        //Erzeugt ein Kamerawackeln für duration-Sekunden und mit der Stärke strength.
        Vector3 originalPosition;
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            originalPosition = transform.localPosition;
            Vector3 randomOffset = Random.insideUnitSphere * strength;
            transform.localPosition = originalPosition + randomOffset;
            yield return null;
            elapsedTime += Time.deltaTime;
        }
   }
}
