using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RAgentStatePathingJump : RAgentState {

    #region State Variables

    #endregion

    #region Constructor

    // Constructor
    public RAgentStatePathingJump(RAgent _stateMachine, 
        RAgentStateFactory _stateFactory) : 
        base (_stateMachine, _stateFactory) {}
    
    #endregion

    #region State Functions

    public override void EnterState() {

        // Testing Purposes
        Ctx.Material.color = Color.red;

    }

    public override void UpdateState() {}
    public override void CheckSwitchState() {
        
        // If grounded
        if (Ctx.Grounded)
            SwitchState(Factory.PathingGrounded());

    }

    public override void FixedUpdateState() {}
    public override void InitializeSubState() {}
    public override void ExitState() {}

    #endregion
    
}
