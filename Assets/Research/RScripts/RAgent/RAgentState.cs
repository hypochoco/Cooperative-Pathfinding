using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region State Skeleton

// public class RAgentStateIdle : RAgentState {

//     #region State Variables

//     #endregion

//     #region Constructor

//     // Constructor
//     public RAgentStateIdle(RAgent _stateMachine, 
//         RAgentStateFactory _stateFactory) : 
//         base (_stateMachine, _stateFactory) {}
    
//     #endregion

//     #region State Functions

//     public override void EnterState() {}
//     public override void UpdateState() {}
//     public override void CheckSwitchState() {}
//     public override void FixedUpdateState() {}
//     public override void InitializeSubState() {}
//     public override void ExitState() {}

//     #endregion
    
// }

#endregion

public abstract class RAgentState {
    
    #region State Variables

    // State Variables
    private bool _rootState;
    private RAgent _stateMachine;
    private RAgentStateFactory _stateFactory;
    private RAgentState _currentSubState;
    private RAgentState _currentSuperState;

    // Getters and Setters
    public bool RootState {
        get { return _rootState; }
        set { _rootState = value; }
    }
    public RAgent Ctx {
        get { return _stateMachine; }
        private set {}
    }
    public RAgentStateFactory Factory {
        get { return _stateFactory; }
        private set {}
    }
    public RAgentState CurrentSubState {
        get { return _currentSubState; }
        private set {}
    }
    public RAgentState CurrentSuperState {
        get { return _currentSuperState; }
        private set {}
    }
    
    #endregion

    #region Constructor

    // Constructor
    public RAgentState(RAgent stateMachine, RAgentStateFactory stateFactory) {
        _stateMachine = stateMachine;
        _stateFactory = stateFactory;
    }

    #endregion

    #region State Functions

    // Basic State Functions
    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void CheckSwitchState();
    public abstract void FixedUpdateState();
    public abstract void InitializeSubState();
    public abstract void ExitState();

    #endregion

    #region Hierarchical State Functions

    // UpdateStates
    public void UpdateStates() {
        UpdateState();
        if (_currentSubState != null)
            _currentSubState.UpdateStates();
    }

    // CheckSwitchStates
    public void CheckSwitchStates() {
        CheckSwitchState();
        if (_currentSubState != null)
            _currentSubState.CheckSwitchStates();
    }

    // FixedUpdateStates
    public void FixedUpdateStates() {
        FixedUpdateState();
        if (_currentSubState != null) 
            _currentSubState.FixedUpdateStates();
    }

    // Exit States
    public void ExistStates() {
        ExitState();
        if (_currentSubState != null)
            _currentSubState.ExistStates();
    }

    // Switch State
    public void SwitchState(RAgentState newState) {
        ExistStates();
        newState.EnterState();
        if (_rootState) {
            _stateMachine.State = newState;
        } else if (_currentSuperState != null) {
            _currentSuperState.SetSubState(newState);
        }
    }

    // Set SuperState
    public void SetSuperState(RAgentState newSuperState) {
        _currentSuperState = newSuperState;
    }

    // Set SubState
    public void SetSubState(RAgentState newSubState) {
        _currentSubState = newSubState;
        newSubState.SetSuperState(this);
    }

    #endregion

}
