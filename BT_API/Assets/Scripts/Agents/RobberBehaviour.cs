using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RobberBehaviour : BTAgent
{
    [SerializeField]
    private GameObject diamond;

    [SerializeField]
    private GameObject van;

    [SerializeField]
    private GameObject backDoor;

    [SerializeField]
    private GameObject frontDoor;

    [SerializeField]
    [Range(0,1000)]
    private int money = 800;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
      
        Sequence steal = new Sequence("Steal");
        Leaf hasGotMoney = new Leaf("Has Got Money", HasMoney);
        Leaf goToBackDoor = new Leaf("Go To Back Door", GoToBackDoor);
        Leaf goToFrontDoor = new Leaf("Go To Front Door", GoToFrontDoor);
        Leaf goToDiamond = new Leaf("Go To Diamond", GoToDiamond);
        Leaf goToVan = new Leaf("Go To Van", GoToVan);
        Selector openDoor = new Selector("Open Door");

        Invert invertMoney = new Invert("Invert Money");
        invertMoney.AddChild(hasGotMoney);

        openDoor.AddChild(goToFrontDoor);
        openDoor.AddChild(goToBackDoor); 
        steal.AddChild(invertMoney);
        steal.AddChild(openDoor);
        steal.AddChild(goToDiamond);       
        steal.AddChild(goToVan);
        behaviourTree.AddChild(steal);

        behaviourTree.Print();         
    }

    public Node.Status GoToDiamond()
    {
        Node.Status status = GoToLocation(diamond.transform.position);

        if (status == Node.Status.SUCCESS) 
        {
            diamond.gameObject.transform.parent = gameObject.transform;
        }
        
        return status;
        
    }

    public Node.Status HasMoney()
    {
        if(money < 500)
        {
            return Node.Status.FAILURE;
        }
        else
        {
            return Node.Status.SUCCESS;
        }
    }

    public Node.Status GoToVan()
    {      
        Node.Status status = GoToLocation(van.transform.position);
        if(status == Node.Status.SUCCESS)
        {
            money += 300;
            Destroy(diamond);
        }

        return status;
    }

    public Node.Status GoToBackDoor()
    {
        return GoToDoor(backDoor);
    }

    public Node.Status GoToFrontDoor()
    {
        return GoToDoor(frontDoor);
    }

    public Node.Status GoToDoor(GameObject door)
    {
        Node.Status status = GoToLocation(door.transform.position);

        if (status == Node.Status.SUCCESS) 
        {
            if(!door.GetComponent<Lock>().IsLocked)
            {
                door.SetActive(false);
                return Node.Status.SUCCESS;
            }
            else
            {
                return Node.Status.FAILURE;
            }
        }
        else
        {
            return status;
        }

    }

    //public Node.Status GoToLocation(Vector3 destination)
    //{
    //    float distanceToTarget = Vector3.Distance(destination, transform.position);

    //    if (state == ActionState.IDLE)
    //    {
    //        agent.SetDestination(destination);
    //        state = ActionState.WORKING;
    //    }
    //    else if (Vector3.Distance(agent.pathEndPosition, destination) >= 2.0f)
    //    {
    //        state = ActionState.IDLE;
    //        return Node.Status.FAILURE;
    //    }
    //    else if (distanceToTarget < 2.0f)
    //    {
    //        state = ActionState.IDLE;
    //        return Node.Status.SUCCESS;
    //    }

    //    return Node.Status.RUNNING;
    //}
}
