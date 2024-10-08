using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sequence : Node
{
    public Sequence(string nodeName): base(nodeName)
    {

    }

    public override Status Process()
    {
        Status childStatus = children[currentChild].Process();

        if(childStatus == Status.RUNNING)
        {
            return Status.RUNNING;
        }
        else if(childStatus == Status.FAILURE)
        {
            currentChild = 0;

            foreach (Node child in children) 
            {
                child.Reset();
            }

            return Status.FAILURE;
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
