using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorWorld : MonoBehaviour
{
    public Transform mirrorThis;
    private float offsetX = 200.0f;

    // Start is called before the first frame update
    void Start()
    {
        if(mirrorThis == null)
        {
            Debug.LogWarning("Mirror element missing assignment");
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.localPosition = mirrorThis.localPosition;
    }

}
