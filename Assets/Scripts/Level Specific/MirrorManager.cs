using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorManager : MonoBehaviour
{
    public static MirrorManager instance;
    public Transform realPlayer;
    public Transform realPlayerBody;
    public Transform dummyPlayer;
    public Transform mapEdgeLight;
    public Transform mapEdgeDark;
    private PlayerController pController;
    private bool flipping = false;
    private float playerHeight = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        pController = realPlayerBody.GetComponent<PlayerController>();
        playerHeight = realPlayerBody.position.y;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G) && flipping == false)
        {
            Debug.Log("DEBUG: gravity flipping");
            pController.enabled = false;
            flipping = true;
            StartCoroutine(FlipDelay());

            //TODO teleport to opposite surface
        }
        if(flipping)
        {
            realPlayerBody.position += Vector3.up * Time.deltaTime * 50.0f;
        }
    }
    IEnumerator FlipDelay()
    {
        yield return new WaitForSeconds(0.2f);
        Vector3 flippedPos = dummyPlayer.position;
        flippedPos.y = playerHeight;
        realPlayerBody.position = flippedPos;
        Transform parentTrade = realPlayer.parent;
        realPlayer.parent = dummyPlayer.parent;
        dummyPlayer.parent = parentTrade;
        flipping = false;
        yield return new WaitForSeconds(0.1f);
        pController.enabled = true;
    }
}
