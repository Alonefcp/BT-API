using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RobberBehaviour : BTAgent
{
    [SerializeField]
    private GameObject diamond;

    [SerializeField]
    private GameObject painting;

    [SerializeField]
    private GameObject[] art;

    [SerializeField]
    private GameObject van;

    [SerializeField]
    private GameObject backDoor;

    [SerializeField]
    private GameObject frontDoor;

    [SerializeField]
    private GameObject cop;

    [SerializeField]
    [Range(0,1000)]
    private int money = 800;

    private GameObject pickUp = null;

    private Leaf goToBackDoor = null;
    private Leaf goToFrontDoor = null;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        Leaf hasGotMoney = new Leaf("Has Got Money", HasMoney);

        goToBackDoor = new Leaf("Go To Back Door", GoToBackDoor, 2);
        goToFrontDoor = new Leaf("Go To Front Door", GoToFrontDoor, 1); 

        //Leaf goToDiamond = new Leaf("Go To Diamond", GoToDiamond,1);
        //Leaf goToPainting = new Leaf("Go To Painting", GoToPainting,2);

        Leaf goToVan = new Leaf("Go To Van", GoToVan);

        PSelector openDoor = new PSelector("Open Door");

        RSelector selectObject = new RSelector("Select object to steal");

        for (int i = 0; i < art.Length; i++)
        {
            Leaf goToArt = new Leaf("Go To Art " + art[i].name, GoToArt, i);
            selectObject.AddChild(goToArt);
        }

        Sequence runAway = new Sequence("Run away");
        Leaf canSeeCop = new Leaf("Can see cop", CanSeeCop);
        Leaf fleeFromCop = new Leaf("Flee from cop", FleeFromCop);

        Invert invertMoney = new Invert("Invert Money");
        invertMoney.AddChild(hasGotMoney);

        openDoor.AddChild(goToFrontDoor);
        openDoor.AddChild(goToBackDoor);

        Invert cantSeeCop = new Invert("Cant see coop");
        cantSeeCop.AddChild(canSeeCop);

        Sequence s1 = new Sequence("S1");
        s1.AddChild(invertMoney);
        Sequence s2 = new Sequence("S2");
        s2.AddChild(cantSeeCop);
        s2.AddChild(openDoor);

        Sequence s3 = new Sequence("S3");
        s3.AddChild(cantSeeCop);
        s3.AddChild(selectObject);

        Sequence s4 = new Sequence("S4");
        s4.AddChild(cantSeeCop);
        s4.AddChild(goToVan);

        //steal.AddChild(s1);
        //steal.AddChild(s2);
        //steal.AddChild(s3);
        //steal.AddChild(s4);

        BehaviourTree seeCop = new BehaviourTree();
        seeCop.AddChild(cantSeeCop);
        DPSequence steal = new DPSequence("Steal", seeCop, agent);
        steal.AddChild(invertMoney);
        steal.AddChild(openDoor);
        steal.AddChild(selectObject);
        steal.AddChild(goToVan);

        runAway.AddChild(canSeeCop);
        runAway.AddChild(fleeFromCop);

        Selector beThief = new Selector("Be thief");
        beThief.AddChild(steal);
        beThief.AddChild(runAway);

        behaviourTree.AddChild(beThief);

        behaviourTree.Print();         
    }

    public Node.Status GoToDiamond()
    {
        if (!diamond.activeSelf) //we have sold the diamond
        {
            return Node.Status.FAILURE;
        }

        Node.Status status = GoToLocation(diamond.transform.position);

        if (status == Node.Status.SUCCESS) 
        {
            diamond.transform.parent = gameObject.transform;
            pickUp = diamond;
        }
        
        return status;       
    }

    public Node.Status GoToPainting()
    {
        if (!painting.activeSelf) //we have sold the painting
        {
            return Node.Status.FAILURE;
        }

        Node.Status status = GoToLocation(painting.transform.position);

        if (status == Node.Status.SUCCESS)
        {
            painting.transform.parent = gameObject.transform;
            pickUp = painting;
        }

        return status;

    }

    public Node.Status GoToArt(int index)
    {
        if (!art[index].activeSelf) //we have sold the art
        {
            return Node.Status.FAILURE;
        }

        Node.Status status = GoToLocation(art[index].transform.position);

        if (status == Node.Status.SUCCESS)
        {
            art[index].transform.parent = gameObject.transform;
            pickUp = art[index];
        }

        return status;
    }

   
    public Node.Status CanSeeCop()
    {
        return CanSee(cop.transform.position, "Cop", 10.0f, 90.0f);
    }

    public Node.Status FleeFromCop()
    {
        return CanFlee(cop.transform.position, 10.0f);
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
            
            pickUp.SetActive(false);
        }

        return status;
    }

    public Node.Status GoToBackDoor()
    {
        Node.Status status = GoToDoor(backDoor);

        if (status == Node.Status.FAILURE)
        {
            goToBackDoor.sortOrder = 10;
        }
        else 
        {
            goToBackDoor.sortOrder = 1;
        }

        return status;
    }

    public Node.Status GoToFrontDoor()
    {
        Node.Status status = GoToDoor(frontDoor);

        if (status == Node.Status.FAILURE)
        {
            goToFrontDoor.sortOrder = 10;
        }
        else
        {
            goToFrontDoor.sortOrder = 1;
        }

        return status;
    }

    public Node.Status GoToDoor(GameObject door)
    {
        Node.Status status = GoToLocation(door.transform.position);

        if (status == Node.Status.SUCCESS) 
        {
            if(!door.GetComponent<Lock>().IsLocked)
            {
                door.GetComponent<NavMeshObstacle>().enabled = false;
                //door.SetActive(false);
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
}
