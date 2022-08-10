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
    private float _distToGround;
    private Material _m;
    private bool _pathing;

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
    public bool Pathing {
        get { return _state is RAgentStatePathing; }
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

    #endregion

    #endregion

    #region Unity Functions

    private void Start() {

        // Set Reference Variables
        _m = _r.material;

        // Set RAgent Variables
        _distToGround = 0.13f;

        // Set State Variables
        _stateFactory = new RAgentStateFactory(this);
        _state = _stateFactory.Idle();
        _state.EnterState();

    }

    private void Update() {

        // State Functions
        _state.UpdateStates();
        _state.CheckSwitchStates();

    }

    private void FixedUpdate() {

        // State Functions
        _state.FixedUpdateStates();

        // Ground check
        _grounded = (Physics.Raycast(_t.position,
            Vector3.down, _distToGround) || 
            _rb.velocity.y == 0f);

    }

    #endregion

    #region RAgent Functions

    // Follow Path
    public void FollowPath(List<Vector3> path) {
        _state.SwitchState(_state.Factory.Pathing(path));
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
