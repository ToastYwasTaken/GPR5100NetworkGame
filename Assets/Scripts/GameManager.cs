using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

/*****************************************************************************
* Project: GPR5100 Networkgame
* File   : GameManager.cs
* Date   : 17.12.2021
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
*	17.12.21	RK	Created
******************************************************************************/

public class GameManager : MonoBehaviourPun
{

    /// <summary>
    /// CustomProperties
    /// </summary>
    private const string CP_HEALTH = "Health";

    public static GameManager instance;

    [SerializeField] private Camera m_RoomCamera;
    [SerializeField] private CursorLockMode m_CursorLockMode = CursorLockMode.Locked;
    [SerializeField] private bool m_IsPause = false;
    public bool IsPause { get => m_IsPause; private set => m_IsPause = value; }
    


    [Header("UI")]
    [SerializeField] private GameObject m_InGamePanel;
    [SerializeField] private GameObject m_PausePanel;
   
    [Header("Enemys")]
    [SerializeField] private GameObject m_EnemyPrefab;
    [SerializeField] private int m_NumberOfEnemy = 0;
    [SerializeField] private float m_SpawnDistance = 5f;
    [SerializeField] private Transform[] m_Spawnpoint;
    private Spawner spawner = new Spawner();

    [Header("Player")]
    [SerializeField] private GameObject m_PlayerPrefab;
    [SerializeField] private float m_MaxPlayerHealth = 100f;
    [SerializeField] private Vector3 m_SpawnPosition = Vector3.zero;
    private GameObject currentPlayer;
    private Player player;

    private void Awake()
    {
        if (!instance) instance = this;
        else Destroy(gameObject);

        m_RoomCamera.gameObject.SetActive(false);

        if (PhotonNetwork.IsConnected)
        {
            Cursor.lockState = m_CursorLockMode;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }

    private void Start()
    {
        if (PhotonNetwork.IsConnected)
        {
            AIData.ClearPlayers();
            currentPlayer = PhotonNetwork.Instantiate(m_PlayerPrefab.name, m_SpawnPosition, Quaternion.identity);
           
            player = currentPlayer.GetComponent<Player>();
            player.OnPlayerDieEvent += GameManager_OnPlayerDieEvent;
            player.OnPLayerTakeDamageEvent += GameManager_OnPLayerTakeDamageEvent;
            player.Health = m_MaxPlayerHealth;

            currentPlayer.name = PhotonNetwork.NickName;

            m_InGamePanel.SetActive(true);
            m_PausePanel.SetActive(false);
        }
        else
        {
            m_InGamePanel.SetActive(false);
            m_PausePanel.SetActive(true);
        }
    }

    private void Update()
    {
        // Pause Menu aufrufen
        if (Controls.Escape())
        {
            m_IsPause = !m_IsPause;

            m_InGamePanel.SetActive(!m_IsPause);
            m_PausePanel.SetActive(m_IsPause);
        }
    }
    private void GameManager_OnPLayerTakeDamageEvent(float _x)
    {
        Debug.Log("Player take damage");     
    }

    private void GameManager_OnPlayerDieEvent()
    {
        PhotonNetwork.Destroy(currentPlayer);

        m_RoomCamera.gameObject.SetActive(true);
    }

    public void SpawnEnemys()
    {
        for (int i = 0; i < m_Spawnpoint.Length; i++)
        {
            // spawner.Spawn(m_EnemyPrefab, m_NumberOfEnemy, m_Spawnpoint[i], m_SpawnDistance);
            spawner.NetworkSpawn(m_EnemyPrefab.name, m_NumberOfEnemy, m_Spawnpoint[i], m_SpawnDistance);
        }
    }

    // Button Callbacks
    public void ResumeGame()
    {
        m_IsPause = false;

        m_InGamePanel.SetActive(!m_IsPause);
        m_PausePanel.SetActive(m_IsPause);
    }

    public void LeaveGame()
    {
        if (PhotonNetwork.InRoom)
        {
            player.OnPLayerTakeDamageEvent -= GameManager_OnPLayerTakeDamageEvent;
            player.OnPlayerDieEvent -= GameManager_OnPlayerDieEvent;
            player.Die(); //!!!
            PhotonNetwork.LeaveRoom();
            StartCoroutine(ExtendedScene.Loading(0));
        }
    }
}
