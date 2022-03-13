using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CrowdAI : MonoBehaviour
{
    public Transform[] waypointList;
    public int currentWaypoint = 0;
    private NavMeshAgent agent;
    private Vector3 goToSameY;

    // Start is called before the first frame update
    void Start()
    {
        agent = gameObject.GetComponent<NavMeshAgent>();
        GoToAtSameY();
    }

    // Update is called once per frame
    void Update()
    {
        float distToGO = Vector3.Distance(transform.position, goToSameY);
        //Debug.Log(currentWaypoint + " " + distToGO);
        if (distToGO < 5.0f)
        {
            currentWaypoint++;
            if(currentWaypoint >= waypointList.Length)
            {
                currentWaypoint = 0;
            }

            GoToAtSameY();
        }
    }

    void GoToAtSameY()
    {
        goToSameY = waypointList[currentWaypoint].position;
        goToSameY.y = transform.position.y;
        agent.destination = goToSameY;
    }
}
