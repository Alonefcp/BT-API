using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cop : BTAgent
{
    [SerializeField]
    private GameObject[] patrolPoints;

    [SerializeField]
    private GameObject robber;

    protected override void Start()
    {
        base.Start();

        Sequence selectPatrolPoint = new Sequence("Select patrol point");

        for (int i = 0; i < patrolPoints.Length; i++)
        {
            Leaf pp = new Leaf("Go to art" +  patrolPoints[i].name, GoToPoint, i);
            selectPatrolPoint.AddChild(pp);
        }

        Sequence chaseRobber = new Sequence("Chase");
        Leaf canSee = new Leaf("Can see robber?", CanSeeRobber);
        Leaf chase = new Leaf("Chase robber", ChaseRobber);

        chaseRobber.AddChild(canSee);
        chaseRobber.AddChild(chase);

        Invert cantSeeRobber = new Invert("Cant see robber");
        cantSeeRobber.AddChild(canSee);

        BehaviourTree patrolConditions = new BehaviourTree();
        Sequence condition = new Sequence("Patrol conditions");
        condition.AddChild(cantSeeRobber);
        patrolConditions.AddChild(condition);
        DepSequence patrol = new DepSequence("Patrol until", patrolConditions, agent);
        patrol.AddChild(selectPatrolPoint);

        Selector beCop = new Selector("beCop");
        beCop.AddChild(patrol);
        beCop.AddChild(chaseRobber);

        behaviourTree.AddChild(beCop);  
    }


    public Node.Status GoToPoint(int i)
    {
        Node.Status status = GoToLocation(patrolPoints[i].transform.position);
        return status; 
    }

    public Node.Status CanSeeRobber()
    {
        return CanSee(robber.transform.position, "Robber", 5.0f, 60.0f);
    }

    public Node.Status ChaseRobber()
    {
        float chaseDistance = 10.0f;

        if (state == ActionState.IDLE) 
        {
            rememberedLocation = transform.position - (transform.position - robber.transform.position).normalized * chaseDistance;
        }
        return GoToLocation(rememberedLocation);
    }
}
