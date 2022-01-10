using UnityEngine;
using UnityEngine.AI;

public class AIBehaviourChase : AIFieldOfView
{
    private NavMeshAgent agent = null;

    [Header("Attack Properties")]
    [SerializeField]
    private float attackDistance = 2.5f;
    [SerializeField]
    private float attackSpeed = 5f;
    [SerializeField]
    private float stoppingdistance = 2f;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (agent == null)
        {
            agent = animator.GetComponent<NavMeshAgent>();
        }

        agent.stoppingDistance = stoppingdistance;
        agent.speed = attackSpeed;

        // CheckEnvironmentForPlayers(animator.gameObject.transform);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Vector3 originPos = animator.gameObject.transform.position;
       
        Player player = GetClosestPlayer(animator);

        if (player != null)
        {
            agent.SetDestination(player.transform.position);
            if (CanAttack())
            {
                animator.SetTrigger(AIData.hashCloseAttack);
            }
        }
        else
        {
            agent.SetDestination(originPos);
            animator.SetBool(AIData.hashIdle, true);
            animator.SetBool(AIData.hashChase, false);
        }
    }

    private bool CanAttack()
    {
        return agent.remainingDistance <= attackDistance;
    }

    private Player GetClosestPlayer(Animator _animator)
    {
        Player[] players = AIData.GetPlayers().ToArray();
        Player closetPlayer = null;

        Vector3 originPos = _animator.gameObject.transform.position;

        if (players.Length > 0)
        {
            float closetDistance = (originPos - players[0].transform.position).sqrMagnitude;

            for (int i = 0; i < players.Length; i++)
            {
                if (players[i] == null)
                {
                    AIData.TrimPlayers();
                    continue;
                }

                Vector3 separation = originPos - players[i].transform.position;
                float distance = separation.sqrMagnitude;

                if (distance <= closetDistance)
                {
                    closetDistance = distance;
                    closetPlayer = players[i];
                }
            }
        }
        else
        {
            closetPlayer = null;
        }
        return closetPlayer;
    }

    private void CheckEnvironmentForPlayers(Transform _originTransform)
    {
        Collider[] colliders = LookAroundForColliders(_originTransform);

        GameObject[] playerObjects = LookForObjects(colliders, AIData.tagPlayer);

        for (int i = 0; i < playerObjects.Length; i++)
        {
            AIData.AddPlayer(playerObjects[i].GetComponent<Player>());
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
