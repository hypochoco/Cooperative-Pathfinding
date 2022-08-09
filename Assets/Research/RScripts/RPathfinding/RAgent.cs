using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RAgent : MonoBehaviour {

    #region RAgent Variables

    [SerializeField] private Transform _t;
    [SerializeField] private Rigidbody _rb;

    private List<Vector3> _path;
    private bool _followingPath;
    private Coroutine _pathing;

    #endregion

    #region RAgent Functions

    // Follow Path
    public void FollowPath(List<Vector3> path) {

        // Set Path
        _path = path;

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
