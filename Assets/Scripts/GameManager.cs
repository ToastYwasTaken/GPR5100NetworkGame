<<<<<<< Updated upstream
=======
using System.Collections;
using System.Collections.Generic;
>>>>>>> Stashed changes
using UnityEngine;

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
public class GameManager : MonoBehaviour
{
<<<<<<< Updated upstream
    public static GameManager instance;

    [SerializeField]
    private CursorLockMode m_LockMode = CursorLockMode.Locked;

    [Header("Enemys")]
    [SerializeField]
    private GameObject m_EnemyPrefab;
    [SerializeField]
    private int m_NumberOfEnemy = 0;
    [SerializeField]
    private float m_SpawnDistance = 5f;
    [SerializeField]
    private Transform[] m_Spawnpoint;

    private Spawner spawner = new Spawner();

    private void Awake()
    {
        if (!instance) instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        Cursor.lockState = m_LockMode;
    }

    public void SpawnEnemys()
    {
        for (int i = 0; i < m_Spawnpoint.Length; i++)
        {
            spawner.Spawn(m_EnemyPrefab, m_NumberOfEnemy, m_Spawnpoint[i], m_SpawnDistance);
        }

    }
=======
    
>>>>>>> Stashed changes
}
