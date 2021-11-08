using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 1.0f;

    public float jumpForce = 15.0f;

    float horizontal;
    bool isGrounded;

    void Start()
    {
        isGrounded = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            GetComponent<Rigidbody2D>().AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        }
    }

void FixedUpdate()
    {
        horizontal = Input.GetAxis("Horizontal");
        Vector2 position = transform.position;
        position.x = position.x + 3.0f * speed * horizontal * Time.deltaTime;
        transform.position = position;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.tag == "ground")
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.collider.tag == "ground")
        {
            isGrounded = false;
        }
    }
}

