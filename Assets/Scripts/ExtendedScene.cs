using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public delegate void OnLoadingSceneProc(float _progress);
public delegate void OnLoadingSceneCompleted(bool _completed);

/*****************************************************************************
* Project: GPR5100 Networkgame
* File   : ExtendedScene.cs
* Date   : 05.01.2022
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
*	05.01.22	RK	Created
******************************************************************************/
public class ExtendedScene
{
    /// <summary>
    /// Meldet den aktuellen Lade Fortschritt.
    /// </summary>
    public static event OnLoadingSceneProc OnLoadedSceneProcecss;
    /// <summary>
    /// Meldet das vollständige Laden der Scene.
    /// </summary>
    public static event OnLoadingSceneCompleted OnLoadedSceneCompleted;

    private static AsyncOperation operation;
    private static float operationProgress = 0;

    /// <summary>
    /// ACHTUNG: Muss in einer Coroutine aufgerufen werden.
    /// Lädt die übergebene Scene asynchron und gibt den Fortschritt über Events zurück.
    /// </summary>
    /// <param name="_idx"></param>
    /// <returns></returns>
    public static IEnumerator Loading(int _idx)
    {
        yield return null;
        operation = SceneManager.LoadSceneAsync(_idx);
        operation.allowSceneActivation = false;

        operationProgress = 0;

        while (!operation.isDone)
        {
            operationProgress = operation.progress;

           // Debug.Log($"Loading Scene Progress: {operationProgress}");

            OnLoadedSceneProcecss?.Invoke(operationProgress);

            if (operationProgress >= 0.9f)
            {
                operation.allowSceneActivation = true;
                OnLoadedSceneCompleted?.Invoke(true);
                break;
            }
        }  
    }
}
