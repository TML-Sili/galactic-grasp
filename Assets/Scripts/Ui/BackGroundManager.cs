using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundManager : MonoBehaviour
{
    private float xScale, yScale;
    // Start is called before the first frame update
    void Start()
    {
        xScale = (GameManager.instance.maxX - GameManager.instance.minX) / gameObject.GetComponent<SpriteRenderer>().bounds.size.x;
        yScale = (GameManager.instance.maxY - GameManager.instance.minY) / gameObject.GetComponent<SpriteRenderer>().bounds.size.y;
        float actualScale = Mathf.Max(xScale, yScale);
        transform.localScale = new Vector3(actualScale, actualScale);
        transform.position = new Vector2((GameManager.instance.maxX + GameManager.instance.minX) / 2, (GameManager.instance.maxY + GameManager.instance.minY));

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
