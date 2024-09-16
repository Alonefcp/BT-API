using System.Collections.Generic;

public class PSelector : Node
{
    private Node[] nodes;

    private bool ordered = false;

    public PSelector(string nodeName):base(nodeName)
    {

    }


    public override Status Process()
    {
        if(!ordered)
        {
            OrderNodes();
            ordered = true;
        }

        Status childStatus = children[currentChild].Process();

        if(childStatus == Status.RUNNING)
        {
            return Status.RUNNING;
        }
        else if(childStatus == Status.SUCCESS)
        {
            //children[currentChild].sortOrder = 1;  
            ordered = false;
            currentChild = 0;
            return Status.SUCCESS;
        }
        //else 
        //{
        //    children[currentChild].sortOrder = 10;
        //}
        

        currentChild++;
        if (currentChild >= children.Count) 
        {
            ordered = false;
            currentChild = 0;
            return Status.FAILURE;
        }


        return Status.RUNNING;  
    }

    private void OrderNodes()
    {
        nodes = children.ToArray();
        Sort(nodes, 0, children.Count - 1);
        children = new List<Node>(nodes);
    }

    //Quick sort, Adapted from: https://exceptionnotfound.net/quick-sort-csharp-the-sorting-algorithm-family-reunion/
    private int Partition(Node[] array, int low,
                                int high)
    {
        Node pivot = array[high];

        int lowIndex = (low - 1);

        //2. Reorder the collection.
        for (int j = low; j < high; j++)
        {
            if (array[j].sortOrder <= pivot.sortOrder)
            {
                lowIndex++;

                Node temp = array[lowIndex];
                array[lowIndex] = array[j];
                array[j] = temp;
            }
        }

        Node temp1 = array[lowIndex + 1];
        array[lowIndex + 1] = array[high];
        array[high] = temp1;

        return lowIndex + 1;
    }

    private void Sort(Node[] array, int low, int high)
    {
        if (low < high)
        {
            int partitionIndex = Partition(array, low, high);
            Sort(array, low, partitionIndex - 1);
            Sort(array, partitionIndex + 1, high);
        }
    }
}
