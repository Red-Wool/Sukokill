using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class JoinCreateRooms : MonoBehaviourPunCallbacks
{
    public void CreateRoom()
    {
        PhotonNetwork.CreateRoom("Game");
    }

    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom("Game");
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("GameTestOnline");
    }
}
