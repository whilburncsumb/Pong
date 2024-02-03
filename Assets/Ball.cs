using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Ball : MonoBehaviour
{
    private const float initialSpeed = 15f;
    private float currentSpeed;
    private Vector3 direction;
    private Manager manager;
    private Rigidbody _rigidbody;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        StartRound(1);
        manager = transform.parent.GetComponent<Manager>();
    }

    void FixedUpdate()
    {
        MoveBall();
    }

    void MoveBall()
    {
        // Calculate the new position based on the current velocity
        Vector3 newPosition = transform.position + direction * (currentSpeed * Time.deltaTime);

        // Move the ball using Rigidbody to handle collisions
        _rigidbody.MovePosition(newPosition);
    }


    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Paddle"))
        {
            // Reverse the x direction and increase speed
            Debug.Log("bounce!");
            direction = new Vector3(-direction.x,direction.y, direction.z).normalized;
            currentSpeed += 0.2f;
        }
        else if (collision.gameObject.CompareTag("Wall"))
        {
            // Reverse the y direction
            direction = new Vector3(direction.x,-direction.y, direction.z).normalized;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Goal"))
        {
            manager.incrementScore(2);
        }
        else if (other.CompareTag("Goal2"))
        {
            manager.incrementScore(1);
        }
    }

    public void StartRound(int winner)
    {
        SetRandomDirection(winner);
        currentSpeed = initialSpeed;
    }
    
    void SetRandomDirection(int winner)
    {
        Vector3 newDirection;
        // if player 1 won, the ball should go right, otherwise, left
        if (winner == 1)
        {
            newDirection = new Vector3(1f, 0f, 0f);
        }
        else
        {
            newDirection = new Vector3(-1f, 0f, 0f);
        }
        // randomly alter the starting direction in the range of 45 degrees either direction
        float randomAngle = Random.Range(-45f, 45f);
        newDirection = Quaternion.Euler(0f, 0f, randomAngle) * newDirection;
        newDirection.Normalize();
        direction = newDirection;
    }
}

