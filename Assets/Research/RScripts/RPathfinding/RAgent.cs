using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RAgent : MonoBehaviour {

    [SerializeField] private Transform _t;
    [SerializeField] private Rigidbody _rb;

    private List<Vector3> _path;
    private int _pathIndex;
    private Vector3 dir; 


    public void Update() {

        if (_path != null) {

            if ((_t.position - _path[_pathIndex]).sqrMagnitude < 1) {
                _pathIndex++;
                dir = _path[_pathIndex] - _t.position;
                dir.y = 0;
                dir = dir.normalized;
            }
            
            _rb.MovePosition(_path[_pathIndex]);

        }

    }


    public void FollowPath(List<Vector3> path) {

        _pathIndex = 0;
        _path = path;

        dir = _path[_pathIndex] - _t.position;
        dir.y = 0;
        dir = dir.normalized;

    }








}
