using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Transform chaseThis;
    public Transform[] gunList;
    public GameObject muzzleEffect;
    public LayerMask bulletMask;
    NavMeshAgent agent;
    int fireNext = 0;

    public enum EnemyAIMode { Nearest, Stand};
    public EnemyAIMode currentMode = EnemyAIMode.Nearest;
    public EnemyNodeData myWaypoint;

    private Damageable myDamageScript;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        myDamageScript = GetComponent<Damageable>();
        StartCoroutine(AIThink());
        StartCoroutine(RateOfFire());

    }

    public bool isAlive()
    {
        return myDamageScript.isDead() == false;
    }
    IEnumerator AIThink()
    {
        GameObject[] coverPoints = GameObject.FindGameObjectsWithTag("CoverNode");
        while (true)
        {
            //agent.destination = chaseThis.position;
            if(coverPoints.Length > 0)
            {
                int bestIdx = -1;
                float bestIdxScore = 999999.0f;
                EnemyNodeData end;
                for (int i = 0; i < coverPoints.Length; i++)
                {
                    float considerScore = Vector3.Distance(transform.position, coverPoints[i].transform.position);
                    end = coverPoints[i].GetComponent<EnemyNodeData>();
                    if (considerScore < bestIdxScore && (end.beingUsedBy == null || end.beingUsedBy == this))
                    { 
                        bestIdx = i;
                        bestIdxScore = considerScore;
                    }
                }
                if (bestIdx != -1)
                {
                    if (myWaypoint != null)
                    {
                        myWaypoint.beingUsedBy = null;
                    }
                    myWaypoint = coverPoints[bestIdx].GetComponent<EnemyNodeData>();
                    myWaypoint.beingUsedBy = this;
                    agent.destination = myWaypoint.transform.position;
                }   
            }
            yield return new WaitForSeconds(1.0f);
        }
    }

    IEnumerator RateOfFire()
    {
        while (true)
        {
            bool shouldShoot = false;
            Quaternion gunTargetAngle = Quaternion.LookRotation(chaseThis.position - gunList[fireNext].position);
            Quaternion facingAngle = Quaternion.LookRotation(chaseThis.position - transform.position);
            float angToTarget = Quaternion.Angle(transform.rotation, facingAngle);
            //assesses whether player is within line of sight
            shouldShoot = angToTarget < 45.0f;
            if (shouldShoot)
            {
                gunList[fireNext].rotation = Quaternion.Slerp(gunList[fireNext].rotation,
                    gunTargetAngle,
                    0.5f); //percent angle to correct
                GameObject.Instantiate(muzzleEffect, gunList[fireNext].position, gunList[fireNext].rotation);
                RaycastHit rhInfo;
                if (Physics.Raycast(gunList[fireNext].position, gunList[fireNext].forward, out rhInfo, 200.0f, bulletMask))
                {
                    //Debug.Log(rhInfo.collider.name);
                    Damageable hurtScript = rhInfo.collider.GetComponentInParent<Damageable>();
                    if (hurtScript)
                    {
                        hurtScript.TakeDamage(10.0f, gunList[fireNext].forward);
                    }
                    //should be hit effect, muzzleEffect is placeholder
                    Instantiate(muzzleEffect, rhInfo.point + rhInfo.normal * 0.1f, Quaternion.identity);
                }
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
