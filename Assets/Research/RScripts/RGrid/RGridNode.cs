using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// RGridNode
public class RGridNode : RIHeapItem<RGridNode> {
    
    #region Grid Node Variables

    // Grid Node Variables
    private int _x;
    private int _y;
    private int _z;

    private int _gCost;
    private int _hCost;
    private int _fCost;

    private bool _walkable;
    private int _heapIndex;
    private RGridNode _previousNode;

    // Getters and Setters
    public int x {
        get { return _x; }
        set { _x = value; }
    }
    public int y {
        get { return _y; }
        set { _y = value; }
    }
    public int z {
        get { return _z; }
        set { _z = value; }
    }
    public bool Walkable {
        get { return _walkable; }
        set { _walkable = value;}
    }
    public int GCost {
        get { return _gCost; }
        set { _gCost = value; }
    }
    public int HCost {
        get { return _hCost; }
        set { _hCost = value; }
    }
    public int FCost {
        get { return _fCost; }
        set { _fCost = value; }
    }
    public int HeapIndex {
        get { return _heapIndex; }
        set { _heapIndex = value; }
    }
    public RGridNode PreviousNode {
        get { return _previousNode; }
        set { _previousNode = value; }
    }

    #endregion

    #region Constructor

    // Constructor
    public RGridNode(int x, int y, int z) {
        _x = x;
        _y = y;
        _z = z;
        _walkable = true;
    }

    #endregion

    #region GridNode Functions

    // Calculate fCost
    public void CalculateFCost() {
        _fCost = _gCost + _hCost;
    }

    // Compare Function
    public int CompareTo(RGridNode node) {
        int compare = _fCost.CompareTo(node.FCost);
        if (compare == 0)
            compare = _hCost.CompareTo(node.HCost);
        return -compare;
    }

    #endregion

    #region Debug

    // Testing Purposes
    public override string ToString() {
        return "(" + _x + ", " + _y + ", " + _z + ")";
    }

    #endregion

}
