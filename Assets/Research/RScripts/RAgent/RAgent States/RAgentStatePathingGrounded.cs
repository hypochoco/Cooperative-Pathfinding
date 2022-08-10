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
        _delay = _ctx.Delay;

        // Direction
        Vector3 dir = _ctx.Path.LookPoints[_ctx.PathIndex] - 
            Ctx.Transform.position;

        // Look at initial point
        Quaternion targetRotation = 
            Quaternion.LookRotation(new Vector3(dir.x, 0, dir.z));
        Ctx.Transform.rotation = Quaternion.Lerp(Ctx.Transform.rotation, 
            targetRotation, 100);

    }
    public override void UpdateState() {

        // Delay
        if (_delay >= 0) {
            _delay -= Time.deltaTime;
            return;
        }

        // Direction
        Vector3 dir = _ctx.Path.LookPoints[_ctx.PathIndex] - 
            Ctx.Transform.position;

        // Look at next point
        Quaternion targetRotation = 
            Quaternion.LookRotation(new Vector3(dir.x, 0, dir.z));
        Ctx.Transform.rotation = Quaternion.Lerp(Ctx.Transform.rotation, 
            targetRotation, Time.deltaTime * _ctx.TurnSpeed);

        // Adujust jump height
        float jumpMultiplier = 1f;
        if (dir.y > 0.1f)
            jumpMultiplier = 2f;

        // Adjust jump distance
        float distanceMultiplier = 1f;
        // if ((new Vector3(dir.x, 0 , dir.z)).sqrMagnitude < 0.25f)
        //     distanceMultiplier = 0.5f + (new Vector3(dir.x, 0 , dir.z)).sqrMagnitude;

        // Jump
        Ctx.Rigidbody.AddRelativeForce(_ctx.MovementSpeed * 
            new Vector3(0f, jumpMultiplier, distanceMultiplier));
        SwitchState(Factory.PathingFalling(_ctx));

    }
    public override void CheckSwitchState() {}
    public override void FixedUpdateState() {}
    public override void InitializeSubState() {}
    public override void ExitState() {}

    #endregion

}
