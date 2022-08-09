using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// RAgent
public class RAgent : MonoBehaviour {
    
    #region RAgent Variables

    // RAgent Variables
    

    #region State Variables

    // State Variables
    private RAgentState _state;

    // Getters and Setters
    public RAgentState State {
        get { return _state; }
        set { _state = value; }
    }

    #endregion

    #endregion

    #region Unity Functions

    private void Update() {
        _state.UpdateStates();
        _state.CheckSwitchStates();
    }

    private void FixedUpdate() {
        _state.FixedUpdateStates();
    }

    #endregion

}
