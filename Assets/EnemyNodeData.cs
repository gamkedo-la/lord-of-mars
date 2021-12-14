using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyNodeData : MonoBehaviour
{
    public EnemyAI beingUsedBy;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LivingCheck());
    }

    IEnumerator LivingCheck()
    {
        while (true)
        {
            if(beingUsedBy)
            {
                if(beingUsedBy.isAlive() == false)
                {
                    beingUsedBy = null;
                }
            }
            yield return new WaitForSeconds(Random.Range(3.0f, 6.0f));
        }
    }
}
