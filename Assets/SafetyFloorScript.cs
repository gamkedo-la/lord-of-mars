using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafetyFloorScript : MonoBehaviour
{
    [SerializeField] GameObject resetReferencePointGameObject;
    float yCoordinateForResettingPlayer;

    // Start is called before the first frame update
    void Start()
    {
        yCoordinateForResettingPlayer = resetReferencePointGameObject.transform.position.y;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("trigger entered on safety floor");

    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("trigger entered on safety floor");
        if (collision.gameObject.name == "Scene Player")
        {
            
            collision.gameObject.transform.position = new Vector3(collision.gameObject.transform.position.x, yCoordinateForResettingPlayer,
                                                            collision.gameObject.transform.position.z);
        }
    }
}
