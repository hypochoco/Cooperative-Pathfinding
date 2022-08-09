using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RAgentStatePathingGrounded : RAgentState {

    #region State Variables

    #endregion

    #region Constructor

    // Constructor
    public RAgentStatePathingGrounded(RAgent _stateMachine, 
        RAgentStateFactory _stateFactory) : 
        base (_stateMachine, _stateFactory) {}
    
    #endregion

    #region State Functions

    public override void EnterState() {

        // Testing Purposes
        Ctx.Material.color = Color.green;

        // Jump
        Ctx.Rigidbody.AddRelativeForce(100 * new Vector3(0f, 1f, 0.5f));
        SwitchState(Factory.PathingJump());

    }
    public override void UpdateState() {}

    public override void CheckSwitchState() {}

    public override void FixedUpdateState() {}

    public override void InitializeSubState() {}
    public override void ExitState() {}

    #endregion

}
