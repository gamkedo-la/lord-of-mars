using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerEvent : MonoBehaviour
{
    public GameObject[] activateList;
    private bool hasBeenUsed = false;


    private void OnTriggerEnter(Collider other)
    {
        
        if(hasBeenUsed)
        {
            return;
        }
        hasBeenUsed = true;
        Debug.Log("Collision detected");
        for(int i = 0; i<activateList.Length; i++)
        {
            activateList[i].SendMessage("TriggerActivate");
        }
    }

    public void OnDrawGizmos()
    {
        for (int i = 0; i < activateList.Length; i++)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, activateList[i].transform.position);
        }
    }

}
