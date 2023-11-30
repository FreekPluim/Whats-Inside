using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeInput : Puzzle
{
    [SerializeField] List<GameObject> buttons = new List<GameObject>();
    [SerializeField] Camera cam;
    [SerializeField] LayerMask mazeLayer;

    Dictionary<GameObject, int> direction = new Dictionary<GameObject, int>();

    MazeHub stream;
    PuzzleFocus focus;

    public bool won;

    private void Start()
    {
        focus = transform.parent.parent.GetComponent<PuzzleFocus>();
        stream = transform.parent.GetComponent<MazeHub>();

        for (int i = 0; i < buttons.Count; i++)
        {
            direction.Add(buttons[i], i);
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
            if (Physics.Raycast(cam.transform.position, mousepos - cam.transform.position, out hit, Mathf.Infinity, mazeLayer))
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
            stream.SendPressedArrow(direction[obj]);
        }
    }

    void GameWon()
    {
        if (stream.won && !won)
        {
            //Handle game won
        }
    }
}
