using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rat : MonoBehaviour
{
    /*** Public variables ***/
    // Movement
    public float speed = 1.0f; // Horizontal movement speed

    // Health
    public int maxHealth = 3;
    public int health { get { return currentHealth; } }

    /*** Private variables ***/
    // Player
    PlayerController player;

    // Movement
    float horizontal;
    bool goLeft;
    bool goRight;
        
    // Health
    int currentHealth;

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
        currentHealth = maxHealth;  
    }

    void FixedUpdate()
    {
        position = transform.position;
        if ((Mathf.Abs(player.getPosition.y - position.y) < 1 ) && (Mathf.Abs(player.getPosition.x - position.x) > 0.1))
        {
            horizontal = Mathf.Sign(player.getPosition.x - position.x);
            animator.SetFloat("Movement", horizontal);
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
        if (other.tag == "noFallLeft")
            goLeft = false;
        else if (other.tag == "noFallRight")
            goRight = false;
        Projectile projectile = other.gameObject.GetComponent<Projectile>();
        if (projectile != null && this.health > 0)
        {
            Destroy(other.gameObject);
            this.ChangeHealth(-1);
            if (this.health <= 0) 
                Destroy(gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Player" && player.health > 0)
        {
            player.ChangeHealth(-1);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "noFallLeft" || other.tag == "noFallRight") 
        {
            goLeft = true;
            goRight = true;
        }
    }

    public void ChangeHealth(int amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
    }
}
