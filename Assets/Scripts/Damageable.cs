using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    private const float MAX_HEALTH = 100.0f;
    public float health = 0.0f;
    

    // Start is called before the first frame update
    void Start()
    {
        health = MAX_HEALTH;
    }

    
    public void TakeDamage(float amt)
    {
        if(health <= 0.0f && amt > 0.0f)
        {
            Debug.Log("Damage ignored, already dead " + gameObject.name);
            return;
        }
        health -= amt;
        if(health <= 0.0f)
        {
            Debug.Log("die " + gameObject.name);
            health = 0.0f;
            Destroy(gameObject);
        }
        else
        {
            Debug.Log(gameObject.name + " health is now " + (health / MAX_HEALTH));
        }
    }

}
