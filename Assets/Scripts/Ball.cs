using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Ball : MonoBehaviour
{
    private const float initialSpeed = 15f;
    private const float speedIncrease = 5f;
    private float currentSpeed;
    private Vector3 direction;
    private Manager manager;
    public Paddle player1;
    public Paddle player2;
    private Rigidbody _rigidbody;
    public AudioClip high;
    public AudioClip low;
    private AudioSource audioSource;
    public int lastPlayerToHit;
    private Color color;
    public Renderer _renderer;
    public ParticleSystem fire;
    public float particleRate;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        manager = transform.parent.GetComponent<Manager>();
        audioSource = GetComponent<AudioSource>();
        lastPlayerToHit = 1;
        StartRound(1);
    }

    void FixedUpdate()
    {
        MoveBall();

    }

    void MoveBall()
    {
        //determine the new position of the ball based on velocity
        Vector3 newPosition = transform.position + direction * (currentSpeed * Time.deltaTime);
        _rigidbody.MovePosition(newPosition);
    }


    void OnCollisionEnter(Collision collision)
    {
        // if (collision.gameObject.CompareTag("Paddle"))
        // {
        //     // Reverse the x direction and increase speed
        //     direction = new Vector3(-direction.x,direction.y, direction.z).normalized;
        //     currentSpeed += speedIncrease;
        //     audioSource.clip = high;
        //     audioSource.Play();
        // }
        if (collision.gameObject.CompareTag("Paddle"))
        {
            // Calculate the angle between the ball and paddle's xy positions
            Vector3 ballPosition = transform.position;
            Vector3 paddlePosition = collision.transform.position;

            float angle = Mathf.Atan2(paddlePosition.y - ballPosition.y, paddlePosition.x - ballPosition.x) * Mathf.Rad2Deg;

            // Reverse the x direction based on the calculated angle
            direction = new Vector3(Mathf.Cos(Mathf.Deg2Rad * angle), Mathf.Sin(Mathf.Deg2Rad * angle), direction.z).normalized;
            direction = new Vector3(-direction.x,direction.y, direction.z).normalized;
            currentSpeed += speedIncrease;

            // play the high pitched sound
            audioSource.clip = high;
            audioSource.Play();
            //tell the manager to shake the camera
            manager.TriggerShake(currentSpeed/initialSpeed);
            //change the ball's material to be more red as it speeds up
            color = Color.Lerp(Color.white,Color.red, (currentSpeed - initialSpeed)/15f);
            _renderer.material.color = color;
            //control the frequency of fire particle emissions
            var emission = fire.emission;
            particleRate = (currentSpeed - initialSpeed)* 10f;
            emission.rateOverTimeMultiplier = particleRate;
            if (collision.gameObject.name == "leftPaddle")
            {
                lastPlayerToHit = 1;
            }
            else
            {
                lastPlayerToHit = 2;
            }
        }
        else if (collision.gameObject.CompareTag("Wall"))
        {
            //reverse the y direction and play the low pitched sound
            direction = new Vector3(direction.x,-direction.y, direction.z).normalized;
            audioSource.clip = low;
            audioSource.Play();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Goal"))
        {
            manager.IncrementScore(2);
        }
        else if (other.CompareTag("Goal2"))
        {
            manager.IncrementScore(1);
        } else if (other.CompareTag("PowerUp"))
        {
            if (lastPlayerToHit == 1)
            {
                player1.powerUp();
            }
            else
            {
                player2.powerUp();
            }
            Destroy(other.gameObject);
            manager.powerupCountdown = 500;
        }
    }

    public void StartRound(int winner)
    {
        SetRandomDirection(winner);
        currentSpeed = initialSpeed;
        color = Color.white;
        _renderer.material.color = color;
        var emission = fire.emission;
        particleRate = 0;
        emission.rateOverTimeMultiplier = particleRate;
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

