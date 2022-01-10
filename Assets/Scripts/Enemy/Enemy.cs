using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*****************************************************************************
* Project: GPR5100 Networkgame
* File   : Enemy.cs
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
public class Enemy : MonoBehaviour
{
    // Events
    public event OnAIEventHandler OnEnemyDieEvent;

    private Animator animator;

    // States
    private AIBehaviourCloseAttack attackState;

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

    [SerializeField]
    private float m_damage = 33f;
    public float Damage
    {
        get => m_damage;
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        attackState = animator.GetBehaviour<AIBehaviourCloseAttack>();
        attackState.OnAttackPlayer += AttackState_OnAttackPlayer;
    }

    private void AttackState_OnAttackPlayer()
    {
        Debug.Log($"{gameObject.name} attack Player!");
    }

    /// <summary>
    /// Gegner erhaelt schaden
    /// </summary>
    /// <param name="_damage"></param>
    public void TakeDamage(float _damage)
    {
        if (Health >= 0f)
        {
            Health -= _damage;
        }
    }


    /// <summary>
    /// Enemy zerstoeren
    /// </summary>
    private void Die()
    {
        OnEnemyDieEvent?.Invoke();
        PhotonNetwork.Destroy(gameObject);
    }
}
