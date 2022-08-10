using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// RMoveAction
public class RMoveAction : RAction {

    #region Action Variables

    // Action Variables
    private bool _actionCompleted;
    private RPathHandler _rPathHandler;
    private Vector3 _targetPoint;

    #endregion

    #region Constructor

    // Constructor
    public RMoveAction(ref bool actionCompleted, RPathHandler rPathHandler, Vector3 targetPoint) {
        _actionCompleted = actionCompleted;
        _rPathHandler = rPathHandler;
        _targetPoint = targetPoint;
    }

    #endregion

    #region Action Functions
    
    // Start Action
    public void StartAction() {
        _rPathHandler.Test(_targetPoint);
        _actionCompleted = true;
    }

    // Update Action
    public void UpdateAction() {}

    #endregion

}
