using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleFocus : MonoBehaviour
{
    public GameObject vCam;
    BoxCollider focusColider;

    bool beingFocused;
    public bool simonSaysStarted;

    private void Start()
    {
        focusColider = GetComponent<BoxCollider>();
    }

    private void Update()
    {
        beingFocused = vCam.activeSelf;
        focusColider.enabled = !vCam.activeSelf;

        if (beingFocused)
        {
            if(gameObject.TryGetComponent<SimonSays>(out SimonSays ss) && !simonSaysStarted)
            {
                StartCoroutine(SimonSaysStartDelay(ss));
                simonSaysStarted = true;
            }
        }
        else
        {
            simonSaysStarted = false;
        }
    }

    IEnumerator SimonSaysStartDelay(SimonSays ss)
    {
        yield return new WaitForSeconds(3f);

        ss.state = State.Start;
    }

}
