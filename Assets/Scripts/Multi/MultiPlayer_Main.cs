using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;

public class MultiPlayer_Main : MonoBehaviourPunCallbacks
{
    public static MultiPlayer_Main m_sinThis;
    private Action m_acOnJoin;
    private void Awake()
    {
        m_sinThis = this;
    }
    public void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }
    public override void OnConnectedToMaster()
    {
        Debug.Log("Master connected");
    }
    public void CreteOrJoin(string NameL,string NameP,Action callback)
    {
        RoomOptions roomOptions = new RoomOptions();
        string roomName = NameL;
        PhotonNetwork.NickName = NameP;
        roomOptions.MaxPlayers = 8;
        m_acOnJoin = callback;
        PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, TypedLobby.Default);
        
    }
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        m_acOnJoin?.Invoke();
    }
    public void OnDestroy()
    {
    }
    public void Leve()
    {
 
        if (MultiPlayerSkin.m_sinThis)
        {
            Debug.Log("asdsad");
            MultiPlayerSkin.m_sinThis.LeveFromToMainMenu();
        }
        else
        {
            Debug.Log("CC");
            Screen_Loader.Load_Async_Scene(0);
            PhotonNetwork.Disconnect();
        }
    }
}
