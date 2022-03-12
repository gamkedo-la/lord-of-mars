using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ArtilleryBullet : MonoBehaviour
{
    public AudioManager audioManager;

    public GameObject blastEffect;
    public float speed;

    private Vector3 dir;

    private ArtilleryAI artilleryAI;
    private Damageable myDamageScript;
    RaycastHit rhInfo;

    void Start()
    {
        Transform player = GameObject.FindGameObjectWithTag("Player").transform;

        dir = (player.position - transform.position).normalized;
        myDamageScript = GetComponent<Damageable>();
    }

    void Update()
    {
        transform.position += dir * speed * Time.deltaTime;

    }

    private void OnTriggerEnter(Collider other)
    {
        Explode();
    }
    void OnCollisionEnter(Collision coll)
    {
        Explode();
    }

    void Explode()
    {
        Debug.Log("rocket boom");
        GameObject.Instantiate(blastEffect, transform.position, Quaternion.identity);
        Collider[] blastedRadius = Physics.OverlapSphere(transform.position, 10.0f);
        for (int i = 0; i < blastedRadius.Length; i++)
        {
            Damageable damageScript = blastedRadius[i].GetComponent<Damageable>(); //might need to check for children of parent?
            if (damageScript) //todo confirm line of sight with raycast 
            {
                Debug.Log(blastedRadius[i].gameObject.name);
                Vector3 hurtVect = (blastedRadius[i].transform.position - transform.position).normalized;
                damageScript.TakeDamage(30.0f, hurtVect);
                FindObjectOfType<AudioManager>().SoundPlay("RocketExplosion");
            }
        } //end of for 
        Destroy(gameObject);

    }

}
