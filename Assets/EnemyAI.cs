using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Transform chaseThis;
    NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        StartCoroutine(AIThink());
    }

    // Update is called once per frame
    IEnumerator AIThink()
    {
        while (true)
        {
            agent.destination = chaseThis.position;
            yield return new WaitForSeconds(1.0f);
        }
    }
}
