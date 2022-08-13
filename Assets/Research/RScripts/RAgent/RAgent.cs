using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// RAgent
public class RAgent : MonoBehaviour {
    
    #region RAgent Variables

    // RAgent Variables
    [SerializeField] private Transform _t;
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private Renderer _r;
    [SerializeField] private bool _grounded;
    [SerializeField] private RGridConstructor _gridConstructor;
    private RGrid<RGridNode> _grid;
    private float _distToGround;
    private Material _m;
    private RGridNode _goal;
    private bool _goalReached;

    // Getters and Setters
    public Transform Transform {
        get { return _t; }
        private set {}
    }
    public Rigidbody Rigidbody {
        get { return _rb; }
        private set {}
    }
    public Material Material {
        get { return _m; }
        private set {}
    }
    public bool Grounded {
        get { return _grounded; }
        private set {}
    }
    public RGridNode Goal {
        get { return _goal; }
        set { _goal = value; }
    }
    public bool GoalReached {
        get { return _goalReached; }
        set { _goalReached = value; }
    }
    public RGrid<RGridNode> Grid {
        get { return _grid; }
        private set {}
    }
    public List<RGridNode> Path {
        get {
            if (_state is RAgentStatePathing _pathingState) {
                return _pathingState.GridPath;
            } else {
                return null;
            }
        }
        private set {}
    }
    public int PathIndex {
        get {
            if (_state is RAgentStatePathing _pathingState) {
                return _pathingState.PathIndex;
            } else {
                return -1;
            }
        }
        private set {}
    }

    #region State Variables

    // State Variables
    private RAgentState _state;
    private RAgentStateFactory _stateFactory;

    // Getters and Setters
    public RAgentState State {
        get { return _state; }
        set { _state = value; }
    }
    public bool Pathing {
        get { return _state is RAgentStatePathing; }
        private set {}
    }

    #endregion

    #endregion

    #region Unity Functions

    private void Start() {

        // Set Reference Variables
        _m = _r.material;
        _grid = _gridConstructor.Grid;

        // Set RAgent Variables
        _distToGround = 0.13f;

        // Set State Variables
        _stateFactory = new RAgentStateFactory(this);
        _state = _stateFactory.Idle();
        _state.EnterState();

    }

    private void Update() {

        Debug.Log(_state);

        // State Functions
        _state.UpdateStates();
        _state.CheckSwitchStates();

    }

    private void FixedUpdate() {

        // State Functions
        _state.FixedUpdateStates();

        // Ground check
        _grounded = GroundCheck();

    }

    #endregion

    #region RAgent Functions

    // Follow Path
    public void FollowPath(List<RGridNode> path) {
        _state.SwitchState(_state.Factory.Pathing(path));
    }

    // Groung Check
    public bool GroundCheck() {

        // Current Position
        Vector3 _pos = _t.position;

        // Center of RAgent
        if (Physics.Raycast(_pos, Vector3.down, _distToGround))
            return true;

        // Forward Left
        Vector2 _perp2D = Vector2.
            Perpendicular(new Vector2(_pos.x, _pos.z));
        Vector3 _perp = new Vector3(_perp2D.x, 0, _perp2D.y);
        if (Physics.Raycast(_pos + _perp, Vector3.down, _distToGround))
            return true;

        // Foward Right
        if (Physics.Raycast(_pos - _perp, Vector3.down, _distToGround))
            return true;

        // Backward Left
        if (Physics.Raycast(-_pos + _perp, Vector3.down, _distToGround))
            return true;

        // Backward Right
        if (Physics.Raycast(-_pos - _perp, Vector3.down, _distToGround))
            return true;

        return false;

    }

    #endregion

    #region Debug

    // Testing Purposes
    public RPath DebugPath;
    public void OnDrawGizmos() {
        if (DebugPath != null)
            DebugPath.DrawWithGizmos();
    }

    #endregion

}
