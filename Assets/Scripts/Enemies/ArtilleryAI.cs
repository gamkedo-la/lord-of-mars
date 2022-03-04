using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ArtilleryAI : MonoBehaviour
{

    public float speed;
    public float stoppingDistance;
    public float retreatDistance;
    public LayerMask bulletMask;

    public GameObject bullet;
    public Transform player;

    private float timeBtwShots;
    public float startTimeBtwShots;
    public Transform [] shotsSpawnedFrom;
    private int shootFromNext = 0;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        timeBtwShots = startTimeBtwShots;
    }

    // Update is called once per frame
    void Update()
    {
        /*if (Vector3.Distance(transform.position, player.position) > stoppingDistance)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        }
        else if (Vector3.Distance(transform.position, player.position) < stoppingDistance && Vector3.Distance(transform.position, player.position) > retreatDistance)
        {
            transform.position = this.transform.position;
        }
        else if (Vector3.Distance(transform.position, player.position) < retreatDistance)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.position, -speed * Time.deltaTime);
        }*/
        
        if (timeBtwShots < -0)
        {
            Vector3 shootFrom = shotsSpawnedFrom[shootFromNext].position;
            bool shouldShoot = false;
            RaycastHit rhInfo;
            Transform chaseThis = player.transform;
            Quaternion facingAngle = Quaternion.LookRotation(chaseThis.position - shotsSpawnedFrom[shootFromNext].position);
            float angToTarget = Quaternion.Angle(shotsSpawnedFrom[shootFromNext].rotation, facingAngle);
            //assesses whether player is within line of sight
            bool goodAngle = angToTarget < 45.0f;
            bool goodDistance = (Vector3.Distance(chaseThis.position, transform.position) < 40.0f);
            if (goodAngle && goodDistance)
            {
                Vector3 gunToPlayer = chaseThis.position - shotsSpawnedFrom[shootFromNext].position;
                if (Physics.Raycast(shotsSpawnedFrom[shootFromNext].position, gunToPlayer, out rhInfo, 200.0f, bulletMask)) //line of sight test 
                {
                    if (rhInfo.collider.gameObject.CompareTag("Player"))
                    {
                        shouldShoot = true;
                    }
                }
            }
            shootFromNext++;
            if(shootFromNext >= shotsSpawnedFrom.Length)
            {
                shootFromNext = 0;
            }
            if(shouldShoot)
            {
                Quaternion toPlayer = Quaternion.LookRotation(player.position - shootFrom);
                Instantiate(bullet, shootFrom, toPlayer);
            }
            timeBtwShots = startTimeBtwShots;
        }
        else
        {
            timeBtwShots -= Time.deltaTime;
        }

    }
}