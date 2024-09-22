using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worker : BTAgent
{
    [SerializeField]
    private GameObject office;

    private GameObject patron;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();  
        
        Leaf goToPatron = new Leaf("Go to patron", GoToPatron);
        Leaf goToOffice = new Leaf("Go to patron", GoToOffice);
        Leaf allocatePatron = new Leaf("Allocate patron", AllocatePatron);
        Leaf patronStillWaiting = new Leaf("Patron still waiting?", PatronWaiting);

        Sequence getPatron = new Sequence("Get Patron");
        getPatron.AddChild(allocatePatron);

        BehaviourTree waiting = new BehaviourTree();
        waiting.AddChild(patronStillWaiting);

        DepSequence moveToPatron = new DepSequence("Move to patron", waiting, agent);
        moveToPatron.AddChild(goToPatron);

        getPatron.AddChild(moveToPatron);

        Selector beWorker = new Selector("Be wortker");
        beWorker.AddChild(getPatron);
        beWorker.AddChild(goToOffice);

        behaviourTree.AddChild(beWorker);
    }

    public Node.Status PatronWaiting()
    {
        if(patron==null)
        {
            return Node.Status.FAILURE;
        }
        else if(patron.GetComponent<PatronBehaviour>().PatronIsWaiting())
        {
            return Node.Status.SUCCESS;
        }
        return Node.Status.FAILURE;
    }

    public Node.Status AllocatePatron()
    {
        if (Blackboard.Instance.Patrons().Count == 0)
        {
            return Node.Status.FAILURE;
        }

        patron = Blackboard.Instance.Patrons().Pop();

        if (patron == null)
        {
            return Node.Status.FAILURE;
        }
        else
        {
            return Node.Status.SUCCESS;
        }
    }

    public Node.Status GoToPatron()
    {

        if (patron == null)
        {
            return Node.Status.FAILURE;
        }

        Node.Status status = GoToLocation(patron.transform.position);
        if (status == Node.Status.SUCCESS)
        {
            patron.GetComponent<PatronBehaviour>().SetHasATicket(true);
            patron = null;
        }

        return status;

    }

    public Node.Status GoToOffice()
    {
        patron = null;
        Node.Status status = GoToLocation(office.transform.position);
        return status;
    }
}
