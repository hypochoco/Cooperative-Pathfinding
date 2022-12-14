using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// RAgentStatePathing
public class RAgentStatePathing : RAgentState {

    #region State Variables

    // State Variables
    private float _turnDst;
    private float _delay;
    private float _gravMultiplier;
    private List<Vector3> _vectorPath;
    private RPath _smoothPath;
    private int _pathIndex;
    private bool _pathing;
    private Coroutine _startPathing;

    // Getters and Setters
    public float Delay {
        get { return _delay; }
        private set {}
    }
    public float GravMultiplier {
        get { return _gravMultiplier; }
        private set {}
    }
    public List<Vector3> VectorPath {
        get { return _vectorPath; }
        private set {}
    }
    public RPath Path {
        get { return _smoothPath; }
        private set {}
    }
    public int PathIndex {
        get { return _pathIndex; } 
        private set {}
    }
    public bool Pathing {
        get { return _pathing; }
        private set {}
    }

    #endregion

    #region Constructor

    // Constructor
    public RAgentStatePathing(RAgent _stateMachine, 
        RAgentStateFactory _stateFactory, List<Vector3> path) : 
        base (_stateMachine, _stateFactory) {
        
        RootState = true;
        _vectorPath = path;

    }
    
    #endregion

    #region State Functions

    public override void EnterState() {

        // Set Initial Variables
        _turnDst = 0.125f;
        _delay = 0.125f;
        _gravMultiplier = 5f;
        _pathIndex = 0;
        _pathing = true;

        // Create Path
        _smoothPath = new RPath(_vectorPath, Ctx.Transform.position, _turnDst);

        // Start Pathing
        _startPathing = 
            Ctx.StartCoroutine(StartPathingCoroutine());

        // Testing Purposes
        Ctx.DebugPath = _smoothPath;
        Ctx.Material.color = Color.red;

    }
    public override void UpdateState() {

        // Increase pathIndex
        Vector2 pos2D = 
            new Vector2(Ctx.Transform.position.x, Ctx.Transform.position.z);
        while (_smoothPath.TurnBoundaries[_pathIndex].HasCrossedLine(pos2D)) {
            if (_pathIndex == _smoothPath.FinishLineIndex) {
                _pathing = false;
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

        // Ensure StartPathing has stopped
        Ctx.StopCoroutine(_startPathing);

    }

    #endregion

    #region Pathing Functions

    private IEnumerator StartPathingCoroutine() {

        // Ensure grounded before pathing
        while (!Ctx.Grounded)
            yield return null;
        
        // Delay
        yield return new WaitForSeconds(_delay);

        // Start Pathing
        InitializeSubState();

    }

    #endregion
    
}