using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public float initialSpeed = 5f;
    private float currentSpeed;
    private Vector3 direction;

    void Start()
    {
        StartRound();
    }

    void Update()
    {
        MoveBall();
    }

    void MoveBall()
    {
        transform.Translate(direction * currentSpeed * Time.deltaTime);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Paddle"))
        {
            // Reverse the direction and increase speed
            direction = new Vector3(-direction.x,0, direction.z).normalized;
            currentSpeed += 0.5f;
        }
    }

    public void StartRound()
    {
        // Determine initial direction based on who lost the last round
        direction = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
        currentSpeed = initialSpeed;
    }
}

