using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    Rigidbody2D rigidbody2d;

    void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {

    }

    public void Launch(Vector2 direction, float force)
    {
        rigidbody2d.AddForce(direction * force);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        //we also add a debug log to know what the projectile touch
        if (other.collider.tag == "border")
        {
            rigidbody2d.constraints = RigidbodyConstraints2D.FreezeAll;
        }
        //Destroy(gameObject);
    }
}
