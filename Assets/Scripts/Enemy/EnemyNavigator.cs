using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyNavigator : MonoBehaviour
{
    private GameObject[] arrows, enemies;
    private List<GameObject> enemiesOutOfRange;
    private float camWidth, camHeight;
    private int currentArrows;
    [SerializeField] private float minSize, maxSize, distanceRangeScaler;
    [SerializeField] private GameObject arrow;
    // Start is called before the first frame update
    void Start()
    {
        currentArrows = 0;
        camHeight = Camera.main.GetComponent<Camera>().orthographicSize;
        camWidth = Camera.main.GetComponent<Camera>().aspect * camHeight;
        arrows = GameObject.FindGameObjectsWithTag("Arrow");
    }

    // Update is called once per frame
    void Update()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        arrows = GameObject.FindGameObjectsWithTag("Arrow");
        ChangeArrowAmounts();
        arrows = GameObject.FindGameObjectsWithTag("Arrow");
        RearrangeArrows();
    }

    void ChangeArrowAmounts()
    {
        //Überprüft wie viel pfeile es braucht um Gegner anzuzeigen. (löscht oder fügt neue arrows hinzu.)
        enemiesOutOfRange = new List<GameObject>();
        int ArrowCount = 0;
        foreach(GameObject enemy in enemies)
        {
            if(Mathf.Abs(enemy.transform.position.x- Camera.main.transform.position.x) > camWidth || Mathf.Abs(enemy.transform.position.y - Camera.main.transform.position.y) > camHeight)
            {
                enemiesOutOfRange.Add(enemy);
                ArrowCount++;
            }
        }
        currentArrows = arrows.Length;
        int arrowsToPlace = ArrowCount - currentArrows;
        if (arrowsToPlace > 0)
        {
            for(int place = 0; place < arrowsToPlace; place++)
            {
                Instantiate(arrow);
            }
        }
        else if(arrowsToPlace < 0)
        {
            for (int destroy = 0; destroy < -arrowsToPlace; destroy++)
            {
                Destroy(arrows[destroy]);
            }
        }
    }
    void RearrangeArrows()
    {
        //Skaliert, rotiert und Platziert die Pfeile auf der Map.
        int index = 0;
        foreach(GameObject enemieToPoint in enemiesOutOfRange)
        {
            Vector2 targetPosition = enemiesOutOfRange[index].transform.position;
            float arrowSize = arrows[index].GetComponent<SpriteRenderer>().bounds.size.x * .9f;
            arrows[index].transform.position = new Vector2(Mathf.Clamp(targetPosition.x, Camera.main.transform.position.x - camWidth + arrowSize, Camera.main.transform.position.x + camWidth - arrowSize), Mathf.Clamp(targetPosition.y, Camera.main.transform.position.y - camHeight + arrowSize, Camera.main.transform.position.y + camHeight - arrowSize));
            float size =  distanceRangeScaler / PlayerAbilitites.instance.DistanceCalc2D(arrows[index].transform.position, targetPosition);
            size = Mathf.Clamp(size, minSize, maxSize);
            arrows[index].transform.localScale = new Vector3(size, size, 1);
            arrows[index].transform.rotation = Quaternion.Euler(0f, 0f, RotateTo(arrows[index].transform.position, targetPosition));
            index++;

        }
    }
    public float RotateTo(Vector3 fromPosition, Vector3 toPosition)
    {
        // Berechne die Rotation in grad vom Ausgangspunkt zum Ziel
        Vector2 direction = (toPosition - fromPosition).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        return angle;
    }
}
