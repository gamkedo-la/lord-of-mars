using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    public float maxHealth = 100.0f;
    public float health = 0.0f;
    private DamageFlash dFlash;
    private HealthCount healthCount;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        dFlash = gameObject.GetComponent<DamageFlash>();
        healthCount = GetComponent<HealthCount>();
        if (healthCount)
        {
            healthCount.UpdateHealthDisplay();
        }
    }

   
    
    public void TakeDamage(float amt, Vector3 shotDir)
    {
        if(isDead() && amt > 0.0f)
        {
            Debug.Log("Damage ignored, already dead " + gameObject.name);
            return;
        }
        health -= amt;
        if (healthCount)
        {
            healthCount.UpdateHealthDisplay();
        }
        if (dFlash)
        {
            dFlash.TakeShotFrom(shotDir);
        }
        if(isDead())
        {
            Debug.Log("die " + gameObject.name);
            health = 0.0f;
            if (healthCount)
            {
                healthCount.UpdateHealthDisplay();
            }
            else
            {
                Destroy(gameObject);
            } 
        }
        else
        {
            //Debug.Log(gameObject.name + " health is now " + (health / maxHealth));
        }
    }

    public bool isDead()
    {
        return health <= 0.0f;
    }

    public bool isLowOnHealth()
    {
        return health <= 0.4f * maxHealth && (isDead() == false);
    }

}
