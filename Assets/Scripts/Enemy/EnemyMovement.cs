using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public List<Vector2> targetPositions;
    public float speed;
    private int direction;
    private int currentTargetPosInd;
    public float radius;
    [HideInInspector] public bool canMove;
    // Start is called before the first frame update
    void Start()
    {
        canMove = true;
        direction = 1;
        currentTargetPosInd = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {
            manageTargets();
            MoveEnemy();
        }
    }
    private void manageTargets()
    {
        //wenn Distanz zu Target kleiner als 0.08 Einheiten ist wird target gewechselt.
        if (DistanceCalc2D(transform.position, targetPositions[currentTargetPosInd]) < .08f)
        {
            ChangeTarget();
        }
    }

    float DistanceCalc2D(Vector2 vec1, Vector2 vec2)
    {
        //Berechnet die Distanz in 2d, zwischen zwei vektoren.
        float distance = Mathf.Sqrt(Mathf.Pow((vec1.x-vec2.x),2) + Mathf.Pow((vec1.y - vec2.y), 2));
        return distance;
    }

    void MoveEnemy()
    {
        if (GameManager.instance.gameRunning)
        {
            transform.Translate(MoveTowardsCalc(transform.position, targetPositions[currentTargetPosInd], speed) * Time.deltaTime);
        }
    }

    Vector2 MoveTowardsCalc(Vector2 startPos, Vector2 targetpos, float speed)
    {
        //Berechnet die Geschwindigkeit für x und y-Achse in From eines Vektors.
        Vector2 moveVector = targetpos - startPos;
        moveVector = moveVector.normalized * speed;
        return moveVector;
    }
    public void ChangeTarget()
    {
        currentTargetPosInd = (currentTargetPosInd + 1) % targetPositions.Count;
        direction = -direction;
    }
}

