using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RAgentStatePathingFalling : RAgentState {

    #region State Variables

    // State Variables
    private RAgentStatePathing _ctx;
    private float _delay;

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

    public override void EnterState() {

        // State Variables
        _delay = 0.125f;

    }

    public override void UpdateState() {}
    public override void CheckSwitchState() {

        // Better Jump
        Ctx.Rigidbody.velocity += _ctx.GravMultiplier * 
            Time.deltaTime * Vector3.down;

        // Delay
        if (_delay >= 0) {
            _delay -= Time.deltaTime;
            return;
        }
        
        // If grounded
        if (Ctx.Grounded)
            SwitchState(Factory.PathingGrounded(_ctx));                    

    }

    public override void FixedUpdateState() {}
    public override void InitializeSubState() {}
    public override void ExitState() {}

    #endregion
    
}
