using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HealthCount : MonoBehaviour
{
    public TextMeshProUGUI healthDisplay;
    private Damageable damageable;

    // Start is called before the first frame update
    void Start()
    {
        damageable = GetComponent<Damageable>();
        UpdateHealthDisplay();
    }


    public void UpdateHealthDisplay()
    {
        healthDisplay.text = damageable.health + "/" + damageable.maxHealth;
    }

}
