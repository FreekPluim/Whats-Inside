using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SimonSaysHub : MonoBehaviourPun
{
    //RPC VALUES
    int lastType = 5;
    public int type = 5;
    public bool won;
    public List<int> colorsSend = new List<int>();

    [SerializeField] GameObject Input, Reciever;
    PhotonView view;


    int oldListCount;

    SimonSaysReciever recieverScript;
    SimonSaysInput inputScript;

    private void Start()
    {
        view = GetComponent<PhotonView>();
        CheckPuzzleSide();

        recieverScript = Reciever.GetComponent<SimonSaysReciever>();
        inputScript = Input.GetComponent<SimonSaysInput>();
    }

    private void Update()
    {
        if (Reciever.activeSelf)
        {
            if (oldListCount != colorsSend.Count)
            {
                recieverScript.CheckIfOrderCorrect();
                oldListCount = colorsSend.Count;
            }
        }

        if(type != lastType)
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

    public void SendPressedColor(int PressedColorID)
    {
        this.view.RPC("RPC_SendColor", RpcTarget.OthersBuffered, PressedColorID);
    }

    public void SendIfWon(bool pWon)
    {
        this.view.RPC("RPC_SimonSaysWon", RpcTarget.OthersBuffered, pWon);
    }



    [PunRPC]
    void RPC_Set(int pType)
    {
        type = pType;
    }
    [PunRPC]
    void RPC_SendColor(int colorID)
    {
        colorsSend.Add(colorID);
    }
    [PunRPC]
    void RPC_SimonSaysWon(bool pWon)
    {
        won = pWon;
    }
}
