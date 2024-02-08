using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class Manager : MonoBehaviour
{
    public Ball ball;
    public Paddle paddle1;
    public Paddle paddle2;
    public GameObject mainCamera;
    public GameObject powerup;
    public GameObject powerup2;
    public int p1Score;
    public int p2Score;
    public int lastWinner;//This tracks who the previous winner is,
                          //and should be either 1 for player 1 or 2 for player 2
    public float paddleSpeed;
    public float paddleYMin;
    public float paddleYMax;
    public int remainingShakeFrames;
    public float shakeIntensity;
    private Vector3 cameraHomePosition;
    public int powerupCountdown;
    
    private void Start()
    {
        lastWinner = 1;
        p1Score = 0;
        p2Score = 0;
        StartRound();
        cameraHomePosition = mainCamera.transform.position;
        remainingShakeFrames = 0;
        shakeIntensity = 0;
        powerupCountdown = 200;
    }

    private void FixedUpdate()
    {
        //Move the paddles
        float input1 = Input.GetAxis("Vertical");
        //ITs called the horizontal input but it controls the second paddle with up and down
        float input2 = Input.GetAxis("Horizontal");
        MovePaddle(input1, paddle1);
        MovePaddle(input2, paddle2);
        if (remainingShakeFrames > 0)
        {
            Shake();
        }
        else
        {
            mainCamera.transform.position = cameraHomePosition;
        }
        //Decrease powerup countdown timer and spawn a powerup when it reaches 0
        if (powerupCountdown > 0)
        {
            powerupCountdown--;
        }
        else if(powerupCountdown==0)
        {
            spawnPowerup();
            powerupCountdown = -1;
        }
    }

    private void spawnPowerup()
    {
        Instantiate(powerup, new Vector3(0f, 5f, 0f), Quaternion.identity);
        Instantiate(powerup2, new Vector3(0f, -5f, 0f), Quaternion.identity);
    }

    private void StartRound()
    {
        ball.StartRound(lastWinner);
    }

    private void Shake()
    {
        remainingShakeFrames--;
        mainCamera.transform.position = cameraHomePosition + UnityEngine.Random.insideUnitSphere * 
            (shakeIntensity * (float)0.5);
    }
    
    public void TriggerShake(float intensity)
    {
        shakeIntensity = intensity;
        remainingShakeFrames = (int)(8 * shakeIntensity);
        Shake();
    }

    private void MovePaddle(float input, Paddle paddle)
    {
        Vector3 newPosition = new Vector3(0f, 0f, 0f);
        if (!paddle.activeAI)//no ai
        {
            newPosition = new Vector3(0f, input, 0f);
            newPosition = paddle.gameObject.transform.position + newPosition * (paddleSpeed * Time.deltaTime);
        }
        else//ai
        {
            float maxSpeed = (paddleSpeed * Time.deltaTime);
            float yTransform = Mathf.Clamp(ball.transform.position.y - paddle.transform.position.y,-maxSpeed,maxSpeed);
            newPosition = new Vector3(0f, yTransform, 0f); 
            newPosition = paddle.transform.position + newPosition * (paddleSpeed * Time.deltaTime);
        }
        //Make sure the paddles stay within bounds and dont go through walls
        newPosition.y = Math.Clamp(newPosition.y, paddleYMin, paddleYMax);
        //apply the transformations
        paddle.transform.position = newPosition;
    }

    public void IncrementScore(int winner)
    {
        if (winner == 1)
        {
            p1Score++;
            lastWinner = 1;
            Debug.Log("Left Paddle scores!");
        }
        else
        {
            p2Score++;
            lastWinner = 2;
            Debug.Log("Right Paddle scores!");
        }
        Debug.Log($"Current score: {p1Score} to {p2Score}");
        ball.transform.position = new Vector3(0, 0, 0);
        ball.StartRound(winner);

        if (p1Score >= 11 || p2Score >= 11)
        {
            //Win state!
            if (p1Score > p2Score)
            {
                Debug.Log("Game Over, Left Paddle Wins!");
            }
            else
            {
                Debug.Log("Game Over, Right Paddle Wins!");
            }
            Debug.Log("Reseting score...");
            p1Score = 0;
            p2Score = 0;
        }
        
    }
}

/*
 Instructions: 
https://docs.google.com/document/d/1zvTCqMXNgnaEEgkUyic_q291N7PWWan3RHkbNqSYLak/edit
 
https://docs.unity3d.com/Manual/collider-types-interaction.html

Score
- when balls goes past paddle, player on opposite side increase score by 1
- max score 99?

Sound
- ball collides with something
	- different for paddle or wall

Presentation
- 2D
- blocky
- minimal color (black, white)
- middle line (visual only)
- top and bottom lines confine ball as a boundary

Ball
- speeds up after bounce
- angle off of paddle is not real-world physics
- "ball" is square
- spawns in the middle toward the previous scoring player's side
- ball returns to original starting velocity upon spawn

Paddle
- moves vertically
- constrained by top and bottom boundaries
- constant velocity/speed

Players
- single player or 2 player

Goal / Game Rules
- ba

Dynamics
- speed of paddles makes predicting trajectories necessary to win / not lose
*/