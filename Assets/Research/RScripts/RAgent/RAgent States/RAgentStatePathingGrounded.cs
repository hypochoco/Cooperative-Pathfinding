using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RAgentStatePathingGrounded : RAgentState {

    #region State Variables

    // State Variables
    private RAgentStatePathing _ctx;
    private Transform _t;
    private Rigidbody _rb;
    private Vector3 _dir;
    private bool _jumped;
    private Vector3 _jumpVelocity;
    private float _delay;

    // Testing Purposes
    private float _timer;

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
        _dir = _ctx.Path.LookPoints[_ctx.PathIndex] - 
            _t.position;

        // Look at initial point
        Quaternion targetRotation = 
            Quaternion.LookRotation(new Vector3(_dir.x, 0, _dir.z));
        _t.rotation = targetRotation;

        // Jump Calculation
        _jumpVelocity = new Vector3(0, (_dir.y < 0f)? 1.25f : 3f, 1f);

    }
    public override void UpdateState() {

        // Testing Purposes
        _timer += Time.deltaTime;

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
            _rb.AddRelativeForce(_jumpVelocity, 
                ForceMode.Impulse);
        }

    }
    public override void InitializeSubState() {}
    public override void ExitState() {}

    #endregion

}
