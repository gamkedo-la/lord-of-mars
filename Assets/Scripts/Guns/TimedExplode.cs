using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedExplode : MonoBehaviour
{
    public GameObject blastEffect;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ExplodeAfterDelay());
    }


    IEnumerator ExplodeAfterDelay()
    {
        yield return new WaitForSeconds(3.0f);
        Debug.Log("grenade boom");
        GameObject.Instantiate(blastEffect, transform.position, Quaternion.identity);
        Collider[] blastedRadius = Physics.OverlapSphere(transform.position, 10.0f);
        for(int i = 0; i < blastedRadius.Length; i++)
        {
            Damageable damageScript = blastedRadius[i].GetComponent<Damageable>(); //might need to check for children of parent?
            if(damageScript) //todo confirm line of sight with raycast 
            {
                Debug.Log(blastedRadius[i].gameObject.name);
                Vector3 hurtVect = (blastedRadius[i].transform.position - transform.position).normalized;
                damageScript.TakeDamage(30.0f, hurtVect);
            }
        } //end of for 
        Destroy(gameObject);
    } //end of function 

}
