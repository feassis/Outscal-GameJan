using UnityEngine;
using UnityEngine.AI;

public class SnailView : MonoBehaviour
{
    private SnailController controller;

    private NavMeshAgent agent;

    public void SetController(SnailController controller)
    {
        this.controller = controller;
    }

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, controller.GetTarget().position);

        if (distanceToPlayer <= controller.GetFollowRange())
        {
            agent.SetDestination(controller.GetTarget().position);
        }
        else
        {
            agent.ResetPath();
        }
    }
}

