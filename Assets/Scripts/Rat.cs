using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rat : MonoBehaviour
{
    /*** Public variables ***/
    // Movement
    public float speed = 1.0f; // Horizontal movement speed

    /*** Private variables ***/
    // Player
    PlayerController player;

    // Movement
    float horizontal;
    bool goLeft;
    bool goRight;

    // Animator
    Animator animator;

    // Others
    Vector2 position; // Player position
    Rigidbody2D rigidbody2d; // Player rigidbody

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        player = FindObjectOfType<PlayerController>();
        goLeft = true;
        goRight = true;
    }

    void FixedUpdate()
    {
        position = transform.position;
        if ((Mathf.Abs(player.getPosition.y - position.y) < 1 ) && (Mathf.Abs(player.getPosition.x - position.x) > 0.1))
        {
            horizontal = Mathf.Sign(player.getPosition.x - position.x);
            animator.SetFloat("Movement", horizontal);
            Debug.Log("Left? " + goLeft + ", Right? " + goRight + ", horizontal: " + horizontal);
            if (!((!goLeft && (horizontal < 0)) || (!goRight && (horizontal > 0))))
            {
                animator.SetBool("Moving", true);
                position.x = position.x + horizontal * speed * Time.deltaTime;
                transform.position = position;
            }
            else
                animator.SetBool("Moving", false);
        }
        else
            animator.SetBool("Moving", false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        goLeft = true;
        goRight = true;
        if (other.tag == "noFallLeft")
            goLeft = false;
        else if (other.tag == "noFallRight")
            goRight = false;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        goLeft = true;
        goRight = true;
    }
}
