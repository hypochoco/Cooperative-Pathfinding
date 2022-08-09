using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RAgent : MonoBehaviour {

    [SerializeField] private Transform _t;
    [SerializeField] private Rigidbody _rb;

    private List<Vector3> _path;

    private bool hopping;


    public void FollowPath(List<Vector3> path) {

        StartCoroutine(HopTargetList(path));

    }

    // Hop toward the target object
    IEnumerator HopTarget(Vector3 target) {

        hopping = true;

        // While true...
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

            if ((_t.position - target).sqrMagnitude < 1f)
                break;

        }

        hopping = false;

        yield break;

    }

    IEnumerator HopTargetList(List<Vector3> path) {

        _path = path;

        foreach (Vector3 target in path) {

            StartCoroutine(HopTarget(target));

            while (hopping)
                yield return null;

        }

        yield break;

    }

    public void OnDrawGizmos() {

        if (_path == null)
            return;
            
        Gizmos.color = Color.red;

        foreach (var point in _path) {
            
            Gizmos.DrawCube(point, 0.125f * Vector3.one);

        }

    }

}
