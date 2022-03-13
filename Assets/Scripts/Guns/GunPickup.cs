using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunPickup : MonoBehaviour
{
    public PlayerController.WeaponTypes pickupType;


    private void OnTriggerEnter(Collider other)
    {
        PlayerController controller = other.GetComponent<PlayerController>();
        if(controller)
        {
            Debug.Log("TODO- instantiate particle effect for removal");
            controller.ObtainGun(pickupType);
            FindObjectOfType<AudioManager>().SoundPlay("PickUpSound");
            Destroy(gameObject);
        }
    }


}
