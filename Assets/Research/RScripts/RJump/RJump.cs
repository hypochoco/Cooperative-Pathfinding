using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RJump : MonoBehaviour {

    [SerializeField] private Transform _t;
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private bool _grounded;
    private float _jumpTime;
    private Vector3 _jumpVelocity;

    private Vector3 _jumpPosition;
    private bool _gate;

    private void Start() {

        _jumpTime = -1f;
        _jumpVelocity = new Vector3(12f, 18f, 0f);
        
    }

    private void Update() {

        if (_jumpTime >= 0)
            _jumpTime -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Space))
            Jump();

    }


    private void FixedUpdate() {

        _grounded = Physics.Raycast(_t.position, Vector3.down, 0.13f);

        if (_jumpTime >= 0)
            _rb.velocity += _jumpVelocity * Time.deltaTime;
        
        if (_jumpTime < 0 && _gate && _grounded) {
            _gate = false;
            Debug.Log((_t.position - _jumpPosition).magnitude);
        }

    }

    private void Jump() {
        _gate = true;
        _jumpPosition = _t.position;
        _jumpTime = 0.125f;
    }

}
