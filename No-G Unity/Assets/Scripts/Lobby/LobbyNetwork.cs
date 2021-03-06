﻿/*
Reference for Code structure:
https://www.youtube.com/watch?v=EqW-4r7goaQ
*/

using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyNetwork : MonoBehaviourPunCallbacks
{
    public RoomLayoutGroup rlg;
    public PlayerLayoutGroup plg;

    // Use this for initialization
    private void Start()
    {        
        if (!PhotonNetwork.IsConnected)
        {
            print("Connecting to server..");
            PhotonNetwork.ConnectUsingSettings();

            //PhotonNetwork.GameVersion = "1";
        }
    }

    public override void OnConnectedToMaster()
    {
        print("Connected to master.");
        PhotonNetwork.AutomaticallySyncScene = true;
        //PhotonNetwork.playerName = PlayerNetwork.Instance.PlayerName;
        PhotonNetwork.JoinLobby(TypedLobby.Default);

    }

    public override void OnJoinedLobby()
    {
        print("Joined lobby.");

        if (!PhotonNetwork.InRoom)
            MainCanvasManager.Instance.LobbyCanvas.transform.SetAsLastSibling();
    }

    public override void OnJoinedRoom()
    {
        print("Joined Room.");

        if (PhotonNetwork.InRoom)
        {
            PhotonView photonView = PhotonView.Get(this);
            photonView.RPC("JoinRoom", RpcTarget.All);
        }
    }

    public override void OnLeftRoom()
    {
        print("Left Room.");

        if (!PhotonNetwork.InRoom)
        {
            PhotonView photonView = PhotonView.Get(this);
            photonView.RPC("JoinRoom", RpcTarget.All);
        }
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        print("Room List Update");
        rlg.OnReceivedRoomListUpdate(roomList);
    }

    [PunRPC]
    void JoinRoom()
    {
        plg.JoinPhotonRoom();
    }

}
