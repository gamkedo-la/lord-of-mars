using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    
    private AmmoCount ammoCount;


    private void OnTriggerEnter(Collider other)
    {
        PlayerController controller = other.GetComponent<PlayerController>();
        if (controller)
        {
            Debug.Log("TODO- instantiate particle effect for removal");
            ammoCount = other.GetComponent<AmmoCount>();
            ammoCount.GetAmmoLarge();
            Destroy(gameObject);
        }
    }
}
