using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BTAgent : MonoBehaviour
{
    public enum ActionState { IDLE, WORKING};

    protected ActionState state = ActionState.IDLE;
    protected NavMeshAgent agent;
    protected BehaviourTree behaviourTree;
    protected Node.Status treeStatus = Node.Status.RUNNING;

    private WaitForSeconds waitForSeconds;
    private Vector3 rememberedLocation;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        behaviourTree = new BehaviourTree();
        waitForSeconds = new WaitForSeconds(Random.Range(0.1f, 1.0f));
        StartCoroutine("Behave");
    }

    protected IEnumerator Behave()
    {
        while (true) 
        {
            treeStatus = behaviourTree.Process();
            yield return waitForSeconds;
        }
    }


    // Update is called once per frame
    //protected void Update()
    //{
    //    if (treeStatus != Node.Status.SUCCESS)
    //    {
    //        treeStatus = behaviourTree.Process();
    //    }
    //}

    public Node.Status CanSee(Vector3 target, string targetTag, float distance, float maxAngle)
    {
        Vector3 directionToTarget = target - transform.position;
        float angle = Vector3.Angle(directionToTarget, transform.forward);

        if (angle <= maxAngle || directionToTarget.magnitude <= distance) 
        {
            RaycastHit hitInfo;
            if (Physics.Raycast(transform.position, directionToTarget, out hitInfo)) 
            {
                if(hitInfo.collider.gameObject.CompareTag(targetTag))
                {
                    return Node.Status.SUCCESS;
                }
            }
        }

        return Node.Status.FAILURE;
    }

    public Node.Status Flee(Vector3 location, float distance)
    {
        if(state == ActionState.IDLE)
        {
            rememberedLocation = transform.position + (transform.position - location).normalized * distance;
        }

        return GoToLocation(rememberedLocation);
    }

    protected Node.Status GoToLocation(Vector3 destination)
    {
        float distanceToTarget = Vector3.Distance(destination, transform.position);

        if (state == ActionState.IDLE)
        {
            agent.SetDestination(destination);
            state = ActionState.WORKING;
        }
        else if (Vector3.Distance(agent.pathEndPosition, destination) >= 2.0f)
        {
            state = ActionState.IDLE;
            return Node.Status.FAILURE;
        }
        else if (distanceToTarget < 2.0f)
        {
            state = ActionState.IDLE;
            return Node.Status.SUCCESS;
        }

        return Node.Status.RUNNING;
    }

    protected Node.Status GoToDoor(GameObject door)
    {
        Node.Status status = GoToLocation(door.transform.position);

        if (status == Node.Status.SUCCESS)
        {
            if (!door.GetComponent<Lock>().IsLocked)
            {
                door.GetComponent<NavMeshObstacle>().enabled = false;
                //door.SetActive(false);
                return Node.Status.SUCCESS;
            }
            else
            {
                return Node.Status.FAILURE;
            }
        }
        else
        {
            return status;
        }

    }

    protected Node.Status IsOpen()
    {

        if (Blackboard.Instance.TimeOfDay() < 9 || Blackboard.Instance.TimeOfDay() > 17)
        {
            return Node.Status.FAILURE;
        }
        else
        {
            return Node.Status.SUCCESS;
        }
    }
}
