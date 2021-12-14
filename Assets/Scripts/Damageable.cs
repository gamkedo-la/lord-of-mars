using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    public float maxHealth = 100.0f;
    private float health = 0.0f;
    private DamageFlash dFlash;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        dFlash = gameObject.GetComponent<DamageFlash>();
    }

    
    public void TakeDamage(float amt, Vector3 shotDir)
    {
        if(isDead() && amt > 0.0f)
        {
            Debug.Log("Damage ignored, already dead " + gameObject.name);
            return;
        }
        health -= amt;
        if(dFlash)
        {
            dFlash.TakeShotFrom(shotDir);
        }
        if(isDead())
        {
            Debug.Log("die " + gameObject.name);
            health = 0.0f;
            Destroy(gameObject);
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

}
