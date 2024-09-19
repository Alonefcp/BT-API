using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatronBehaviour : BTAgent
{
    [SerializeField]
    private GameObject[] art;
    [SerializeField]
    private GameObject frontDoor;
    [SerializeField]
    private GameObject homeBase;

    [Range(0, 1000)]
    [SerializeField]
    private int boredom = 0;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        RSelector selectObject = new RSelector("Select art to view");

        for (int i = 0; i < art.Length; i++)
        {
            Leaf goToArt = new Leaf("Go To Art " + art[i].name, GoToArt, i);
            selectObject.AddChild(goToArt);
        }

        Leaf goToFrontDoor = new Leaf("Go to front door", GoToFrontDoor);
        Leaf goToHome = new Leaf("Go to home", GoToHome);
        Leaf isBored = new Leaf("Is Bored?", IsBored);

        Sequence viewArt = new Sequence("View art");
        viewArt.AddChild(isBored);
        viewArt.AddChild(goToFrontDoor);

        BehaviourTree whileBored = new BehaviourTree();
        whileBored.AddChild(isBored);

        Loop lookForPaintings = new Loop("Look", whileBored);
        lookForPaintings.AddChild(selectObject);

        viewArt.AddChild(lookForPaintings);
        viewArt.AddChild(goToHome);

        Selector bePatron = new Selector("Be an art patron");
        bePatron.AddChild(viewArt); 

        behaviourTree.AddChild(bePatron);

        StartCoroutine(IncreaseBoredom());
    }

    public Node.Status GoToArt(int index)
    {
        if (!art[index].activeSelf) 
        {
            return Node.Status.FAILURE;
        }

        Node.Status status = GoToLocation(art[index].transform.position);
       
        if(status == Node.Status.SUCCESS)
        {
            boredom = Mathf.Clamp(boredom - 150, 0, 1000);
        }

        return status;
    }

    public Node.Status GoToFrontDoor()
    {
        Node.Status status = GoToDoor(frontDoor);

        return status;
    }

    public Node.Status GoToHome()
    {
        Node.Status status = GoToLocation(homeBase.transform.position);
    

        return status;
    }

    public Node.Status IsBored() 
    {
        int boredomThreshold = 100;
        if(boredom < boredomThreshold)
        {
            return Node.Status.FAILURE;
        }
        else
        {
            return Node.Status.SUCCESS;
        }
    }

    private IEnumerator IncreaseBoredom()
    {
        while (true)
        {
            boredom = Mathf.Clamp(boredom + 20, 0, 1000);

            yield return new WaitForSeconds(Random.Range(1.0f, 5.0f));
        }
    }
}
