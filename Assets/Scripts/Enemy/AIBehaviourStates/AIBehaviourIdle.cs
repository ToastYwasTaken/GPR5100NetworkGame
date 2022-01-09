using UnityEngine;

public class AIBehaviourIdle : AIFieldOfView
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{

    //}

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (CheckEnvironmentForPlayers(animator.transform))
        {
            animator.SetBool(AIData.hashChase, true);
            animator.SetBool(AIData.hashIdle, false);

        }
    }

    private bool CheckEnvironmentForPlayers(Transform _originTransform)
    {
        Collider[] colliders = LookAroundForColliders(_originTransform);

        GameObject[] playerObjects = LookForObjects(colliders, AIData.tagPlayer);

        for (int i = 0; i < playerObjects.Length; i++)
        {
            AIData.AddPlayer(playerObjects[i].GetComponent<Player>());
        }

        if (playerObjects.Length > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
