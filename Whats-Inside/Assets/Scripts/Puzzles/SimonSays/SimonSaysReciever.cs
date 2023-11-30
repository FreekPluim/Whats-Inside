using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimonSaysReciever : Puzzle
{
    public State state = State.UnFocused;

    SimonSaysHub stream;

    [SerializeField] List<GameObject> lights = new List<GameObject>();
    Dictionary<GameObject, MeshRenderer> materials = new Dictionary<GameObject, MeshRenderer>();

    List<int> colorID = new List<int>();
    [SerializeField] List<GameObject> colors = new List<GameObject>();
    Dictionary<int, GameObject> colorsWithID = new Dictionary<int, GameObject>();

    List<int> orderRandom = new List<int>();

    [SerializeField] Material lightOffMaterial;
    [SerializeField] Material lightOnMaterial;
    [SerializeField] Material gameWonMaterial;

    int currentTurn;
    int lastTurn;

    bool onceStart = false;
    bool onceRestart = false;
    bool once = false;
    bool won = false;

    private void Start()
    {
        stream = transform.parent.GetComponent<SimonSaysHub>();
        for (int i = 0; i < lights.Count; i++)
        {
            materials.Add(lights[i], lights[i].GetComponent<MeshRenderer>());
        }
        for (int i = 0; i < 4; i++)
        {
            colorID.Add(i);
        }
        for (int i = 0; i < 4; i++)
        {
            colorsWithID.Add(i, colors[i]);
        }
    }

    private void Update()
    {
        if (!won)
        {
            HandleTurnLights();
            switch (state)
            {
                case State.Start:
                    if (!onceStart) StartNew();
                    state = State.showingColors;
                    break;
                case State.showingColors:
                    if (!once) StartCoroutine(ShowingColors());
                    break;
                case State.Won:
                    GameWon();
                    break;
                case State.Lost:
                    if (!onceRestart) StartCoroutine(DelayRestart());
                    break;
                default:
                    break;
            }
        }
    }

    void StartNew()
    {
        once = false;
        onceRestart = false;
        currentTurn = 0;
        stream.colorsSend.Clear();
        orderRandom.Clear();

        Debug.Log(orderRandom.Count);

        orderRandom.Add(Random.Range(0, colors.Count));
        onceStart = true;
    }
    void AddNew()
    {
        currentTurn++;
        stream.colorsSend.Clear();
        orderRandom.Add(Random.Range(0, colors.Count));
    }

    void GameWon()
    {
        if (!won)
        {
            for (int i = 0; i < materials.Count; i++)
            {
                materials[lights[i]].material = gameWonMaterial;
            }
            stream.SendIfWon(true);
            won = true;
        }
    }


    public void CheckIfOrderCorrect()
    {
        if (isSame())
        {
            if (stream.colorsSend.Count == orderRandom.Count)
            {
                if (orderRandom.Count == 5)
                {
                    state = State.Won;
                }
                else
                {
                    AddNew();
                }

            }
        }
        else
        {
            state = State.Lost;
        }
    }
    bool isSame()
    {
        for (int i = 0; i < stream.colorsSend.Count; i++)
        {
            if (stream.colorsSend[i] != orderRandom[i])
            {
                return false;
            }
        }
        return true;
    }


    IEnumerator ShowingColors()
    {
        once = true;
        onceStart = false;

        for (int i = 0; i < orderRandom.Count; i++)
        {
            colorsWithID[orderRandom[i]].SetActive(true);
            yield return new WaitForSeconds(0.5f);
            colorsWithID[orderRandom[i]].SetActive(false);
            yield return new WaitForSeconds(0.5f);
        }

        yield return new WaitForSeconds(3f);
        if(!won) StartCoroutine(ShowingColors());
    }
    IEnumerator DelayRestart()
    {
        onceRestart = true;
        yield return new WaitForSeconds(1);
        state = State.Start;
    }
    IEnumerator DelayShowingColors()
    {
        yield return new WaitForSeconds(1);
        state = State.showingColors;
    }

    //======Turn Lights======
    private void HandleTurnLights()
    {
        if (currentTurn != lastTurn)
        {
            switch (currentTurn)
            {
                case 0:
                    LightsOff();
                    break;
                case 1:
                    LightsOn(0);
                    break;
                case 2:
                    LightsOn(1);
                    break;
                case 3:
                    LightsOn(2);
                    break;
                case 4:
                    LightsOn(3);
                    break;
                case 5:
                    LightsOn(4);
                    break;
                default:
                    break;
            } // Check turn. Switch on lights if turn is that;
            lastTurn = currentTurn;
        }
    }
    void LightsOn(int LightID)
    {
        materials[lights[LightID]].material = lightOnMaterial;
    }
    void LightsOff()
    {
        for (int i = 0; i < 5; i++)
        {
            materials[lights[i]].material = lightOffMaterial;
        }
    }




}
