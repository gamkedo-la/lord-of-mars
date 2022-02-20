using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AmmoCount : MonoBehaviour
{
    public TextMeshProUGUI ammoDisplay;
    public float largeAmmo = 100.0f;
    public float mediumAmmo = 50.0f;
    public float smallAmmo = 25.0f;

    private float maxAmmo = 250.0f;
    private float ammoCount;

    // Start is called before the first frame update
    void Start()
    {
        ammoCount = maxAmmo;
        UpdateAmmoDisplay();
    }

    public bool HasAmmo()
    {
        return ammoCount > 0;
    }

    public void UseAmmo()
    {
        ammoCount--;
        UpdateAmmoDisplay();
    }

    public void UpdateAmmoDisplay()
    {
        ammoDisplay.text = ammoCount + "/" + maxAmmo;
    }

    public void GetAmmoLarge()
    {
        if (ammoCount <= 150)
        {
            ammoCount += largeAmmo;
        }
        else
        {
            ammoCount = maxAmmo;
        }
        UpdateAmmoDisplay();
    }

    public void GetAmmoMedium()
    {
        if (ammoCount <= 200)
        {
            ammoCount += mediumAmmo;
        }
        else
        {
            ammoCount = maxAmmo;
        }
        UpdateAmmoDisplay();
    }

    public void GetAmmoSmall()
    {
        if (ammoCount <= 225)
        {
            ammoCount += smallAmmo;
        }
        else
        {
            ammoCount = maxAmmo;
        }
        UpdateAmmoDisplay();
    }

}
