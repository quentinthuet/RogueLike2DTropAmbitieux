using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stain : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "puddle")
        {
            Destroy(gameObject);
        }
        else if (other.tag == "Player")
        {
            PlayerController player = other.gameObject.GetComponent<PlayerController>();
            if (player != null && player.health > 0)
            {
                player.health = player.health - 1;
                HealthBar.SetHealthBarValue(player.health);
            }
        }
    }
}
