using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedExplode : MonoBehaviour
{
    public GameObject blastEffect;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.B))
        {
            Debug.Log("grenade boom");
            GameObject.Instantiate(blastEffect, transform.position, Quaternion.identity);
            Collider[] blastedRadius = Physics.OverlapSphere(transform.position, 10.0f);
            for(int i = 0; i < blastedRadius.Length; i++)
            {
                Debug.Log(blastedRadius[i].gameObject.name);
            }
        }
    }
}
