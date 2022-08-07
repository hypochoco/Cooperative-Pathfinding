using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO : weights for pathfinding

public class RGridNode {

    #region Grid Node Variables

    // Grid Node Variables
    private bool _walkable;
    private Vector3 _position;

    // Getters and Setters
    public bool Walkable {
        get { return _walkable; }
        set { _walkable = value;}
    }
    public Vector3 Position {
        get { return _position; }
        set { _position = value; }
    }

    #endregion

    #region Constructor

    // Constructor
    public RGridNode(bool walkable, Vector3 position) {

        _position = position;
        _walkable = walkable;

    }

    #endregion

}
