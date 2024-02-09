using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pupil : MonoBehaviour
{
    public Transform ball;
    private Vector3 initialPosition;
    public float pupilDistance = 0.5f;
    
    void Start()
    {
        initialPosition = transform.position;
    }
    
    void FixedUpdate()
    {
        Vector3 directionToBall = ball.position - transform.position;
        directionToBall.Normalize();
        transform.position = initialPosition + directionToBall * pupilDistance;
    }
}
