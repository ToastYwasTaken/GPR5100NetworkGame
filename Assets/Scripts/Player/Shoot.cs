using Photon.Pun;
using UnityEngine;

/*****************************************************************************
* Project: GPR5100 Networkgame
* File   : Shoot.cs
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

public class Shoot : MonoBehaviourPun
{
    [SerializeField] private Transform m_RayOrigin;
    [SerializeField] private float m_RayLength;

    private void Update()
    {
        if (!photonView.IsMine) return;

        if (GameManager.instance.IsPause) return;

        if (Controls.MouseButtonLeft())
        {        
            photonView.RPC("Shooting", RpcTarget.All);
        }

        if (Controls.MouseButtonRight())
        {
            Debug.Log("Player Scope!");
        }
    }

    [PunRPC]
    private void Shooting()
    {
        RaycastHit hit;

        Debug.Log("Fire");
        if (Physics.Raycast(m_RayOrigin.position, m_RayOrigin.TransformDirection(Vector3.forward),
                out hit, m_RayLength))
        {
            Debug.DrawRay(m_RayOrigin.position, m_RayOrigin.TransformDirection(Vector3.forward) * hit.distance, Color.red);
            Debug.Log($"Player hit {hit.collider.name}");
        }
        else
        {
            Debug.DrawRay(m_RayOrigin.position, m_RayOrigin.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
           
        }
    }

    private void Fire(WeaponType weapon)
    {
       

    }

}
