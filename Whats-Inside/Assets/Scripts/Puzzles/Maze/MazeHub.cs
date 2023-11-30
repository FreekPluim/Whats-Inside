using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MazeHub : MonoBehaviour
{
    [SerializeField] GameObject Input, Reciever;
    PhotonView view;

    int lastType = 5;
    public int type = 5;
    public bool won;

    MazeReciever recieverScript;
    MazeInput inputScript;

    private void Start()
    {
        view = GetComponent<PhotonView>();
        CheckPuzzleSide();

        recieverScript = Reciever.GetComponent<MazeReciever>();
        inputScript = Input.GetComponent<MazeInput>();
    }

    private void Update()
    {
        if (type != lastType)
        {
            if (type == 0)
            {
                Input.SetActive(true);
                Reciever.SetActive(false);
            } //Is Input
            else
            {
                Input.SetActive(false);
                Reciever.SetActive(true);
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

    public void SendPressedArrow(int PressedColorID)
    {
        this.view.RPC("RPC_SendButtonPressed", RpcTarget.OthersBuffered, PressedColorID);
    }

    public void SendIfWon(bool pWon)
    {
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

    [PunRPC]
    void RPC_ReachedGoal(bool pWon)
    {
        inputScript.won = pWon;
    }


}
