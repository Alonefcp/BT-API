using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invert : Node
{
    public Invert(string nodeName): base(nodeName)
    {

    }

    public override Status Process()
    {
        Status childStatus = children[0].Process();

        if(childStatus == Status.RUNNING)
        {
            return Status.RUNNING;
        }
        else if(childStatus == Status.FAILURE)
        {
            return Status.SUCCESS;
        }
        else
        {
            return Status.FAILURE;
        }
    }
}
