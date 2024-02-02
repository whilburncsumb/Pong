using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public Ball ball;
    public GameObject player1;
    public GameObject player2;
    public int p1Score;
    public int p2Score;
    public int lastWinner;//This tracks who the previous winner is,
                          //and should be either 1 for player 1 or 2 for player 2

    private void Start()
    {
        lastWinner = 1;
        p1Score = 0;
        p2Score = 0;
        StartRound();
        Transform ballTrans = transform.Find("Ball");
        ball = ballTrans.GetComponent<Ball>();

    }

    private void StartRound()
    {
        // Determine who lost the last round


        if (lastWinner==1)
        {
            ball.transform.position = player1.transform.position;
        }
        else
        {
            ball.transform.position = player2.transform.position;
        }

        ball.StartRound();
    }

    public void incrementScore(int winner)
    {
        
        if (winner == 1)
        {
            p1Score++;
            Debug.Log("Player 1 scores!");
        }
        else
        {
            p2Score++;
            Debug.Log("Player 2 scores!");
        }
        Debug.Log($"Current score: {p1Score} to {p2Score}");
        ball.transform.position = new Vector3(0, 0, 0);

        if (p1Score >= 11 || p2Score >= 11)
        {
            //Win state!
            
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