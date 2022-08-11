using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RAgentStatePathingGrounded : RAgentState {

    #region State Variables

    // State Variables
    private RAgentStatePathing _ctx;
    private Transform _t;
    private Rigidbody _rb;
    private bool _jumped;
    private Vector3 _jumpVelocity;
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
        _t = Ctx.Transform;
        _rb = Ctx.Rigidbody;
        _delay = _ctx.Delay;

        // Direction
        Vector3 dir = _ctx.Path.LookPoints[_ctx.PathIndex] - 
            _t.position;

        // Look at initial point
        Quaternion targetRotation = 
            Quaternion.LookRotation(new Vector3(dir.x, 0, dir.z));
        _t.rotation = targetRotation;


        // Testing Purposes
        if (Ctx.test != null)
            Debug.Log((_t.position - Ctx.test).magnitude);
        Ctx.test = _t.position;

    }
    public override void UpdateState() {

        // Delay
        if (_delay > 0)
            _delay -= Time.deltaTime;

    }
    public override void CheckSwitchState() {
        
        // Switch State
        if (!Ctx.Grounded && _rb.velocity.y <= 0)
            SwitchState(Factory.PathingFalling(_ctx));
            
    }
    public override void FixedUpdateState() {

        // Delay
        if (_delay > 0) return;

        // Jump
        if (!_jumped) {
            _jumped = true;
            _rb.AddRelativeForce(new Vector3(0, 1.25f, 1f), 
                ForceMode.Impulse);
        }
        
    }
    public override void InitializeSubState() {}
    public override void ExitState() {}

    #endregion

}
