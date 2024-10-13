using System.Collections.Generic;

public class Node 
{
    public enum Status {SUCCESS, RUNNING, FAILURE }
    public Status status { get; set; }

    public List<Node> children { get; set; } = new List<Node>();
  
    protected int currentChild = 0;

    public string name { get; set; }
    public int sortOrder { get; set; }

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

    public void Reset()
    {
        foreach (Node child in children)
        {
            child.Reset();
        }

        currentChild = 0;
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
