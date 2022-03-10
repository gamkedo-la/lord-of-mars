using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CrowdAI : MonoBehaviour
{
    public Transform[] waypointList;
    public int currentWaypoint = 0;
    private NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        agent = gameObject.GetComponent<NavMeshAgent>();
        agent.destination = waypointList[currentWaypoint].position;
    }

    // Update is called once per frame
    void Update()
    {
        float distToGO = Vector3.Distance(transform.position, waypointList[currentWaypoint].position);
        if (distToGO < 5.0f)
        {
            currentWaypoint++;
            if(currentWaypoint >= waypointList.Length)
            {
                currentWaypoint = 0;
            }
            agent.destination = waypointList[currentWaypoint].position;
        }
    }
}
