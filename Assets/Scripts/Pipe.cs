using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pipe : MonoBehaviour
{
    /*** Public variables ***/


    // Stain 
    public GameObject stainPrefab;

    // Delay
    public float delay = 5.0f;


    /*** Private variables ***/


    // Delay
    float lastStain;

    // Others
    Rigidbody2D rigidbody2d;



    // Start is called before the first frame update
    void Start()
    {
        lastStain = Time.time;
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - lastStain > delay)
        {
            lastStain = Time.time;
            GameObject stainObject = Instantiate(stainPrefab, rigidbody2d.position + new Vector2(0,-0.5f), Quaternion.identity);
        }

    }
}
