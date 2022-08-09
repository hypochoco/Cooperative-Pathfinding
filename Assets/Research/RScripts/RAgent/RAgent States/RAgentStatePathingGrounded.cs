using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RAgentStatePathingGrounded : RAgentState {

    #region State Variables

    // State Variables
    private RAgentStatePathing _ctx;
    private float _delay;

    #endregion

    #region Constructor

    // Constructor
    public RAgentStatePathingGrounded(RAgent _stateMachine, 
        RAgentStateFactory _stateFactory, RAgentStatePathing PathingCtx) : 
        base (_stateMachine, _stateFactory) {
        
        _ctx = PathingCtx;
    }
    
    #endregion

    #region State Functions

    public override void EnterState() {

        // State Variables
        _delay = 0.125f;

        // Look at initial point
        Quaternion targetRotation = 
            Quaternion.LookRotation(_ctx.Path.LookPoints[_ctx.PathIndex] - 
            Ctx.Transform.position);
        targetRotation.x = 0;
        targetRotation.z = 0;
        Ctx.Transform.rotation = targetRotation;

    }
    public override void UpdateState() {

        // Delay
        if (_delay >= 0) {
            _delay -= Time.deltaTime;
            return;
        }

        // Look at next point
        Quaternion targetRotation = 
            Quaternion.LookRotation(_ctx.Path.LookPoints[_ctx.PathIndex] - 
            Ctx.Transform.position);
        targetRotation.x = 0;
        targetRotation.z = 0;
        Ctx.Transform.rotation = Quaternion.Lerp(Ctx.Transform.rotation, 
            targetRotation, Time.deltaTime * _ctx.TurnSpeed);

        // Jump
        Ctx.Rigidbody.AddRelativeForce(_ctx.MovementSpeed * new Vector3(0f, 1f, 0.5f));
        SwitchState(Factory.PathingFalling(_ctx));

    }
    public override void CheckSwitchState() {}
    public override void FixedUpdateState() {}
    public override void InitializeSubState() {}
    public override void ExitState() {}

    #endregion

}
