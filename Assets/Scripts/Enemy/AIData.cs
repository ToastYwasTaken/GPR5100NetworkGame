using System.Collections.Generic;
using UnityEngine;

/*****************************************************************************
* Project: GPR5100 Networkgame
* File   : AIData.cs
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
public static class AIData
{
    public static int hashIdle = Animator.StringToHash("Idle");
    public static int hashChase = Animator.StringToHash("Chase");
    // public static int hashFarAttack = Animator.StringToHash("FarAttack");
    public static int hashCloseAttack = Animator.StringToHash("CloseAttack");

    public static string tagPlayer = "Player";
    public static string tagEnemy = "Enemy";

    private static List<Player> players = new List<Player>();

    public static void AddPlayer(Player _player)
    {
        if (!players.Contains(_player))
        {
            players.Add(_player);
        }
    }

    public static void RemovePlayer(Player _player)
    {
        if (players.Contains(_player))
        {
            players.Remove(_player);
        }
    }

    public static List<Player> GetPlayers()
    {
        return players;
    }



}
