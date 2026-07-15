using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public float turnSpeed = 5f;
    public float horizontalInput;
    public float verticalInput;
    public float groundOffset = 0.1f;
    public float rayDistance = 2f;
    public float gravity = -20f;
    float verticalVelocity;
    Animator animator;


    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal"); // A - D
        verticalInput = Input.GetAxis("Vertical");   // W - S

        transform.Translate(Vector3.forward * Time.deltaTime * speed * verticalInput);
        transform.Rotate(0, horizontalInput * turnSpeed * Time.deltaTime, 0);

        float moveAmount = Mathf.Abs(verticalInput);
        animator.SetFloat("Speed", moveAmount);

        StickToGround();
        ApplyGravity();
    }

    void StickToGround()
    {
        RaycastHit hit;
        Vector3 rayOrigin = transform.position + Vector3.up;

        if (Physics.Raycast(rayOrigin, Vector3.down, out hit, rayDistance))
        {
            float groundY = hit.point.y + groundOffset;

            if (transform.position.y < groundY)
            {
                Vector3 pos = transform.position;
                pos.y = groundY;
                transform.position = pos;
                verticalVelocity = 0f;
            }
        }
    }

    void ApplyGravity()
    {
        verticalVelocity += gravity * Time.deltaTime;
        Vector3 move = Vector3.up * verticalVelocity * Time.deltaTime;
        transform.position += move;
    }

}
