// PathNode Object
    // Goal - Used in Pathfinding to store several properties
    // Known Bugs - N/A
    // Todo - N/A
public class PathNode : IHeapItem<PathNode> {

    // Variables
    private Grid<PathNode> grid;
    public int x;
    public int y;

    public int heapIndex;

    public int gCost;
    public int hCost;
    public int fCost;

    public bool isWalkable;

    public PathNode cameFromNode;

    // Constructor
    public PathNode(Grid<PathNode> _grid, int _x, int _y) {
        grid = _grid;
        x = _x;
        y = _y;
        isWalkable = true;
    }

    // Calculates FCost
    public void CalculateFCost() {
        fCost = gCost + hCost;
    }

    // Testing Purposes
    public override string ToString() {
        return "(" + x + "," + y + ") " + isWalkable;
    }

    // HeapIndex
    public int HeapIndex {
        get {
            return heapIndex;
        }
        set {
            heapIndex = value;
        }
    }

    // Compare function
    public int CompareTo(PathNode nodeToCompare) {
        int compare = fCost.CompareTo(nodeToCompare.fCost);
        if (compare == 0) {
            compare = hCost.CompareTo(nodeToCompare.hCost);
        }
        return -compare;
    }
}