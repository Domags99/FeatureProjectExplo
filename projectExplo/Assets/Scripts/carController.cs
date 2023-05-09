using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class carController : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float turnSpeed = 100f;
    public float boostMultiplier = 2f;
    public float boostDuration = 2f;
    public ParticleSystem boostParticle;

    private float boostTimeLeft = 0f;

    

    private void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        bool boostInput = Input.GetKey(KeyCode.LeftShift);

        // Move the car forward/backward
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime * verticalInput);

        // Turn the car left/right
        transform.Rotate(Vector3.up * turnSpeed * Time.deltaTime * horizontalInput);

        // the boost particle will activate when the Boost or shift is pressed
        if (boostInput && boostTimeLeft > 0f)
        {
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime * boostMultiplier);
            boostTimeLeft -= Time.deltaTime;
            if (!boostParticle.isPlaying)
            {
                boostParticle.Play();
            }
        }
        else
        {
            if (boostParticle.isPlaying)
            {
                boostParticle.Stop();
            }
        }
    }

    private void FixedUpdate()
    {
        // Reset boost time left if boost button is not pressed
        if (!Input.GetKey(KeyCode.LeftShift))
        {
            boostTimeLeft = Mathf.Min(boostTimeLeft + Time.fixedDeltaTime, boostDuration);
        }
    }
}
