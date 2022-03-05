using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    private Damageable damageable;


    private void OnTriggerEnter(Collider other)
    {
        PlayerController controller = other.GetComponent<PlayerController>();
        if (controller)
        {
            Debug.Log("TODO- instantiate particle effect for removal");
            damageable = other.GetComponent<Damageable>();
            damageable.HealDamage();
            Destroy(gameObject);
        }
    }

}
