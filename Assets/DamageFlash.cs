using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageFlash : MonoBehaviour
{
    public Image frontFlasher;
    public Image backFlasher;
    public Image leftFlasher;
    public Image rightFlasher;


    // Start is called before the first frame update
    void Start()
    {
        frontFlasher.color = Color.clear;
        rightFlasher.color = Color.clear;
        backFlasher.color = Color.clear;
        leftFlasher.color = Color.clear;

    }

    void FixedUpdate() //fixed so we can do a times equals percentage 
    {
        float fadePerc = 0.95f;
        Color fadeAlpha = frontFlasher.color;
        fadeAlpha.a *= fadePerc;
        frontFlasher.color = fadeAlpha;

        fadeAlpha = rightFlasher.color;
        fadeAlpha.a *= fadePerc;
        rightFlasher.color = fadeAlpha;

        fadeAlpha = backFlasher.color;
        fadeAlpha.a *= fadePerc;
        backFlasher.color = fadeAlpha;

        fadeAlpha = leftFlasher.color;
        fadeAlpha.a *= fadePerc;
        leftFlasher.color = fadeAlpha;
    }

   public void TakeShotFrom(Vector3 shotForward)
    {
        float damageWindow = 90.0f;
        Color tempAlpha = Color.white;
        float angDif = Quaternion.Angle(Quaternion.LookRotation(-transform.forward), Quaternion.LookRotation(shotForward))/damageWindow;
        angDif = 1.0f - Mathf.Clamp01(angDif);
        tempAlpha.a = angDif;
        frontFlasher.color = tempAlpha;

        angDif = Quaternion.Angle(Quaternion.LookRotation(-transform.right), Quaternion.LookRotation(shotForward)) / damageWindow;
        angDif = 1.0f - Mathf.Clamp01(angDif);
        tempAlpha.a = angDif;
        rightFlasher.color = tempAlpha;

        angDif = Quaternion.Angle(Quaternion.LookRotation(transform.forward), Quaternion.LookRotation(shotForward)) / damageWindow;
        angDif = 1.0f - Mathf.Clamp01(angDif);
        tempAlpha.a = angDif;
        backFlasher.color = tempAlpha;

        angDif = Quaternion.Angle(Quaternion.LookRotation(transform.right), Quaternion.LookRotation(shotForward)) / damageWindow;
        angDif = 1.0f - Mathf.Clamp01(angDif);
        tempAlpha.a = angDif;
        leftFlasher.color = tempAlpha;
    }
}


