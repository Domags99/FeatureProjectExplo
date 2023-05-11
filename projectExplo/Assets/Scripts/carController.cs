using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class carController : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float boostSpeed = 20f;
    public float jumpForce = 5f;
    public float rotationSpeed = 50f;
    public float airRollSpeed = 50f;
    public ParticleSystem boostParticles;
    public Transform cameraTransform;

    private Rigidbody rb;
    private bool isGrounded = false;
    private bool isBoosting = false;
    private Vector3 cameraStartPosition;
    private Quaternion cameraStartRotation;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        boostParticles.Stop();

        // Store initial camera position and rotation
        cameraStartPosition = cameraTransform.localPosition;
        cameraStartRotation = cameraTransform.localRotation;
    }


void Update()
{
    // Boost function
    if (Input.GetKeyDown(KeyCode.LeftShift))
    {
        isBoosting = true;
        boostParticles.Play();

        // Pan camera back when boosting
        cameraTransform.localPosition -= Vector3.forward * 3f;
    }
    if (Input.GetKeyUp(KeyCode.LeftShift))
    {
        isBoosting = false;
        boostParticles.Stop();

        // Pan camera forward when not boosting
        cameraTransform.localPosition = cameraStartPosition;
        cameraTransform.localRotation = cameraStartRotation;
    }

    // Jump function
    if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    // Rotation function
    if (!isGrounded)
    {
        float rotation = 0f;
        if (Input.GetKey(KeyCode.I))
        {
            rotation = rotationSpeed;
        }
        else if (Input.GetKey(KeyCode.K))
        {
            rotation = -rotationSpeed;
        }
        rb.rotation *= Quaternion.Euler(rotation * Time.deltaTime, 0f, 0f);
    }

    // Air roll function
    if (!isGrounded)
    {
        if (Input.GetKey(KeyCode.Q))
        {
            rb.rotation *= Quaternion.Euler(0f, 0f, airRollSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.E))
        {
            rb.rotation *= Quaternion.Euler(0f, 0f, -airRollSpeed * Time.deltaTime);
        }
    }
}

    void FixedUpdate()
    {
        // Movement function
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        Vector3 moveDirection = new Vector3(moveHorizontal, 0f, moveVertical);

        if (!isGrounded && moveVertical < 0)
        {
            moveDirection *= -1;
        }

        rb.AddForce(moveDirection.normalized * moveSpeed, ForceMode.Acceleration);

    // Rotation function
    float turn = moveHorizontal * rotationSpeed * Time.fixedDeltaTime;
    Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);
    rb.MoveRotation(rb.rotation * turnRotation);

    // Boost function
    if (isBoosting)
    {
        rb.AddForce(transform.forward * boostSpeed, ForceMode.Acceleration);
    }

    // Brake function
    if (Input.GetKey(KeyCode.Space))
    {
        Vector3 brakeForce = -rb.velocity.normalized * moveSpeed;
        rb.AddForce(brakeForce, ForceMode.Acceleration);
    }
 }

void OnCollisionStay(Collision collision)
{
    // Ground check
    if (collision.gameObject.CompareTag("Ground"))
    {
        isGrounded = true;
    }
}

void OnCollisionExit(Collision collision)
{
    // Ground check
    if (collision.gameObject.CompareTag("Ground"))
    {
        isGrounded = false;
    }
}
}
