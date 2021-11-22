using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{


    /*** Public variables ***/


    // Movement
    public float speed = 1.0f; // Horizontal movement speed

    // Jump
    public float jumpForce = 15.0f; // Jump force
    public float jumpDelay = 0.5f; // Delay between two jumps (s)

    // Launch projectile
    public float forceProjectile = 300.0f; // Projectile launch force
    public float timeLaunch = 1.0f; // Key hold time for projectile launch
    public GameObject projectilePrefab; // Projectile prefab

    // Bow
    public GameObject bowPrefab; // Bow prefab

    // Controller
    public bool controller = true; // Is the player using a controller ?


    /*** Private variables ***/


    // Axis
    float horizontal; // Horizontal axis value
    float vertical; // Vertical axis value

    // Jump
    bool isGrounded; // Is the player touching a ground platform ?
    bool jumped; // Has the player jumped ?

    // Projectile launch
    Vector3 relativeMouse; // Mouse position relative to player
    Vector2 direction; // Projectile launch direction
    float angle; // Target angle
    float downTime; // Last time the launch key has been pressed

    // Bow
    Vector2 bowRelativePos = new Vector2(0.5f,0);
    GameObject bowObject;
    Bow bow;
    Animator bowAnimator;

    // Animator
    Animator animator; // Animator
    float lookX; // Look direction

    // Others
    Vector2 position; // Player position
    Rigidbody2D rigidbody2d; // Player rigidbody


    /*** Keyboard bindings ***/

    string jumpKeyKeyboard = "space";
    string launchButtonKeyboard = "mouse 0";

    /*** Controller bindings ***/

    string jumpKeyController = "joystick button 0";
    string launchKeyController = "joystick button 1";

    /*** Bindings ***/

    string jumpKey;
    string launchKey;

    void Start()
    {
        /*** Initialisations ***/
        isGrounded = false;
        jumped = false;

        downTime = 0;

        lookX = 1.0f;
        direction = new Vector2(lookX, 0f);

        animator = GetComponent<Animator>();

        rigidbody2d = GetComponent<Rigidbody2D>();

        // Controls initialisation
        if (controller)
        {
            jumpKey = jumpKeyController;
            launchKey = launchKeyController;
        }
        else
        {
            jumpKey = jumpKeyKeyboard;
            launchKey = launchButtonKeyboard;
        }

        // Equiping 
        bowObject = Instantiate(bowPrefab, rigidbody2d.position + bowRelativePos, Quaternion.identity);
        bow = bowObject.GetComponent<Bow>();
        bowAnimator = bow.GetComponent<Animator>();
    }

    void Update()
    {
        // Axis values getter
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        // Target angle and direction getter, look direction setter
        if (controller) // If controller, we update direction only if the joystick is not on neutral
        {
            if (!(Mathf.Approximately(horizontal, 0.0f) && Mathf.Approximately(vertical, 0.0f)))
            {
                direction.Set(horizontal, vertical);
                direction.Normalize();
                angle = Mathf.Atan2(vertical, horizontal) * Mathf.Rad2Deg;
            }
            if (!Mathf.Approximately(horizontal, 0.0f))
            {
                lookX = horizontal / Mathf.Abs(horizontal); ;
            }
        }
        else
        { // If controller, we update direction with the cursor position relatively to the player
            relativeMouse = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
            direction.Set(relativeMouse.x, relativeMouse.y);
            direction.Normalize();
            angle = Mathf.Atan2(relativeMouse.y, relativeMouse.x) * Mathf.Rad2Deg;
            if (!Mathf.Approximately(relativeMouse.x, 0.0f))
            {
                lookX = relativeMouse.x / Mathf.Abs(relativeMouse.x);
            }
        }

        // Jump
        if ((Input.GetKey(jumpKey) || Input.GetKey("joystick button 0")) && isGrounded && !jumped && !(Input.GetKey(launchKey) || Input.GetKey("joystick button 1")))
        {
            Jump();
            jumped = true;
            StartCoroutine(SpamBlockco());
        }

        // Launch projectile if key is hold
        if (Input.GetKeyDown(launchKey) || Input.GetKeyDown("joystick button 1"))
        {
            downTime = Time.time;
            bowAnimator.SetTrigger("Launch");
        }

        if (Input.GetKey(launchKey) || Input.GetKey("joystick button 1"))
        {
            bow.transform.rotation = Quaternion.Euler(0, 0, angle);
            if (Time.time - downTime > timeLaunch)
            { 
                downTime = Time.time;
                Launch();
            }
        }
        else
        {
            bow.transform.rotation = Quaternion.Euler(0, 0, 90f*(lookX-1));
        }
        
        if (Input.GetKeyUp(launchKey) || Input.GetKeyUp("joystick button 1"))
        {
            bowAnimator.SetTrigger("CancelLaunch");
        }

        // Sending look direction to animator
        animator.SetFloat("LookX", lookX);

        // Crossbow position
        bow.transform.position = transform.position + lookX * new Vector3(bowRelativePos.x, bowRelativePos.y, 0);

    }

    void FixedUpdate()
    {
        // Position setter
        if (!(isGrounded && (Input.GetKey(launchKey) || Input.GetKey("joystick button 1"))))
        {
            position = transform.position;
            position.x = position.x + 3.0f * speed * horizontal * Time.deltaTime;
            transform.position = position;
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        // Checking if player is on ground and if he is on top of a platform
        if (other.collider.tag == "ground" && other.enabled)
        {
            isGrounded = true;
            animator.SetBool("OnGround", true);
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

    // Adding force to jump, triggering jump animation
    void Jump()
    {
        GetComponent<Rigidbody2D>().AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        animator.SetBool("OnGround", false);
        animator.SetTrigger("Jump");
    }

    // Creating a projectile and launching it
    void Launch()
    {
        GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2d.position + lookX * bowRelativePos, Quaternion.Euler(0, 0, angle));
        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        Physics2D.IgnoreCollision(this.GetComponent<Collider2D>(), projectile.GetComponent<Collider2D>());
        projectile.Launch(direction, forceProjectile);
    }

    // Jump delay
    public IEnumerator SpamBlockco()
    {
        if (jumped == true)
        {
            yield return new WaitForSeconds(jumpDelay);
        }
        yield return null;
        jumped = false;
    }
}

