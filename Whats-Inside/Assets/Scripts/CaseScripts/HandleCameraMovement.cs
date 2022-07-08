using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class HandleCameraMovement : MonoBehaviour
{
    [SerializeField] Camera cam;
    [SerializeField] GameObject vCam;
    [SerializeField] GameObject Case;
    [SerializeField] float sensitivity;
    [SerializeField] LayerMask puzzleLayer;

    Vector3 PreviousPosition;
    Vector3 camDistance;

    Vector3 posBeforeFocus;
    Quaternion rotBeforeFocus;

    bool isFocusing;
    PuzzleFocus currentFocus;

    private void Update()
    {
        Vector3 mousepos = Input.mousePosition;
        mousepos.z = 10f;
        mousepos = cam.ScreenToWorldPoint(mousepos);

        RaycastHit hit;
        if (Input.GetMouseButtonDown(0)) {
            if (Physics.Raycast(cam.transform.position, mousepos - cam.transform.position, out hit, Mathf.Infinity, puzzleLayer))
            {
                currentFocus = hit.collider.GetComponent<PuzzleFocus>();
                currentFocus.vCam.SetActive(true);
                isFocusing = true;
            }
        }
        if (!isFocusing)
        {
            CameraDragMovement();
        }
        else
        {
            HandleFocus();
        }
        
    }

    void HandleFocus()
    {
        if (Input.GetMouseButtonDown(1))
        {
            currentFocus.vCam.SetActive(false);
            currentFocus = null;
            isFocusing = false;
        }

    }

    private void OnDrawGizmos()
    {
        Vector3 mousepos = Input.mousePosition;
        mousepos.z = 10f;
        mousepos = cam.ScreenToWorldPoint(mousepos);
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(cam.transform.position, mousepos - cam.transform.position);
    }

    private void CameraDragMovement()
    {
        if (Input.GetMouseButtonDown(0))
        {
            PreviousPosition = cam.ScreenToViewportPoint(Input.mousePosition);
            if (camDistance == Vector3.zero) camDistance = Case.transform.position - vCam.transform.position;
            Debug.Log(camDistance);
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 direction = PreviousPosition - cam.ScreenToViewportPoint(Input.mousePosition);

            vCam.transform.position = Vector3.zero;
            vCam.transform.Rotate(Vector3.right, direction.y * (sensitivity * 50f));
            vCam.transform.Rotate(Vector3.up, -direction.x * (sensitivity * 50f), Space.World);
            vCam.transform.Translate(new Vector3(0, 0, -camDistance.z));

            PreviousPosition = cam.ScreenToViewportPoint(Input.mousePosition);
        }
    }

    
}
