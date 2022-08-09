using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// RToggleSelectable
public class RToggleSelectableAction : RAction {

    #region Action Variables

    // Action Variables
    private bool _actionCompleted;
    private RSelectable _rSelectable;

    #endregion

    #region Constructor

    // Constructor
    public RToggleSelectableAction(ref bool actionCompleted, RSelectable rSelectable) {
        _actionCompleted = actionCompleted;
        _rSelectable = rSelectable;
    }

    #endregion

    #region Action Functions

    // Start Action
    public void StartAction() {
        _rSelectable.ToggleSelect();
        _actionCompleted = true;
    }

    #endregion

}
