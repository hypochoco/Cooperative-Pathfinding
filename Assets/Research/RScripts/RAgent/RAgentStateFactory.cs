using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// RAgentStateFactory
public class RAgentStateFactory {

    #region RAgentStateFactory Variables

    // RAgentStateFactory Variables
    private RAgent _stateMachine;

    #endregion
    
    #region Constructor

    // Constructor
    public RAgentStateFactory(RAgent stateMachine) {
        _stateMachine = stateMachine;
    }

    #endregion

    #region State Constructors

    // Idle State
    public RAgentState Idle() {
        return new RAgentStateIdle(_stateMachine, this);
    }

    // Pathing State
    public RAgentState Pathing() {
        return new RAgentStatePathing(_stateMachine, this);
    }

    #endregion
}
