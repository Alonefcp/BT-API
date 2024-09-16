using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leaf : Node
{
    public delegate Status Tick();
    public Tick ProcessMethod;

    public Leaf() { }

    public Leaf(string nodeName, Tick processMethod): base(nodeName) 
    {
        ProcessMethod = processMethod;
    }

    public Leaf(string nodeName, Tick processMethod, int order): base(nodeName)
    {
        ProcessMethod = processMethod;
        sortOrder = order;
    }

    public override Status Process()
    {
        if(ProcessMethod != null)
        {
             return ProcessMethod();
        }
        else 
        {
            return Status.FAILURE;
        }
    }
}
