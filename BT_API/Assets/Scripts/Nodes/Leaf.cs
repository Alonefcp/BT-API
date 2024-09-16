
public class Leaf : Node
{
    public delegate Status Tick();
    public Tick ProcessMethod;

    public delegate Status TickM(int index);
    public TickM ProcessMethodM;

    public int index;

    public Leaf() { }

    public Leaf(string nodeName, Tick processMethod): base(nodeName) 
    {
        ProcessMethod = processMethod;
    }

    public Leaf(string nodeName, Tick processMethod, int order): base(nodeName)
    {
        ProcessMethod = processMethod;
        sortOrder = order;
    }

    public Leaf(string nodeName, TickM processMethod, int ind) : base(nodeName)
    {
        ProcessMethodM = processMethod;
        index = ind;
    }

    public override Status Process()
    {
        if(ProcessMethod != null)
        {
             return ProcessMethod();
        }
        else if (ProcessMethodM != null)
        {
            return ProcessMethodM(index);
        }
        else 
        {
            return Status.FAILURE;
        }
    }
}
