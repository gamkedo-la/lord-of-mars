using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeManager : MonoBehaviour
{
    public Transform canvas;
    public GameObject indicatorPrefab;
    public GameObject grenadePrefab;
    Vector3 lastPosition = Vector3.zero;

    void Start()
    {
        StartCoroutine(GrenadeMeTest());
    }


    IEnumerator GrenadeMeTest()
    {
        while(true)
        {
            yield return new WaitForSeconds(Random.Range(3.0f, 7.0f)); //how often do we check throwing a grenade 
            Vector3 posNow = transform.position;
            float distMoved = Vector3.Distance(posNow, lastPosition);
            lastPosition = posNow;
            if (distMoved < 2.0f) //how far the player has moved between checks to not get grenaded 
            {

            
                Collider[] throwRadius = Physics.OverlapSphere(transform.position, 80.0f); //how far AI can throw a grenade
                bool foundThrower = false;
                for (int i = 0; i < throwRadius.Length; i++)
                {
                    EnemyAI enemyScript = throwRadius[i].GetComponent<EnemyAI>(); //might need to check for children of parent?
                    if (enemyScript)
                    {
                        foundThrower = true;
                        break;
                    }
                }
                if(foundThrower)
                {
                    //right now spawns in front of camera- needs UI indicator- should happen off camera at that point
                   GameObject grenGO = GameObject.Instantiate(grenadePrefab, Camera.main.transform.position - Camera.main.transform.forward * 2.0f, Quaternion.identity);
                    GameObject indicatorGO = GameObject.Instantiate(indicatorPrefab);
                    indicatorGO.transform.SetParent(canvas);
                    indicatorGO.transform.localPosition = Vector3.zero;
                    GrenadeTracker gtScript = indicatorGO.GetComponent<GrenadeTracker>();
                    gtScript.grenadeTracked = grenGO.transform;
                    Rigidbody rb = grenGO.GetComponent<Rigidbody>();
                    rb.angularVelocity = Random.insideUnitSphere;
                    rb.AddForce(Random.onUnitSphere * 100.0f);
                }
            }
        }
    }

}
