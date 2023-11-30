using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

public class CheckPlayersInRoom : MonoBehaviour
{
    [SerializeField] GameObject p1, p2;
    [SerializeField] TextMeshProUGUI p1Nickname, p2Nickname;

    [SerializeField] GameObject startButton;

    private void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    private void Update()
    {
        switch (PhotonNetwork.PlayerList.Length)
        {
            case 1:
                p1.SetActive(true);
                p2.SetActive(false);
                break;
            case 2:
                p1.SetActive(true);
                p2.SetActive(true);
                break;
            default:
                break;
        }

        if (p1.activeSelf) p1Nickname.text = PhotonNetwork.PlayerList[0].NickName;
        if (p2.activeSelf) p2Nickname.text = PhotonNetwork.PlayerList[1].NickName;


        if (PhotonNetwork.PlayerList.Length == 2)
        {
            startButton.SetActive(true);
        }
        else
        {
            startButton.SetActive(false);
        }
    }

    public void onStartClick()
    {
        if(PhotonNetwork.IsMasterClient) PhotonNetwork.LoadLevel("Game");
        else { return; }
    }
}
