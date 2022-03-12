using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    private Transform chaseThis;
    public Transform[] gunList;
    public GameObject muzzleEffect;
    public LayerMask bulletMask;
    NavMeshAgent agent;
    int fireNext = 0;

    public enum EnemyAIMode { Nearest, Stand, AimFromCover, Rush};
    public EnemyAIMode currentMode = EnemyAIMode.Nearest;
    public EnemyNodeData myWaypoint;

    private Damageable myDamageScript;
    private Damageable playerDamageScript;

    // Start is called before the first frame update
    void Start()
    {
        chaseThis = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        myDamageScript = GetComponent<Damageable>();
        playerDamageScript = chaseThis.GetComponent<Damageable>();
        if(playerDamageScript == null)
        {
            Debug.Log("Warning: Player Damage Script not found");
        }
        StartCoroutine(AIThink());
        StartCoroutine(RateOfFire());
    }

    private void FixedUpdate() // so that we can use slerp without framerate inconsistency 
    {
        Vector3 tempVector;
        /*switch (currentMode)
        {
            case EnemyAIMode.Nearest: //navmesh handles this 
                break;
            case EnemyAIMode.Stand: //do nothing (helpful for testing)
                break;
            case EnemyAIMode.AimFromCover: //aim at player 
            */
                //transform.LookAt(chaseThis);
                tempVector = chaseThis.transform.position;
                tempVector.y = transform.position.y; //same height as us, so we only turn side to side 
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(tempVector - transform.position), 0.1f);
              /*  break;
            case EnemyAIMode.Rush:
                break;
            default:
                Debug.Log("unhandled AI mode in fixed update " + currentMode);
                break;
        } */
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
            if(playerDamageScript.isLowOnHealth())
            {
                currentMode = EnemyAIMode.Rush;
            }
            switch (currentMode)
            {
                case EnemyAIMode.Nearest:
                    if (coverPoints.Length > 0)
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
                    if(myWaypoint != null)
                    {
                        if(Vector3.Distance(transform.position, myWaypoint.transform.position) < 1.0f)
                        {
                            Debug.Log("aiming from cover");
                            agent.enabled = false;
                            currentMode = EnemyAIMode.AimFromCover;
                        }
                    }
                    break;
                case EnemyAIMode.Stand:
                    agent.isStopped = true;
                    break;
                case EnemyAIMode.AimFromCover:
                    if (agent.enabled)
                    {
                        agent.isStopped = true;
                    }
                    //to do if player is to close to me, flee  
                    break;
                case EnemyAIMode.Rush:
                    agent.enabled = true;
                    agent.destination = chaseThis.position;
                    agent.isStopped = false;
                    break;
                default:
                    Debug.Log("unhandled AI mode in AIThink " + currentMode);
                    break;
            }

            yield return new WaitForSeconds(Random.Range(1.0f, 2.0f));
        }
    }

    IEnumerator RateOfFire()
    {
        RaycastHit rhInfo;
        while (true)
        {
            bool shouldShoot = false;
            Quaternion gunTargetAngle = Quaternion.LookRotation(chaseThis.position - gunList[fireNext].position);
            Quaternion facingAngle = Quaternion.LookRotation(chaseThis.position - transform.position);
            float angToTarget = Quaternion.Angle(transform.rotation, facingAngle);
            //assesses whether player is within line of sight
            bool goodAngle = angToTarget < 45.0f;
            bool goodDistance = (Vector3.Distance(chaseThis.position, transform.position) < 40.0f);
            if(goodAngle && goodDistance)
            {
                Vector3 gunToPlayer = chaseThis.position - gunList[fireNext].position;
                if (Physics.Raycast(gunList[fireNext].position, gunToPlayer, out rhInfo, 200.0f, bulletMask)) //line of sight test 
                {
                    if (rhInfo.collider.gameObject.CompareTag("Player"))
                    {
                        shouldShoot = true;
                    }
                }
            }

            if (shouldShoot)
            {
                gunList[fireNext].rotation = Quaternion.Slerp(gunList[fireNext].rotation,
                    gunTargetAngle,
                    0.5f); //percent angle to correct
                GameObject.Instantiate(muzzleEffect, gunList[fireNext].position, gunList[fireNext].rotation);
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
