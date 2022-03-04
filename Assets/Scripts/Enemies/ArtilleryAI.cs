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
    
    public float startTimeBtwShots;
    public Transform [] shotsSpawnedFrom;
    private int shootFromNext = 0;
    private float timeBtwHopTurns = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        StartCoroutine(AutoFire());
        StartCoroutine(HopTurn());
    }

    IEnumerator HopTurn()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeBtwHopTurns);
            Vector3 sameHeight = player.transform.position;
            sameHeight.y = transform.position.y;
            transform.LookAt(sameHeight);
        }
    }


    IEnumerator AutoFire()
    {
        while(true)
        {
            yield return new WaitForSeconds(startTimeBtwShots);
        
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
        }

    }


}