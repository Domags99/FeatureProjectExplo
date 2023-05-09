using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarJump : MonoBehaviour
{
    public float jumpforce = 10f;
    Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector3.up * jumpforce, (ForceMode)ForceMode2D.Impulse);

        }

    }
}
