using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 1.0f;

    public float jumpForce = 15.0f;

    public float forceArrow = 300.0f;

    public GameObject arrowPrefab;

    float horizontal;
    bool isGrounded;

    Animator animator;

    Rigidbody2D rigidbody2d;

    float lookX = 1.0f; 



    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        isGrounded = false;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown("joystick button 0")) && isGrounded)
        {
            GetComponent<Rigidbody2D>().AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            animator.SetBool("OnGround", false);
            animator.SetTrigger("Jump");
        }

        if (Input.GetKeyDown("c"))
        {
            Launch();
        }

    }

void FixedUpdate()
    {
        horizontal = Input.GetAxis("Horizontal");
        Vector2 position = transform.position;
        position.x = position.x + 3.0f * speed * horizontal * Time.deltaTime;
        transform.position = position;
        if (!Mathf.Approximately(horizontal, 0.0f))
        {
            lookX = horizontal / Mathf.Abs(horizontal);
        }
        animator.SetFloat("LookX", lookX);

    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.tag == "ground" && other.transform.position.y < transform.position.y)
        {
            isGrounded = true;
            animator.SetBool("OnGround",true);
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.collider.tag == "ground")
        {
            isGrounded = false;
            animator.SetBool("OnGround", false);
        }
    }

    void Launch()
    {
        GameObject arrowObject = Instantiate(arrowPrefab, rigidbody2d.position, Quaternion.identity);
        Arrow arrow = arrowObject.GetComponent<Arrow>();
        arrow.Launch(new Vector2(lookX,0), forceArrow);
    }
}

