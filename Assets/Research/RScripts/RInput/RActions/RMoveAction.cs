using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// RMoveAction
public class RMoveAction : RAction {

    #region Action Variables

    // Action Variables
    private bool _actionCompleted;
    private RPathRequestHandler _rPathRequestHandler;

    #endregion

    #region Constructor

    // Constructor
    public RMoveAction(ref bool actionCompleted, RPathRequestHandler rPathRequestHandler) {
        _actionCompleted = actionCompleted;
        _rPathRequestHandler = rPathRequestHandler;
    }

    #endregion

    #region Action Functions
    
    // Start Action
    public void StartAction() {
        _rPathRequestHandler.Test();
        _actionCompleted = true;
    }

    // Update Action
    public void UpdateAction() {}

    #endregion

}
