using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GaussGun : MonoBehaviour
{
    Transform cameraTransform;
    LayerMask bulletMask;
    public Transform fireFrom;
    public GameObject flashParticle;
    public GameObject tracerPrefab;
    public ParticleSystem flash;
    public float hitAmount = 75.0f;
    public float reloadTimeBetweenShots = 1.0f;

    private float reloadTimeRemaining = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        cameraTransform = Camera.main.transform;
        bulletMask = LayerMask.GetMask("Default", "TransparentFX", "Water", "Grapple", "Ground", "Enemy");
    }
    private void Update()
    {
        if(reloadTimeRemaining > 0.0f)
        {
            reloadTimeRemaining -= Time.deltaTime;
            if(reloadTimeRemaining <= 0.0f)
            {
                Debug.Log("TODO, Indicate weapon ready HUD element");
            }

        }

    }


    public void Shoot()
    {
        if(reloadTimeRemaining > 0.0f)
        {
            Debug.Log("Put sound for gun not ready to fire again");
            return;
        }
        reloadTimeRemaining = reloadTimeBetweenShots;

        RaycastHit rhInfo;
        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out rhInfo, 200.0f, bulletMask))
        {
            //Debug.Log(rhInfo.collider.name);
            FindObjectOfType<AudioManager>().SoundPlay("FunGun");
            Damageable hurtScript = rhInfo.collider.GetComponentInParent<Damageable>();
            if (hurtScript)
            {
                hurtScript.TakeDamage(hitAmount, cameraTransform.forward);
            }
            Instantiate(flashParticle, rhInfo.point + rhInfo.normal * 0.1f, Quaternion.identity);
            
        }
        flash.Play();

        if (tracerPrefab) Instantiate(tracerPrefab, fireFrom.position, Quaternion.LookRotation(cameraTransform.forward));


    }
}
