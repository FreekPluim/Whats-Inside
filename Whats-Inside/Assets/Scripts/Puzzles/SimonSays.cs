using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State { Start, showingColors, PlayerInput, Won, Lost, UnFocused }

public class SimonSays : MonoBehaviour
{
    public State state = State.UnFocused;

    [SerializeField] List<GameObject> buttons = new List<GameObject>();
    [SerializeField] Camera cam;
    [SerializeField] LayerMask simonSaysLayer;
    [SerializeField] int maxTurns;

    Dictionary<GameObject, Material> material = new Dictionary<GameObject, Material>();

    List<GameObject> orderRandom = new List<GameObject>();
    List<GameObject> orderClicked = new List<GameObject>();

    bool onceStart = false;
    bool onceRestart = false;
    bool once = false;
    bool won = false;


    private void Start()
    {
        SetupMaterials();
    }
    void SetupMaterials()
    {
        material.Add(buttons[0], buttons[0].GetComponent<Renderer>().material);
        material.Add(buttons[1], buttons[1].GetComponent<Renderer>().material);
        material.Add(buttons[2], buttons[2].GetComponent<Renderer>().material);
        material.Add(buttons[3], buttons[3].GetComponent<Renderer>().material);
    }

    private void Update()
    {
        switch (state)
        {
            case State.Start:
                if(!onceStart) StartNew();
                state = State.showingColors;
                break;
            case State.showingColors:
                if (!once) StartCoroutine(ShowingColors());
                break;
            case State.PlayerInput:
                ReadPlayerInput();
                break;
            case State.Won:
                GameWon();
                break;
            case State.Lost:
                if(!onceRestart) StartCoroutine(DelayRestart());
                break;
            default:
                break;
        }
    }
    void StartNew()
    {
        once = false;
        onceRestart = false;
        orderClicked.Clear();
        orderRandom.Clear();

        Debug.Log(orderRandom.Count);

        orderRandom.Add(buttons[Random.Range(0, buttons.Count)]);
        onceStart = true;
    }
    void AddNew()
    {
        orderClicked.Clear();
        orderRandom.Add(buttons[Random.Range(0, buttons.Count)]);
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

            if (isSame())
            {
                if(orderClicked.Count == orderRandom.Count)
                {
                    if(orderRandom.Count == maxTurns)
                    {
                        state = State.Won;
                    }
                    else
                    {
                        once = false;
                        AddNew();
                        StartCoroutine(DelayShowingColors());
                    }

                } 
            }
            else
            {
                state = State.Lost;
            }      
        }
    }
    void checkHit(ref RaycastHit hit, GameObject obj)
    {
        if(hit.collider.gameObject == obj)
        {
            StartCoroutine(LightUp(obj));
            orderClicked.Add(obj);
        }
    }
    void GameWon()
    {
        if (!won)
        {
            foreach (var index in material)
            {
                index.Value.SetFloat("_EmissiveExposureWeight", 0);
            }
            won = true;
        }
    }


    bool isSame()
    {
        for (int i = 0; i < orderClicked.Count; i++)
        {
            if (orderClicked[i].tag != orderRandom[i].tag)
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
            material[orderRandom[i]].SetFloat("_EmissiveExposureWeight", 0);
            yield return new WaitForSeconds(0.5f);
            material[orderRandom[i]].SetFloat("_EmissiveExposureWeight", 1);
            yield return new WaitForSeconds(0.5f);
        }

        state = State.PlayerInput;
        Debug.Log("Changing to Player input");
    }
    IEnumerator LightUp(GameObject color)
    {
        material[color].SetFloat("_EmissiveExposureWeight", 0);
        yield return new WaitForSeconds(0.5f);
        material[color].SetFloat("_EmissiveExposureWeight", 1);
    }
    IEnumerator DelayShowingColors()
    {
        yield return new WaitForSeconds(1);
        state = State.showingColors;
    }
    IEnumerator DelayRestart()
    {
        onceRestart = true;
        yield return new WaitForSeconds(1);
        state = State.Start;
    }
}
