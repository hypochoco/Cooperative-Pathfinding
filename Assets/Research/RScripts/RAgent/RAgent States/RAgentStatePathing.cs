using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// RAgentStatePathing
public class RAgentStatePathing : RAgentState {

    #region State Variables

    // State Variables
    private float _turnDst;
    private float _turnSpeed;
    private float _movemntSpeed;
    private RPath _path;
    private int _pathIndex;

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

        // Substate
        InitializeSubState();

        // Set Initial Variables
        _turnDst = 2f;
        _turnSpeed = 200f;
        _movemntSpeed = 100f;
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

        // Look at next point
        Quaternion targetRotation = 
            Quaternion.LookRotation(_path.LookPoints[_pathIndex] - 
            Ctx.Transform.position);
        targetRotation.x = 0;
        targetRotation.z = 0;
        Ctx.Transform.rotation = Quaternion.Lerp(Ctx.Transform.rotation, 
            targetRotation, Time.deltaTime * _turnSpeed);

    }
    public override void CheckSwitchState() {}
    public override void FixedUpdateState() {}
    public override void InitializeSubState() {

        // Gounded Substate
        SetSubState(Factory.PathingGrounded());
        CurrentSubState.EnterState();

    }

    public override void ExitState() {

        // Testing Purposes
        Ctx.DebugPath = null;

        // Remove Path
        _path = null;

    }

    #endregion
    
}