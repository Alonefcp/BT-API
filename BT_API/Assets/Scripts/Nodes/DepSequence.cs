using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DepSequence : Node
{
    private BehaviourTree dependancy;
    private NavMeshAgent agent;

    public DepSequence(string nodeName, BehaviourTree d, NavMeshAgent a) : base(nodeName)
    {
        dependancy = d;
        agent = a;
    }

    public override Status Process()
    {
        if(dependancy.Process() == Status.FAILURE)
        {           
            agent.ResetPath();

            foreach (Node node in children)
            {
                node.Reset();
            }
            return Status.FAILURE;
        }

        // Prevent main sequence from running until all dependency nodes are checked
        //if (dependancy.Process() == Status.RUNNING) { return Status.RUNNING; }

        Status childStatus = children[currentChild].Process();

        if(childStatus == Status.RUNNING)
        {
            return Status.RUNNING;
        }
        else if(childStatus == Status.FAILURE)
        {
            return childStatus;
        }

        currentChild++;
        if (currentChild >= children.Count) 
        {
            currentChild = 0;
            return Status.SUCCESS;
        }

        return Status.RUNNING;
    }
}
