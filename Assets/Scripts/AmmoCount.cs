using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AmmoCount : MonoBehaviour
{
    public TextMeshProUGUI ammoDisplay;
    private int maxBullets = 250;
    private int bulletCount;

    // Start is called before the first frame update
    void Start()
    {
        bulletCount = maxBullets;
        UpdateAmmoDisplay();
    }

    public bool HasAmmo()
    {
        return bulletCount > 0;
    }

    public void UseAmmo()
    {
        bulletCount--;
        UpdateAmmoDisplay();
    }

    public void UpdateAmmoDisplay()
    {
        ammoDisplay.text = bulletCount + "/" + maxBullets ;
    }
}
