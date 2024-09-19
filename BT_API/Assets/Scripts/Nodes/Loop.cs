using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loop : Node
{

    private BehaviourTree dependancy;

    public Loop(string nodeName, BehaviourTree d) : base(nodeName)
    {
        dependancy = d;
    }

    public override Status Process()
    {
        if (dependancy.Process() == Status.FAILURE) 
        {
            return Status.SUCCESS;
        }


        Status childStatus = children[currentChild].Process();

        if (childStatus == Status.RUNNING)
        {
            return Status.RUNNING;
        }
        else if (childStatus == Status.FAILURE)
        {
            //currentChild = 0;

            //foreach (Node child in children)
            //{
            //    child.Reset();
            //}

            return Status.FAILURE;
        }

        currentChild++;
        if (currentChild >= children.Count)
        {
            currentChild = 0;           
        }

        return Status.RUNNING;
    }
}
