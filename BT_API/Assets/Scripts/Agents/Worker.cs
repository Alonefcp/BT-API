using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worker : BTAgent
{
    [SerializeField]
    private GameObject office;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();  
        
        Leaf goToPatron = new Leaf("Go to patron", GoToPatron);
        Leaf goToOffice = new Leaf("Go to patron", GoToOffice);

        Selector beWorker = new Selector("Be wortker");
        beWorker.AddChild(goToPatron);
        beWorker.AddChild(goToOffice);

        behaviourTree.AddChild(beWorker);
    }

    public Node.Status GoToPatron()
    {
        if(Blackboard.Instance.Patron() == null)
        {
            return Node.Status.FAILURE;
        }

        Node.Status status = GoToLocation(Blackboard.Instance.Patron().transform.position);
        if (status == Node.Status.SUCCESS)
        {
            Blackboard.Instance.Patron().GetComponent<PatronBehaviour>().SetHasATicket(true);
            Blackboard.Instance.DeregisterPatron();

        }

        return status;

    }

    public Node.Status GoToOffice()
    {
        Node.Status status = GoToLocation(office.transform.position);
        return status;
    }
}
