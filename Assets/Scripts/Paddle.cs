using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour
{
    public int timer;

    public float speedMultiplier;

    private float scaleSpeed;

    private float minScale = 5f;

    private float maxScale = 10f;

    public bool activeAI;
    
    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
        speedMultiplier = 1;
        scaleSpeed = 3f;
    }
    
    void FixedUpdate()
    {
        if (timer > 0)
        {
            transform.localScale += new Vector3(0f,scaleSpeed*Time.deltaTime, 0f);
            if (transform.localScale.y > maxScale)
            {
                transform.localScale = new Vector3(1f,maxScale,1f);
            }
        }
        else
        {
            transform.localScale -= new Vector3(0f,scaleSpeed*Time.deltaTime, 0f);
            if (transform.localScale.y < minScale)
            {
                transform.localScale = new Vector3(1f,minScale,1f);
            }
        }
        timer--;
    }

    public void powerUp()
    {
        timer = 500;
        speedMultiplier = 1.5f;
    }
}
