

public class RSelector : Node
{
    private bool shuffled = false;

    public RSelector(string nodeName):base(nodeName)
    {

    }

    public override Status Process()
    {      
        if(!shuffled)
        {
            children.Shuffle();
            shuffled = true;
        }

        Status childStatus = children[currentChild].Process();

        if(childStatus == Status.RUNNING)
        {
            return Status.RUNNING;
        }
        else if(childStatus == Status.SUCCESS)
        {
            shuffled = false;
            currentChild = 0;
            return Status.SUCCESS;
        }
              
        currentChild++;
        if (currentChild >= children.Count) 
        {
            shuffled = false;
            currentChild = 0;
            return Status.FAILURE;
        }


        return Status.RUNNING;  
    }  
}
