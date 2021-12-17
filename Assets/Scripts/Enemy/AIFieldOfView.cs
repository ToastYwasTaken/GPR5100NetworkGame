using UnityEngine;

/*****************************************************************************
* Project: GPR5100 Networkgame
* File   : AIFieldOfView.cs
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

public class AIFieldOfView : StateMachineBehaviour
{
    [Header("Viewfield Setting")]
    [SerializeField, Range(0f, 360f)]
    protected float m_viewAngle = 120f;
    [SerializeField, Range(0f, 1f)]
    protected float m_viewOffset = 0.5f;
    [SerializeField]
    protected float m_viewDistance = 100f;
    [SerializeField]
    protected float m_auraDistance = 5f;
    [SerializeField]
    protected LayerMask m_LayerForViewField = 1;
    [SerializeField]
    protected QueryTriggerInteraction queryTrigger = QueryTriggerInteraction.Ignore;

    [Header("Scanfield Setting")]
    [SerializeField]
    protected float m_ScanRadius = 50f;
    [SerializeField]
    protected LayerMask m_LayerForScanField = 1;

    /// <summary>
    /// Erzeugt eine Sphere und gibt alle Collider innerhalb zurueck
    /// </summary>
    /// <param name="_originTransform"></param>
    /// <returns></returns>
    protected Collider[] LookAroundForColliders(Transform _originTransform)
    {
        Vector3 origin = _originTransform.position;
        Debug.DrawLine(origin, new Vector3(origin.x + m_ScanRadius, origin.y, origin.z), Color.cyan);
        Debug.DrawLine(origin, new Vector3(origin.x, origin.y, origin.z + m_ScanRadius), Color.cyan);
        return Physics.OverlapSphere(origin, m_ScanRadius, m_LayerForScanField, queryTrigger);
    }

    /// <summary>
    /// Durchsucht das uebergebene Collider-Array nach dem TAG und gibt es als GameObject zurueck
    /// </summary>
    /// <param name="_colliders"></param>
    /// <param name="_objectTag"></param>
    /// <returns></returns>
    protected GameObject LookForObject(Collider[] _colliders, string _objectTag)
    {
        foreach (Collider collider in _colliders)
        {
            if (collider.CompareTag(_objectTag))
            {
                return collider.gameObject;
            }
        }
        return null;
    }

    protected GameObject[] LookForObjects(Collider[] _colliders, string _objectTag)
    {
        GameObject[] objects = new GameObject[_colliders.Length];
        int counter = 0;

        foreach (Collider collider in _colliders)
        {
            if (collider.CompareTag(_objectTag))
            {
                objects[counter] = collider.gameObject;
                counter++;
            }
        }

        GameObject[] gameObjects = new GameObject[counter];

        for (int i = 0; i < gameObjects.Length; i++)
        {
            gameObjects[i] = objects[i];
        }

        return gameObjects;

    }
  
}
