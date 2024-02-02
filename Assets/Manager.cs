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
