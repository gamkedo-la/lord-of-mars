using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GrenadeTracker : MonoBehaviour
{
    public Transform grenadeTracked;
    Image image;
    float angTo = 0.0f;
    Camera cam;
    float ignoreDistance = 15.0f;

    private void Start()
    {
        cam = Camera.main;
        image = gameObject.GetComponent<Image>();
        StartCoroutine(IndicatorFlash());
    }

    IEnumerator IndicatorFlash()
    {
        while (true)
        {
            float distNow = Vector3.Distance(grenadeTracked.position, cam.transform.position);
            if (distNow < ignoreDistance)
            {
                image.enabled = !image.enabled;
            }
            else
            {
                image.enabled = false;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(grenadeTracked == null)
        {
            Destroy(gameObject);
            return;
        }
        Vector3 usToTarget = cam.transform.InverseTransformPoint(grenadeTracked.position);
        angTo = Mathf.Atan2(usToTarget.z, usToTarget.x) *Mathf.Rad2Deg;
        transform.localRotation = Quaternion.AngleAxis(angTo, Vector3.forward);
    }
}
