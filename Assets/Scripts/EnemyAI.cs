using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Transform chaseThis;
    public Transform[] gunList;
    public GameObject muzzleEffect;
    NavMeshAgent agent;
    int fireNext = 0;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        StartCoroutine(AIThink());
        StartCoroutine(RateOfFire());
    }

    IEnumerator AIThink()
    {
        while (true)
        {
            agent.destination = chaseThis.position;
            yield return new WaitForSeconds(1.0f);
        }
    }

    IEnumerator RateOfFire()
    {
        while (true)
        {
            // need to do: shoot raycast to target
            bool shouldShoot = false;
            float angToTarget = Quaternion.Angle(transform.rotation, Quaternion.LookRotation(chaseThis.position - transform.position));
            //assesses whether player is within line of sight
            shouldShoot = angToTarget < 10.0f;
            if (shouldShoot)
            {
                GameObject.Instantiate(muzzleEffect, gunList[fireNext].position, gunList[fireNext].rotation);
                fireNext++;
                if (fireNext >= gunList.Length)
                {
                    fireNext = 0;
                }
            }
            yield return new WaitForSeconds(0.5f);
        }
    }
}
