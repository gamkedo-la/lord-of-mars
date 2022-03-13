using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickupSmall : MonoBehaviour
{

    private AmmoCount ammoCount;


    private void OnTriggerEnter(Collider other)
    {
        PlayerController controller = other.GetComponent<PlayerController>();
        if (controller)
        {
            Debug.Log("TODO- instantiate particle effect for removal");
            ammoCount = other.GetComponent<AmmoCount>();
            FindObjectOfType<AudioManager>().SoundPlay("PickUpSound");
            ammoCount.GetAmmoSmall();
            Destroy(gameObject);
        }
    }
}
