using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Text nickName;

    private const int max_ccu = 8;

    private RoomOptions options = new RoomOptions(); // <- Create new RoomOptions, before set options

    [Header("Panels")]
    [SerializeField]
    private GameObject mainMenuPanel;
    [SerializeField]
    private GameObject joinFriendPanel;
    [SerializeField]
    private GameObject roomPanel;

    private void Awake()
    {
        joinFriendPanel.SetActive(false);
        roomPanel.SetActive(false);

        if (PhotonNetwork.IsConnected)
        {
            return;
        }

        PhotonNetwork.ConnectUsingSettings();
        Debug.Log("Trying to connect");
    }
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public override void OnConnected()
    {
        mainMenuPanel.SetActive(true);
        Debug.Log("Connected");
        SetOptionsRandomLobby(); // <- Call here (not by joining the lobby)
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Joined lobby");
       // SetOptionsRandomLobby(); -> Move to Callback OnConnected() 
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Joining random room failed");
        
        PhotonNetwork.CreateRoom(null, options);
    }

    public override void OnJoinedRoom()
    {
        mainMenuPanel.SetActive(false);
        roomPanel.SetActive(true);
        joinFriendPanel.SetActive(false);
        Debug.Log("Joined room");
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("Created new room");
    }

    #region Buttons
    public void BTN_ConnectToRandomLobby()
    {
        Debug.Log("Trying to join random server");
        PhotonNetwork.JoinRandomRoom();
        SetNickName();
    }

    public void BTN_TryJoiningAFriend()
    {
        
    }

    public void BTN_GoToFriendMenu()
    {
        mainMenuPanel.SetActive(false);
        joinFriendPanel.SetActive(true);
    }

    public void BTN_BackToMainMenu()
    {
        mainMenuPanel.SetActive(true);
        joinFriendPanel.SetActive(false);
        roomPanel.SetActive(false);
    }

    public void BTN_StartGameIfLobbyLeader()
    {
        StartCoroutine(ExtendedScene.Loading(1)); // <- Loading Game Scene
    }


    #endregion
    public void SetNickName()
    {
        if (nickName.text == null)
        {
            //PhotonNetwork.NickName = RandomNameGenerator.GenerateRandomName();
        }
        PhotonNetwork.NickName = nickName.text;
        Debug.Log("Nickname is: " + PhotonNetwork.NickName);
    }

    private void SetOptionsRandomLobby()
    {
        options.MaxPlayers = max_ccu;
        options.IsOpen = true;  //players can join until closed or maxCCU reached
        options.IsVisible = true; 
    }

    
}
