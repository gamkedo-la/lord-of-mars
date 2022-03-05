using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGun : MonoBehaviour
{
    Transform cameraTransform;
    LayerMask bulletMask;
    public Transform fireFrom;
    public GameObject flashParticle;
    public ParticleSystem flash;


    // Start is called before the first frame update
    void Start()
    {
        cameraTransform = Camera.main.transform;
        bulletMask = LayerMask.GetMask("Default", "TransparentFX", "Water", "Grapple", "Ground", "Enemy");
    }


    public void Shoot()
    {
        RaycastHit rhInfo;
        AudioManager.instance.SoundPlayOneShot("MainGun");
        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out rhInfo, 200.0f, bulletMask))
        {
            //Debug.Log(rhInfo.collider.name);
            Damageable hurtScript = rhInfo.collider.GetComponentInParent<Damageable>();
            AudioManager.instance.SoundPlayOneShot("BulletHit");
            if (hurtScript)
            {
                hurtScript.TakeDamage(25.0f, cameraTransform.forward);
            }
            Instantiate(flashParticle, rhInfo.point + rhInfo.normal * 0.1f, Quaternion.identity);
        }
        flash.Play();
    }
}
