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
    private Material _m;
    private bool _pathing;
    private List<Vector3> _path;

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
    public bool Pathing {
        get { return _pathing; }
        set { _pathing = value; }
    }
    public List<Vector3> Path {
        get { return _path; }
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

        // Set State Variables
        _stateFactory = new RAgentStateFactory(this);
        _state = _stateFactory.Idle();
        _state.EnterState();

    }

    private void Update() {
        _state.UpdateStates();
        _state.CheckSwitchStates();
    }

    private void FixedUpdate() {
        _state.FixedUpdateStates();
    }

    #endregion

    #region RAgent Functions

    // Follow Path
    public void FollowPath(List<Vector3> path) {
        _path = path;
        _pathing = true;
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
