using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*****************************************************************************
* Project: GPR5100 Networkgame
* File   : Spawner.cs
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
public class Spawner 
{
    public GameObject Spawn(GameObject _prefab, Transform _spawnOrigin)
    {
        Quaternion spawnRotation = Quaternion.Euler(0f, Random.Range(0, 360), 0);
        GameObject obj = Object.Instantiate(_prefab, _spawnOrigin.position, spawnRotation);
        return obj;
    }

    public GameObject[] Spawn(GameObject _prefab, int _count, Transform _spawnOrigin, float _spawnDistance)
    {
        GameObject[] spawns = new GameObject[_count];

        for (int i = 0; i < _count; i++)
        {
            Vector3 spawnPos = _spawnOrigin.position + new Vector3(
                Random.Range(-_spawnDistance, _spawnDistance),
                0f,
                Random.Range(-_spawnDistance, _spawnDistance));

            Quaternion spawnRotation = Quaternion.Euler(0f, Random.Range(0, 360), 0);
            spawns[i]  = Object.Instantiate(_prefab, spawnPos, spawnRotation);
        }

        return spawns;
    }

}
