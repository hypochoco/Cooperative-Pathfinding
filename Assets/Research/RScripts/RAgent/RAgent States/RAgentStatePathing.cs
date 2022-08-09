using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// RAgentStatePathing
public class RAgentStatePathing : RAgentState {

    #region State Variables

    // State Variables
    private float _turnDst;
    private float _turnSpeed;
    private float _movementSpeed;
    private float _gravMultiplier;
    private RPath _path;
    private int _pathIndex;

    // Getters and Setters
    public float TurnDst {
        get { return _turnDst; }
        private set {}
    }
    public float TurnSpeed {
        get { return _turnSpeed; }
        private set {}
    }
    public float MovementSpeed {
        get { return _movementSpeed; }
        private set {}
    }
    public float GravMultiplier {
        get { return _gravMultiplier; }
        private set {}
    }
    public RPath Path {
        get { return _path; }
        private set {}
    }
    public int PathIndex {
        get { return _pathIndex; } 
        private set {}
    }

    #endregion

    #region Constructor

    // Constructor
    public RAgentStatePathing(RAgent _stateMachine, 
        RAgentStateFactory _stateFactory, List<Vector3> path) : 
        base (_stateMachine, _stateFactory) {
        
        RootState = true;
        _path = new RPath(path, Ctx.Transform.position, _turnDst);

    }
    
    #endregion

    #region State Functions

    public override void EnterState() {

        // Testing Purposes
        Ctx.DebugPath = _path;
        Ctx.Material.color = Color.red;

        // Substate
        InitializeSubState();

        // Set Initial Variables
        _turnDst = 2f;
        _turnSpeed = 50f;
        _movementSpeed = 100f;
        _gravMultiplier = 1f;
        _pathIndex = 0;

    }
    public override void UpdateState() {

        // Increase pathIndex
        Vector2 pos2D = 
            new Vector2(Ctx.Transform.position.x, Ctx.Transform.position.z);
        while (_path.TurnBoundaries[_pathIndex].HasCrossedLine(pos2D)) {
            if (_pathIndex == _path.FinishLineIndex) {
                SwitchState(Factory.Idle());
                return;
            } else {
                _pathIndex++;
            }
        }

    }
    public override void CheckSwitchState() {}
    public override void FixedUpdateState() {}
    public override void InitializeSubState() {

        // Gounded Substate
        SetSubState(Factory.PathingGrounded(this));
        CurrentSubState.EnterState();

    }

    public override void ExitState() {

        // Testing Purposes
        Ctx.DebugPath = null;

    }

    #endregion
    
}