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
    public RAgentState Pathing(List<Vector3> path) {
        return new RAgentStatePathing(_stateMachine, this, path);
    }

    // Pathing Grounded Sub State
    public RAgentState PathingGrounded() {
        return new RAgentStatePathingGrounded(_stateMachine, this);
    }

    // Pathing Jump Sub State
    public RAgentState PathingJump() {
        return new RAgentStatePathingJump(_stateMachine, this);
    }

    #endregion
}
