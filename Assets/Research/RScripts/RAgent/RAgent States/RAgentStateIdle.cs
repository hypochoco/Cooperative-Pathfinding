using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RAgentStateIdle : RAgentState {

    #region State Variables

    #endregion

    #region Constructor

    // Constructor
    public RAgentStateIdle(RAgent _stateMachine, 
        RAgentStateFactory _stateFactory) : 
        base (_stateMachine, _stateFactory) {
        
        RootState = true;
    }
    
    #endregion

    #region State Functions

    public override void EnterState() {

        // Testing Purposes
        Ctx.Material.color = Color.white;

    }

    public override void UpdateState() {}
    public override void CheckSwitchState() {}
    public override void FixedUpdateState() {}
    public override void InitializeSubState() {}
    public override void ExitState() {}

    #endregion
    
}
