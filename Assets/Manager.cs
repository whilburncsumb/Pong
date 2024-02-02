using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public Ball ball;
    public GameObject player1;
    public GameObject player2;

    private void Start()
    {
        StartRound();
    }

    private void StartRound()
    {
        // Determine who lost the last round
        bool player1Lost = false/* Your logic to determine if player 1 lost */;
        bool player2Lost = true/* Your logic to determine if player 2 lost */;

        if (player1Lost)
        {
            ball.transform.position = player1.transform.position;
        }
        else if (player2Lost)
        {
            ball.transform.position = player2.transform.position;
        }

        ball.StartRound();
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