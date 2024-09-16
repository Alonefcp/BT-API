using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Root node 
public class BehaviourTree : Node
{
    public BehaviourTree()
    {
        name = "Tree";
    }

    public BehaviourTree(string treeName) : base(treeName)
    {
    }

    public override Status Process()
    {
        if (children.Count <= 0)
        {
            return Status.SUCCESS;
        }
        else
        {
            return children[currentChild].Process();
        }
    }


    protected struct NodeLevel
    {
        public int level;
        public Node node;
    
    }

    public void Print()
    {
        string treePrintout = "";
        Stack<NodeLevel> nodeStack = new Stack<NodeLevel>();
        Node currentNode = this;
        nodeStack.Push(new NodeLevel { level = 0,node = currentNode});

        while (nodeStack.Count != 0)
        {
            NodeLevel nextNode = nodeStack.Pop();
            treePrintout += new string('-', nextNode.level) + nextNode.node.name + "\n";

            for (int i = nextNode.node.children.Count-1; i >= 0; i--) 
            {
                nodeStack.Push(new NodeLevel { level = nextNode.level+1, node = nextNode.node.children[i] });
            }         
        }

        Debug.Log(treePrintout);

    }
} 
