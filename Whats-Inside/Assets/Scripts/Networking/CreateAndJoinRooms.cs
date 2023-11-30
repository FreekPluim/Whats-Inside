using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class CreateAndJoinRooms : MonoBehaviourPunCallbacks
{
    [SerializeField] InputField CodeInput;
    [SerializeField] InputField NicknameInput;

    public void CreateRoom()
    {
        PhotonNetwork.CreateRoom(CodeInput.text, new RoomOptions() { MaxPlayers = 2, PublishUserId = true }) ;
    }

    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(CodeInput.text);
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LocalPlayer.NickName = NicknameInput.text;
        PhotonNetwork.LoadLevel("Lobby2");
    }
}
