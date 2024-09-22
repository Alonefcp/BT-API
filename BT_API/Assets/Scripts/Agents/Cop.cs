using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cop : BTAgent
{
    [SerializeField]
    private GameObject[] patrolPoints;


    protected override void Start()
    {
        base.Start();

        Sequence selectPatrolPoint = new Sequence("Select patrol point");

        for (int i = 0; i < patrolPoints.Length; i++)
        {
            Leaf pp = new Leaf("Go to art" +  patrolPoints[i].name, GoToPoint, i);
            selectPatrolPoint.AddChild(pp);
        }

        behaviourTree.AddChild(selectPatrolPoint);  
    }


    public Node.Status GoToPoint(int i)
    {
        Node.Status status = GoToLocation(patrolPoints[i].transform.position);
        return status;
    }

}
