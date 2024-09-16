using System.Collections;
using System.Collections.Generic;


public class Node 
{
    public enum Status {SUCCESS, RUNNING, FAILURE }
    public Status status;

    public List<Node> children = new List<Node>();
    public int currentChild = 0;

    public string name;
    public int sortOrder;

    

    public Node() { }

    public Node(string nodeName)
    {
        name = nodeName; 
    }

    public Node(string nodeName, int order)
    {
        name = nodeName;
        sortOrder = order;
    }

    public virtual Status Process()
    {
        return children[currentChild].Process();
    }

    public void AddChild(Node child)
    {
       children.Add(child); 
    }
}
