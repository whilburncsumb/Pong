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
    public Vector3 direction;
    private Manager manager;
    public Paddle player1;
    public Paddle player2;
    private Rigidbody _rigidbody;
    public AudioClip blip1;
    public AudioClip blip2;
    public AudioClip blip3;
    public AudioClip blip4;
    public AudioClip powerUp;
    private AudioSource audioSource;
    public AudioSource audioSource2;
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
        if (collision.gameObject.CompareTag("Paddle"))
        {
            //determine the new direction of the ball based on the difference in y position between the ball and paddle
            Vector3 ballPosition = transform.position;
            Vector3 paddlePosition = collision.transform.position;
            //clamp the y vector to prevent the ball from going at extreme angles that make it take a long time to cross the field
            float ydiff = Mathf.Clamp(ballPosition.y - paddlePosition.y,-.8f,.8f);
            //reverse the x vector and apply the new y vector
            direction = new Vector3(-direction.x,ydiff, direction.z).normalized;
            currentSpeed += speedIncrease;

            switch (currentSpeed)
            {
                // choose a sound to play based on the speed
                case <= initialSpeed + speedIncrease:
                    audioSource.clip = blip3;
                    break;
                case <= initialSpeed + (speedIncrease * 3f):
                    audioSource.clip = blip2;
                    break;
                default:
                    audioSource.clip = blip1;
                    break;
            }
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
            lastPlayerToHit = collision.gameObject.name == "leftPaddle" ? 1 : 2;
        }
        else if (collision.gameObject.CompareTag("Wall"))
        {
            //reverse the y direction and play the low pitched sound
            direction = new Vector3(direction.x,-direction.y, direction.z).normalized;
            audioSource.clip = blip4;
            audioSource.Play();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Determine if the ball hit a goal or a powerup
        if (other.CompareTag("Goal"))
        {
            manager.IncrementScore(2);
        }
        else if (other.CompareTag("Goal2"))
        {
            manager.IncrementScore(1);
        } 
        else if (other.CompareTag("PowerUp"))
        {
            if (lastPlayerToHit == 1)
            {
                player1.powerUp();
            }
            else
            {
                player2.powerUp();
            }
            audioSource2.Play();
            Destroy(other.gameObject);
            manager.powerupCountdown = 500;
        }
        else if (other.CompareTag("PowerUp2"))
        {
            currentSpeed *= 2f;
            audioSource2.Play();
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

