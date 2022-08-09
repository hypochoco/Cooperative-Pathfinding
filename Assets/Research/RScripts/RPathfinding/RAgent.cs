using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RAgent : MonoBehaviour {

    #region RAgent Variables

    [SerializeField] private Transform _t;
    [SerializeField] private Rigidbody _rb;

    private Vector3 _goal;
    private bool _goalReached;

    private List<Vector3> _path;
    private bool _followingPath;
    private Coroutine _pathing;

    // Getters and Setters
    public List<Vector3> Path {
        get { return _path; }
        set { _path = value; }
    }

    public Vector3 Goal {
        get { return _goal; }
        set { _goal = value; }
    }
    public bool GoalReached {
        get { return _goalReached; }
        private set {}
    }

    #endregion

    #region RAgent Functions

    private void Update() {

        if (!_goalReached) {
            if ((_t.position - _goal).sqrMagnitude < 0.25f) {
                _goalReached = true;
            }
        }

    }

    // Follow Path
    public void FollowPath(int timeStep) {

        // Ensure path exists
        if (_path == null)
            return;

        _path = _path.GetRange(0, timeStep);

        // Start pathing coroutine
        if (_pathing != null)
            StopCoroutine(_pathing);
        _pathing = StartCoroutine(FollowPathCoroutine());

    }

    IEnumerator FollowPathCoroutine() {
        
        // Set followingPath
        _followingPath = true;

        foreach (Vector3 target in _path) {

            while (true) {

                // Look at the correct direction
                Vector3 dir = target - _t.position;
                Quaternion rot = Quaternion.LookRotation(dir, Vector3.up);
                rot.x = 0;
                rot.z = 0;
                _t.rotation = rot; 

                // Hop toward target
                Vector3 ndir = dir.normalized;
                ndir.y = 0.5f;
                _rb.AddForce(ndir * 100);

                // Wait for hop to end
                yield return new WaitForSeconds(0.45f);

                if ((_t.position - target).sqrMagnitude < 0.25f)
                    break;
                
            }

        }

        // Set followingPath
        _followingPath = false;

        // Clear path
        _path = null;

        // Stop IEnumerator
        yield break;

    }

    #endregion

    #region Debug

    // Testing Purposes
    public void OnDrawGizmos() {

        // Ensure path exists
        if (_path == null)
            return;
            
        // Draw path in red cubes
        Gizmos.color = Color.red;
        foreach (var point in _path) {
            Gizmos.DrawCube(point, 0.125f * Vector3.one);
        }

    }

    #endregion

}
