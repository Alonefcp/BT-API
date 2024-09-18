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
}
