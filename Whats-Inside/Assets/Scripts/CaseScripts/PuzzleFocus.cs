using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleFocus : MonoBehaviour
{
    public GameObject vCam;
    BoxCollider focusColider;

    bool beingFocused;

    private void Start()
    {
        focusColider = GetComponent<BoxCollider>();
    }

    private void Update()
    {
        beingFocused = vCam.activeSelf;
        focusColider.enabled = !vCam.activeSelf;
    }

}
