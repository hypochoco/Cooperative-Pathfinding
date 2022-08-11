using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RAgentStatePathingFalling : RAgentState {

    #region State Variables

    // State Variables
    private RAgentStatePathing _ctx;

    #endregion

    #region Constructor

    // Constructor
    public RAgentStatePathingFalling(RAgent _stateMachine, 
        RAgentStateFactory _stateFactory, RAgentStatePathing PathingCtx) : 
        base (_stateMachine, _stateFactory) {
        
        _ctx = PathingCtx;
    }
    
    #endregion

    #region State Functions

    public override void EnterState() {}
    public override void UpdateState() {}
    public override void CheckSwitchState() {
        
        // If grounded
        if (Ctx.Grounded) {
            if (_ctx.Pathing) {
                SwitchState(Factory.PathingGrounded(_ctx));
            } else {
                SwitchState(Factory.Idle());
            }            
        }

    }
    public override void FixedUpdateState() {

        // Better Jump        
        Ctx.Rigidbody.AddForce(_ctx.GravMultiplier * Vector3.down);

    }
    public override void InitializeSubState() {}
    public override void ExitState() {}

    #endregion
    
}
