using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleFocus : MonoBehaviour
{
    public GameObject vCam;
    BoxCollider focusColider;

    public bool beingFocused;
    public bool simonSaysStarted;
    public bool mazeStarted;

    [SerializeField] GameObject puzzle;

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
            if(puzzle.TryGetComponent<Puzzle>(out Puzzle pPuzzle))
            {
                if (pPuzzle is SimonSaysReciever) HandleSimonSays(pPuzzle as SimonSaysReciever);
                if (pPuzzle is MazeInput) HandleMaze(pPuzzle as MazeInput);
            }
        }
        else
        {
            simonSaysStarted = false;
        }
    }

    void HandleSimonSays(SimonSaysReciever simonSays)
    {
        if (!simonSaysStarted)
        {
            StartCoroutine(SimonSaysStartDelay(simonSays));
            simonSaysStarted = true;
        }
    }

    void HandleMaze(MazeInput maze)
    {
        
    }

    IEnumerator SimonSaysStartDelay(SimonSaysReciever ss)
    {
        yield return new WaitForSeconds(3f);
        ss.state = State.Start;
    }



}
