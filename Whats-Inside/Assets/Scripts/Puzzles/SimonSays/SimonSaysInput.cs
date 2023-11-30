using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimonSaysInput : Puzzle
{
    [SerializeField] List<GameObject> buttons = new List<GameObject>();
    [SerializeField] Camera cam;
    [SerializeField] LayerMask simonSaysLayer;

    SimonSaysHub stream;

    Dictionary<GameObject, Material> material = new Dictionary<GameObject, Material>();
    Dictionary<GameObject, int> colorID = new Dictionary<GameObject, int>();

    PuzzleFocus focus;

    bool won;

    private void Start()
    {
        focus = transform.parent.parent.GetComponent<PuzzleFocus>();

        stream = transform.parent.GetComponent<SimonSaysHub>();
        SetupMaterials();

        for (int i = 0; i < buttons.Count; i++)
        {
            colorID.Add(buttons[i], i);
        }
    }

    private void Update()
    {
        if (focus.beingFocused && !won)
        {
            ReadPlayerInput();
            GameWon();
        }
    }

    void SetupMaterials()
    {
        material.Add(buttons[0], buttons[0].GetComponent<Renderer>().material);
        material.Add(buttons[1], buttons[1].GetComponent<Renderer>().material);
        material.Add(buttons[2], buttons[2].GetComponent<Renderer>().material);
        material.Add(buttons[3], buttons[3].GetComponent<Renderer>().material);
    }

    void ReadPlayerInput()
    {
        Vector3 mousepos = Input.mousePosition;
        mousepos.z = 10f;
        mousepos = cam.ScreenToWorldPoint(mousepos);

        RaycastHit hit;

        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(cam.transform.position, mousepos - cam.transform.position, out hit, Mathf.Infinity, simonSaysLayer))
            {
                checkHit(ref hit, buttons[0]);
                checkHit(ref hit, buttons[1]);
                checkHit(ref hit, buttons[2]);
                checkHit(ref hit, buttons[3]);
            }
        }
    }

    void checkHit(ref RaycastHit hit, GameObject obj)
    {
        if (hit.collider.gameObject == obj)
        {
            StartCoroutine(LightUp(obj));
            stream.SendPressedColor(colorID[obj]);
        }
    }

    void GameWon()
    {
        if (stream.won && !won)
        {
            foreach (var index in material)
            {
                index.Value.SetFloat("_EmissiveExposureWeight", 0);
            }
            won = true;
        }
    }

    IEnumerator LightUp(GameObject color)
    {
        material[color].SetFloat("_EmissiveExposureWeight", 0);
        yield return new WaitForSeconds(0.5f);
        material[color].SetFloat("_EmissiveExposureWeight", 1);
    }
}
