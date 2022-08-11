using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Findings :
    // Unity far more consistent with addrelativeforce
    // rather than adding to velocity

public class RJump : MonoBehaviour {

    [SerializeField] private Transform _t;
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private bool _grounded;

    private bool _jumpRequested;
    private float _jumpDelay;
    private Vector3 _jumpVelocity;
    private Vector3 _jumpPosition;


    private void Start() {

        _jumpVelocity = 0.125f * new Vector3(12f, 18f, 0f);
        
    }

    private void Update() {

        if (Input.GetKeyDown(KeyCode.Space) && _grounded && _jumpDelay <= 0)
            _jumpRequested = true;

        if (_jumpDelay > 0)
            _jumpDelay -= Time.deltaTime;

    }


    private void FixedUpdate() {

        _grounded = Physics.Raycast(_t.position, Vector3.down, 0.13f);

        if (_jumpRequested && _grounded)
            Jump();

    }

    private void Jump() {

        _jumpDelay = 0.125f;
        _jumpRequested = false;
        _jumpPosition = _t.position;
        _rb.AddRelativeForce(_jumpVelocity, ForceMode.Impulse);

        StartCoroutine(DistanceCheck());

    }

    IEnumerator DistanceCheck() {

        yield return new WaitForSeconds(0.125f);

        while (!_grounded)
            yield return null;

        Debug.Log((_t.position - _jumpPosition).magnitude);

        yield break;

    }

}
