using UnityEngine.AI;

public class DPSequence : Node
{
    private BehaviourTree dependency;
    private NavMeshAgent agent;

    public DPSequence(string nodeName, BehaviourTree treeDependency, NavMeshAgent navMeshAgent): base(nodeName)
    {
        dependency = treeDependency;
        agent = navMeshAgent;
    }

    public override Status Process()
    {

        if (dependency.Process() == Status.FAILURE) 
        {
            agent.ResetPath();
            foreach (Node node in children)
            {
                node.Reset();
            }

            return Status.FAILURE;
        }

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
