using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Ball : MonoBehaviour
{
    public float initialSpeed;
    private float currentSpeed;
    private Vector3 direction;
    private Manager manager;

    void Start()
    {
        initialSpeed = 15f;
        StartRound();
        manager = transform.parent.GetComponent<Manager>();
    }

    void FixedUpdate()
    {
        MoveBall();
    }

    void MoveBall()
    {
        // Calculate the new position based on the current velocity
        Vector3 newPosition = transform.position + direction * currentSpeed * Time.deltaTime;

        // Move the ball using Rigidbody to handle collisions
        GetComponent<Rigidbody>().MovePosition(newPosition);
    }


    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Paddle"))
        {
            // Reverse the x direction and increase speed
            direction = new Vector3(-direction.x,0, direction.z).normalized;
            currentSpeed += 0.5f;
        }
        if (collision.gameObject.CompareTag("Wall"))
        {
            // Reverse the y direction
            direction = new Vector3(direction.x,-direction.y, direction.z).normalized;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        
        
        if (other.CompareTag("Goal"))
        {
            manager.incrementScore(1);
            // StartRound();
        }
        else if (other.CompareTag("Goal2"))
        {
            manager.incrementScore(2);
        }
    }

    public void StartRound()
    {
        
        // Determine initial direction based on who lost the last round
        direction = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f),0).normalized;
        currentSpeed = initialSpeed;
    }
}

