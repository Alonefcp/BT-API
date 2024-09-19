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
        Leaf canSee = new Leaf("Can see cop?", CanSeeCoop);
        Leaf flee = new Leaf("Flee from cop", FleeFromCop);

        Invert invertMoney = new Invert("Invert Money");
        invertMoney.AddChild(hasGotMoney);

        Invert cantSeCop = new Invert("Cant see cop");
        cantSeCop.AddChild(canSee);

        openDoor.AddChild(goToFrontDoor);
        openDoor.AddChild(goToBackDoor);

        runAway.AddChild(canSee);
        runAway.AddChild(flee);

        //Sequence s1 = new Sequence("s1");
        //s1.AddChild(invertMoney);

        //Sequence s2 = new Sequence("s2");
        //s2.AddChild(cantSeCop);
        //s2.AddChild(openDoor);

        //Sequence s3 = new Sequence("s3");
        //s3.AddChild(cantSeCop);
        //s3.AddChild(selectObject);

        //Sequence s4 = new Sequence("s4");
        //s4.AddChild(cantSeCop);
        //s4.AddChild(goToVan);

        //steal.AddChild(s1);
        //steal.AddChild(s2);
        //steal.AddChild(s3);
        //steal.AddChild(s4);

        BehaviourTree seeCop = new BehaviourTree();
        //Sequence conditions = new Sequence("Steal conditions");
        //conditions.AddChild(invertMoney);
        //conditions.AddChild(cantSeCop);
        seeCop.AddChild(cantSeCop);
        DepSequence steal = new DepSequence("Steal", seeCop, agent);

        steal.AddChild(invertMoney);
        steal.AddChild(openDoor);
        steal.AddChild(selectObject);
        steal.AddChild(goToVan);
    
        Selector beThief = new Selector("Be a thief");
        beThief.AddChild(steal);
        beThief.AddChild(runAway);

        behaviourTree.AddChild(beThief);

        behaviourTree.Print();         

        StartCoroutine(ReduceMoney());
    }

    public Node.Status CanSeeCoop()
    {
        return CanSee(cop.transform.position, "Cop", 10.0f, 90.0f);
    }

    public Node.Status FleeFromCop()
    {
        return Flee(cop.transform.position,10.0f);
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

    public Node.Status GoToArt2()
    {
        if (!art[1].activeSelf) //we have sold the art
        {
            return Node.Status.FAILURE;
        }

        Node.Status status = GoToLocation(art[1].transform.position);

        if (status == Node.Status.SUCCESS)
        {
            art[1].transform.parent = gameObject.transform;
            pickUp = art[1];
        }

        return status;
    }

    public Node.Status GoToArt3()
    {
        if (!art[2].activeSelf) //we have sold the art
        {
            return Node.Status.FAILURE;
        }

        Node.Status status = GoToLocation(art[2].transform.position);

        if (status == Node.Status.SUCCESS)
        {
            art[2].transform.parent = gameObject.transform;
            pickUp = art[2];
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

    private IEnumerator ReduceMoney()
    {
        while (true)
        {
            money = Mathf.Clamp(money - 20, 0, 1000);

            yield return new WaitForSeconds(Random.Range(1.0f, 5.0f));
        }
    }
}
