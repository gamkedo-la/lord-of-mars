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
    private float timeBtwHopTurns = 1.5f;

    Quaternion startRotation;
    public Quaternion goalRotation;
    float groundHeight;
    float hopStartTime = -5.0f;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        StartCoroutine(AutoFire());
        StartCoroutine(HopTurn());
    }

    private void Update()
    {
        float hopProgress = Time.timeSinceLevelLoad - hopStartTime;
        hopProgress *= 3.0f;
        if(hopProgress > 0.0f && hopProgress < 1.0f)
        {
            transform.rotation = Quaternion.Slerp(startRotation, goalRotation, hopProgress);
            Vector3 hopHeight = transform.position;
            float midDist = 1.0f - 2.0f * Mathf.Abs(0.5f - hopProgress);
            midDist *= midDist;
            hopHeight.y = groundHeight + midDist * 0.5f;
            transform.position = hopHeight;
        }
    }

    IEnumerator HopTurn()
    {
        Vector3 sameHeight;
        while (true)
        {
            yield return new WaitForSeconds(timeBtwHopTurns);
            sameHeight = player.transform.position;
            sameHeight.y = transform.position.y;
            // transform.LookAt(sameHeight);
            startRotation = transform.rotation;
            goalRotation = Quaternion.LookRotation(sameHeight - transform.position);
            groundHeight = sameHeight.y;
            hopStartTime = Time.timeSinceLevelLoad;
        }
    }


    IEnumerator AutoFire()
    {
        while(true)
        {
            yield return new WaitForSeconds(startTimeBtwShots);

            for (int i = 0; i < 2; i++)
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
                if (shootFromNext >= shotsSpawnedFrom.Length)
                {
                    shootFromNext = 0;
                }
                if (shouldShoot)
                {
                    Quaternion toPlayer = Quaternion.LookRotation(player.position - shootFrom);
                    Instantiate(bullet, shootFrom, toPlayer);
                }
            }
        }


    }


}