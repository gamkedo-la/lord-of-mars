using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SonicGun : MonoBehaviour
{
    Transform cameraTransform;
    LayerMask bulletMask;
    public Transform fireFrom;
    public ParticleSystem shockwave;

    // Start is called before the first frame update
    void Start()
    {
        cameraTransform = Camera.main.transform;
        bulletMask = LayerMask.GetMask("Default", "TransparentFX", "Water", "Grapple", "Ground");
    }

    public void Shoot()
    {
        FindObjectOfType<AudioManager>().SoundPlay("SonicGun");
        Instantiate(shockwave, fireFrom.position, fireFrom.rotation);
        RaycastHit rhInfo;
        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out rhInfo, 200.0f, bulletMask))
        {
            Damageable hurtScript = rhInfo.collider.GetComponentInParent<Damageable>();
            if (hurtScript)
            {
                hurtScript.TakeDamage(25.0f, cameraTransform.forward);
            }
            RaycastHit[] pushList = Physics.CapsuleCastAll(transform.position, rhInfo.point, 2.0f, transform.forward); //last argument might be wrong 
            for(int i = 0; i< pushList.Length; i++)
            {
                Rigidbody rb = pushList[i].collider.GetComponent<Rigidbody>();
                if(rb)
                {
                    rb.AddForce(transform.forward * 1000.0f);
                }
            }
        }
    }
}
