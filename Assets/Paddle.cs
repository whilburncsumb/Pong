using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour
{
    public float ups = 50f;

    private Rigidbody _rb;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float vValue = Input.GetAxis("Vertical");
        // Debug.Log($"vValue: {vValue}");

        Vector3 force = Vector3.forward * vValue;//(horizontalValue * ups * Time.deltaTime);

        _rb.AddForce(force, ForceMode.Force);
        // Transform t = GetComponent<Transform>();
        // t.position += Vector3.right * (horizontalValue * ups * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision other)
    {
        // Debug.Log($"We hit {other.gameObject.name}");

        BoxCollider bc = GetComponent<BoxCollider>();
        Bounds bounds = bc.bounds;
        float maxX = bounds.max.x;
        float minX = bounds.min.x;
        
        // Debug.Log($"x pos of ball is ");
        
        Quaternion rotation = Quaternion.Euler(0f, 0f, -60f);
        Vector3 bounceDir = rotation * Vector3.up;
        Rigidbody cb = other.rigidbody;
        // cb.AddForce(new Vector3(1000f,1000f,0f),ForceMode.Force);
        cb.AddForce(bounceDir * 1000, ForceMode.Force);
    }
}
