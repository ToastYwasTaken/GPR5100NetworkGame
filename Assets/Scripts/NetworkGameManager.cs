using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.UI;

/*****************************************************************************
* Project: GPR5100 Networkgame
* File   : NetworkManager.cs
* Date   : 06.01.2022
* Author : René Kraus (RK)
* Version: 1.0
*
* These coded instructions, statements, and computer programs contain
* proprietary information of the author and are protected by Federal
* copyright law. They may not be disclosed to third parties or copied
* or duplicated in any form, in whole or in part, without the prior
* written consent of the author.
*
* History:
*	31.12.22	RK	Created
******************************************************************************/
public class NetworkGameManager : MonoBehaviourPunCallbacks
{
    public static NetworkGameManager instance;

    /// <summary>
    /// CustomProperties
    /// </summary>
    private const string CP_HEALTH = "Health";

    [Header("UI")]
    [SerializeField] private TMP_Text[] playerNamesText = new TMP_Text[4];
    [SerializeField] private Image[] playerHealthbarsImage = new Image[4];

    public Dictionary<int, Photon.Realtime.Player> players = new Dictionary<int, Photon.Realtime.Player>();
    

    private void Awake()
    {
        if (!instance) instance = this;
        else Destroy(gameObject);

    }

    private void Start()
    {
        RPC_SetPlayerNames();
        SetPlayersDictionary();

        //photonView.RPC("RPC_SetHealthBar", RpcTarget.AllBuffered, 50, 100);


    }

    //PhotonNetwork.LocalPlayer.SetCustomProperties(CustomProperty(CP_HEALTH, _x));

    //    float customHealthProperty = (float)PhotonNetwork.LocalPlayer.CustomProperties["Health"];

    //photonView.RPC("SetHealthBar", RpcTarget.All, _x, m_MaxPlayerHealth);

    private void SetPlayersDictionary()
    {
        players.Clear();    

        players = PhotonNetwork.CurrentRoom.Players;
    }




    private Hashtable CustomProperty<T>(string _name, T _value)
    {
        return new Hashtable() { { _name, _value } };
    }

    [PunRPC]
    private void RPC_SetPlayerNames()
    {
        int playerCount = PhotonNetwork.CurrentRoom.Players.Count;

        for (int i = 0; i < playerNamesText.Length; i++)
        {
            playerNamesText[i].text = "";
        }


        for (int i = 0; i < playerCount; i++)
        {
            playerNamesText[i].text = PhotonNetwork.CurrentRoom.Players[i + 1].NickName;
        }
    }

    [PunRPC]
    private void RPC_SetHealthBar(float _currentHealth, float _maxHealth)
    {
        int playerNumber = PhotonNetwork.LocalPlayer.ActorNumber - 1;

        playerHealthbarsImage[playerNumber].fillMethod = Image.FillMethod.Horizontal;
        playerHealthbarsImage[playerNumber].type = Image.Type.Filled;
        playerHealthbarsImage[playerNumber].fillAmount = _currentHealth / _maxHealth;
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("I joined!");   
       // SetPlayersDictionary();

       
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        Debug.Log($"{newPlayer.NickName} joined!");
        SetPlayersDictionary();
        RPC_SetPlayerNames();
        
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        SetPlayersDictionary();
        RPC_SetPlayerNames();
    }

    public override void OnPlayerPropertiesUpdate(Photon.Realtime.Player targetPlayer, Hashtable changedProps)
    {
        Debug.Log("Properties Updated!");
    }
}
