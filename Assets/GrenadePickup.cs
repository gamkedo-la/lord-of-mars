using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadePickup : MonoBehaviour
{
    private AmmoCount ammoCount;

    private void OnTriggerEnter(Collider other)
    {
        PlayerController controller = other.GetComponent<PlayerController>();
        if (controller)
        {
            ammoCount = other.GetComponent<AmmoCount>();
            ammoCount.GetGrenades();
            Destroy(gameObject);
        }
    }
}
