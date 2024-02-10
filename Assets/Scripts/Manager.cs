using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Manager : MonoBehaviour
{
    public Ball ball;
    public Paddle paddle1;
    public Paddle paddle2;
    public GameObject mainCamera;
    public GameObject powerup;
    public GameObject powerup2;
    public float paddleSpeed;
    public int p1Score;
    public int p2Score;
    public int lastWinner;//This tracks who the previous winner is,
                          //and should be either 1 for player 1 or 2 for player 2
    
    private float shakeDuration;
    private float shakeIntensity;
    private float shakeStartTime;
    private Vector3 cameraHomePosition;
    
    public int powerupCountdown;
    public TextMeshProUGUI p1Text;
    public TextMeshProUGUI p2Text;
    private float p1TextColor;
    private float p2TextColor;
    public int scoreThreshold;//score needed to win

    public GameObject background;
    private Renderer _texture;
    public Texture[] textures;
    
    private void Start()
    {
        lastWinner = 1;
        p1Score = 0;
        p2Score = 0;
        StartRound();
        cameraHomePosition = mainCamera.transform.position;
        shakeIntensity = 0;
        powerupCountdown = 200;
        setGuiText(p1Text,0);
        setGuiText(p2Text,0);
        p1TextColor = 0;
        p2TextColor = 0;
        _texture = background.GetComponent<Renderer>();
    }

    private void FixedUpdate()
    {
        //Move the paddles
        float input1 = Input.GetAxis("Vertical");
        //Its called the horizontal input but it controls the second paddle with up and down
        float input2 = Input.GetAxis("Horizontal");
        MovePaddle(input1, paddle1);
        MovePaddle(input2, paddle2);
        
        Shake(); //shake the screen if there was a paddle impact
        setTextColors(); //cycle the text colors
        spawnPowerup(); //spawn powerups after a certain time
    }
    
    

    private void setTextColors()
    {
        float p1Speed = ((float)p1Score / scoreThreshold);
        float p2Speed = ((float)p2Score / scoreThreshold);
        p1TextColor += p1Speed/2f;
        p1Text.color = pickColor((int)p1TextColor,p1Speed+.1f);
        p2TextColor += p2Speed/2f;
        p2Text.color = pickColor((int)p2TextColor,p2Speed+.1f);
    }

    private Color pickColor(int input,float intensity)
    {
        Color output = Color.white;
        switch (input%5)
        {
            case 0:
            {
                output = Color.white;
                break;
            }
            case 1:
            {
                output = new Color(1f, 0.6f, 0.6f);
                break;
            }
            case 2:
            {
                output = Color.yellow;
                break;
            }
            case 3:
            {
                output = Color.cyan;
                break;
            }
            case 4:
            {
                output = new Color(0.5f, 1f, 0.5f);
                break;
            }
        }
        return Color.Lerp(Color.white,output, intensity);
    }

    private void spawnPowerup()
    {
        //Decrease powerup countdown timer and spawn a powerup when it reaches 0
        if (powerupCountdown > 0)
        {
            powerupCountdown--;
        }
        else if(powerupCountdown==0)
        {
            if (GameObject.FindGameObjectWithTag("PowerUp")==null)
            {
                Instantiate(powerup, new Vector3(0f, 5f, 0f), Quaternion.identity);
            }

            if (GameObject.FindGameObjectWithTag("PowerUp2")==null)
            {
                Instantiate(powerup2, new Vector3(0f, -5f, 0f), Quaternion.identity);
            }
            powerupCountdown = -1;
        }
    }

    private void StartRound()
    {
        ball.StartRound(lastWinner);
    }

    private void Shake()
    {
        float shakeTime = shakeDuration - (Time.time - shakeStartTime);
        if (shakeTime <= 0f)
        {
            mainCamera.transform.position = cameraHomePosition;
            return;
        }
        // Calculate shake intensity based on remaining shake time
        float currentIntensity = Mathf.Clamp01(shakeTime / shakeDuration);
        float shakeSpeed = 10;
        
        // Calculate shake offset using Perlin noise
        float offsetX = Mathf.PerlinNoise(Time.time * shakeSpeed, 0f) * 2f - 1f;
        float offsetY = Mathf.PerlinNoise(0f, Time.time * shakeSpeed) * 2f - 1f;
        Vector3 shakeOffset = new Vector3(offsetX, offsetY, 0f) * shakeIntensity * currentIntensity;

        // Apply shake offset to camera position
        mainCamera.transform.position = cameraHomePosition + shakeOffset;
    }
    
    public void TriggerShake(float intensity)
    {
        shakeIntensity = intensity;
        shakeDuration = Mathf.Clamp(intensity / 2.5f,0f,1f);
        shakeStartTime = Time.time;
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
        // newPosition.y = Math.Clamp(newPosition.y, paddleYMin, paddleYMax);
        //apply the transformations
        paddle.transform.position = newPosition;
    }

    public void IncrementScore(int winner)
    {
        if (winner == 1)
        {
            p1Score++;
            setGuiText(p1Text,p1Score);
            lastWinner = 1;
            Debug.Log("Left Paddle scores!");
        }
        else
        {
            p2Score++;
            setGuiText(p2Text,p2Score);
            lastWinner = 2;
            Debug.Log("Right Paddle scores!");
        }
        Debug.Log($"Current score: {p1Score} to {p2Score}");
        ball.transform.position = new Vector3(0, 0, 0);
        ball.StartRound(winner);
        _texture.material.mainTexture = textures[(p1Score + p2Score)/3];
        
        if (p1Score >= scoreThreshold || p2Score >= scoreThreshold)
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
            _texture.material.mainTexture = textures[0];
            setGuiText(p1Text,0);
            setGuiText(p2Text,0);
            p1TextColor = 0;
            p2TextColor = 0;
        }
    }

    private void setTexture(int input)
    {
        _texture.material.mainTexture = textures[input];
    }

    private void setGuiText(TextMeshProUGUI gui, int newScore)
    {
        gui.text = newScore.ToString();
    }
}