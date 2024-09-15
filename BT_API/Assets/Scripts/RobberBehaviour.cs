using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RobberBehaviour : MonoBehaviour
{
    [SerializeField]
    private GameObject diamond;

    [SerializeField]
    private GameObject van;

    private NavMeshAgent agent;
    private BehaviourTree behaviourTree;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        behaviourTree = new BehaviourTree();

        Node steal = new Node("Steal");
        Leaf goToDiamond = new Leaf("Go To Diamond", GoToDiamond);
        Leaf goToVan = new Leaf("Go To Van", GoToVan);

        steal.AddChild(goToDiamond);
        steal.AddChild(goToVan);
        behaviourTree.AddChild(steal);

        behaviourTree.Print();

        behaviourTree.Process();    
    }

    public Node.Status GoToDiamond()
    {
        agent.SetDestination(diamond.transform.position);
        return Node.Status.SUCCESS;
    }

    public Node.Status GoToVan()
    {
        agent.SetDestination(van.transform.position);
        return Node.Status.SUCCESS;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
