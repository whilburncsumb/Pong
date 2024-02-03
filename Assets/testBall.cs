using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testBall : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private float m_thrust = 10f;
    // Start is called before the first frame update
    void Start()
    {
        _rigidbody.AddForce(transform.right * m_thrust);
        Debug.Log($"Starting test ball... velocity is: {_rigidbody.velocity}");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
