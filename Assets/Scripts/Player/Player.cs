using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*****************************************************************************
* Project: GPR5100 Networkgame
* File   : Player.cs
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

public class Player : MonoBehaviour
{
    public event OnPlayerEventHandler OnPlayerDieEvent;

    [SerializeField]
    private bool m_GodMode = false;

    [SerializeField]
    private float m_health = 100f;
    public float Health
    {
        get => m_health;
        set
        {
            m_health = value;
            if (m_health <= 0) Die();
        }
    }

    private void Awake()
    {
        
    }


    /// <summary>
    /// Player zerstoeren
    /// </summary>
    private void Die()
    {
        OnPlayerDieEvent?.Invoke();

        AIData.RemovePlayer(this);

        Destroy(gameObject);
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (m_GodMode) return;

        if (collision.gameObject.CompareTag(AIData.tagEnemy))
        {
            Health -= collision.gameObject.GetComponent<Enemy>().Damage;
        }
    }

}
