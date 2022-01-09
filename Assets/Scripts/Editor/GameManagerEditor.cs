using UnityEditor;
using UnityEngine;

/*****************************************************************************
* Project: GPR5100 Networkgame
* File   : GameManagerEditor.cs
* Date   : 17.12.2021
* Author : René Kraus (RK)
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

[CustomEditor(typeof(GameManager))]
public class GameManagerEditor : Editor
{
    private GameManager gameManager;

    public override void OnInspectorGUI()
    {
        gameManager = (GameManager)target;

        if (GUILayout.Button("Spawn Enemys"))
        {
            gameManager.SpawnEnemys();

        }

        base.OnInspectorGUI();


    }
}
