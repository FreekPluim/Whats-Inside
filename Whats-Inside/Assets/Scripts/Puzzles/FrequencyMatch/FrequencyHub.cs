using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class FrequencyHub : MonoBehaviour
{
    PuzzleFocus focus;
    [SerializeField] GameObject Input, Reciever;
    PhotonView view;

    int lastType = 5;
    public int type = 5;
    public bool won;

    FrequencyReciever recieverScript;
    FrequencyInput inputScript;

    private void Start()
    {
        view = GetComponent<PhotonView>();
        CheckPuzzleSide();

        focus = transform.parent.GetComponent<PuzzleFocus>();
        recieverScript = Reciever.GetComponent<FrequencyReciever>();
        inputScript = Input.GetComponent<FrequencyInput>();
    }

    private void Update()
    {
        if (type != lastType)
        {
            if (type == 0)
            {
                Input.SetActive(true);
                Reciever.SetActive(false);

                focus.OnPuzzleFocus.AddListener(inputScript.OnFocused);
                focus.OnPuzzleUnFocus.AddListener(inputScript.OnUnFocus);
            } //Is Input
            else
            {
                Input.SetActive(false);
                Reciever.SetActive(true);

                focus.OnPuzzleFocus.AddListener(recieverScript.OnFocused);
                focus.OnPuzzleUnFocus.AddListener(recieverScript.OnUnFocus);
            } //Is Reciever
            lastType = type;
        }
    }

    private void CheckPuzzleSide()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            type = Random.Range(0, 2);
            if (view.IsMine)
            {
                if (type == 0) this.view.RPC("RPC_Set", RpcTarget.OthersBuffered, 1);
                if (type == 1) this.view.RPC("RPC_Set", RpcTarget.OthersBuffered, 0);
            }
        } //P1 create random number
    }

    public void SendPressedButton(int PressedDirectionID)
    {
        this.view.RPC("RPC_SendButtonPressed", RpcTarget.OthersBuffered, PressedDirectionID);
    }

    public void SendIfWon(bool pWon)
    {
        won = pWon;
        this.view.RPC("RPC_ReachedGoal", RpcTarget.OthersBuffered, pWon);
    }

    [PunRPC]
    void RPC_Set(int pType)
    {
        type = pType;
    }

    [PunRPC]
    void RPC_SendButtonPressed(int buttonID)
    {
        recieverScript.buttonPressed = buttonID;
    }
}
