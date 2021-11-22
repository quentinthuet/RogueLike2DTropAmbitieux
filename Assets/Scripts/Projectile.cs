using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    Rigidbody2D rigidbody2d;

    void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    public void Launch(Vector2 direction, float force)
    {
        rigidbody2d.AddForce(direction * force);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.tag == "border")
        {
            rigidbody2d.constraints = RigidbodyConstraints2D.FreezeAll;
            gameObject.layer = 9;
        }
        if (other.collider.tag == "character")
        {
            Destroy(gameObject);
        }
    }
}
