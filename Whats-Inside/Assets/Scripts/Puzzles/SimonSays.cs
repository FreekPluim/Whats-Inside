using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum State { Start, showingColors, PlayerInput, Won, Lost }

public class SimonSays : MonoBehaviour
{
    State state = State.Start;

    [SerializeField] List<GameObject> buttons = new List<GameObject>();
    Dictionary<GameObject, Material> material = new Dictionary<GameObject, Material>();

    List<GameObject> orderRandom = new List<GameObject>();
    List<GameObject> orderClicked = new List<GameObject>();

    bool once = false;

    private void Start()
    {
        SetupMaterials();
    }

    void SetupMaterials()
    {
        material.Add(buttons[0], buttons[0].GetComponent<Material>());
        material.Add(buttons[1], buttons[1].GetComponent<Material>());
        material.Add(buttons[2], buttons[2].GetComponent<Material>());
        material.Add(buttons[3], buttons[3].GetComponent<Material>());
    }
    void StartNew()
    {
        orderRandom.Clear();

        orderRandom.Add(buttons[Random.Range(0, buttons.Count)]);
    }
    void AddNew()
    {
        orderRandom.Add(buttons[Random.Range(0, buttons.Count)]);
    }

    private void Update()
    {
        switch (state)
        {
            case State.Start:
                StartNew();
                state = State.showingColors;
                break;
            case State.showingColors:
                if (!once) StartCoroutine(ShowingColors());
                break;
            case State.PlayerInput:
                break;
            case State.Won:
                break;
            case State.Lost:
                state = State.Start;
                break;
            default:
                break;
        }
    }

    IEnumerator ShowingColors()
    {
        foreach (GameObject color in orderRandom)
        {
            material[color].SetFloat("_EmissiveExposureWeight", 0);
            yield return new WaitForSeconds(1);
            material[color].SetFloat("_EmissiveExposureWeight", 1);
        }
        state = State.PlayerInput;


    }

}
