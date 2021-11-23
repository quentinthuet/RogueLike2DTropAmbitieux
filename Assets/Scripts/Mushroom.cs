using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushroom : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }
    void OnTriggerEnter2D(Collider2D other)
    {

        Debug.Log("aie");
        PlayerController player = other.gameObject.GetComponent<PlayerController>();
        if (player != null && player.health > 0)
        {
            player.health = player.health - 1;

            HealthBar.SetHealthBarValue(player.health);
        }
    }
}
