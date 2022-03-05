using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ArtilleryBullet : MonoBehaviour
{

    public float speed;

    private Transform player;
    private Vector3 target;

    private ArtilleryAI artilleryAI;
    private Damageable myDamageScript;
    RaycastHit rhInfo;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        target = new Vector3(player.position.x, player.position.y, player.position.z);
        myDamageScript = GetComponent<Damageable>();
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

        if(transform.position.x == target.x && transform.position.y == target.y && transform.position.z == target.z)
        {
            DestroyArtilleryBullet();

        }

    }


    void OnTriggerEnter2D(Collider2D other)
    {

        if(other.CompareTag("Player"))
        {
            Damageable hurtScript = rhInfo.collider.GetComponentInParent<Damageable>();
            if (hurtScript)
            {
                hurtScript.TakeDamage(30.0f, artilleryAI.shotsSpawnedFrom[0].forward);
            }
            DestroyArtilleryBullet();
        }

    }


    void DestroyArtilleryBullet()
    {
        Destroy(gameObject);
    }

}
