using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrequencyInput : Puzzle
{
    PuzzleFocus focus;
    FrequencyHub stream;

    [SerializeField] List<GameObject> buttons = new List<GameObject>();
    [SerializeField] Camera cam;
    [SerializeField] LayerMask frequencyLayer;

    [SerializeField] GameObject lightOff;
    [SerializeField] GameObject lightOn;

    Dictionary<GameObject, int> direction = new Dictionary<GameObject, int>();

    bool won = false;

    private void Start()
    {
        focus = transform.parent.parent.GetComponent<PuzzleFocus>();
        stream = transform.parent.GetComponent<FrequencyHub>();

        for (int i = 1; i < buttons.Count+1; i++)
        {
            direction.Add(buttons[i-1], i);
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

    void ReadPlayerInput()
    {
        Vector3 mousepos = Input.mousePosition;
        mousepos.z = 10f;
        mousepos = cam.ScreenToWorldPoint(mousepos);

        RaycastHit hit;

        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(cam.transform.position, mousepos - cam.transform.position, out hit, Mathf.Infinity, frequencyLayer))
            {
                foreach (var button in buttons)
                {
                    checkHit(ref hit, button);
                }
            }
        }
    }

    void checkHit(ref RaycastHit hit, GameObject obj)
    {
        if (hit.collider.gameObject == obj)
        {
            stream.SendPressedButton(direction[obj]);
        }
    }

    void GameWon()
    {
        if (stream.won)
        {
            lightOff.SetActive(false);
            lightOn.SetActive(true);
        }
    }
}
