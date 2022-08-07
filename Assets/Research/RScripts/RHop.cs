using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RHop : MonoBehaviour {
   
    // Hopping 
    [SerializeField] private Rigidbody _rb;

    // Hop Variables
    [SerializeField] private float _movementSpeed;

    // Prevent overlapping hops
    private bool _hopping;
   
    // Hop Input
    private void Update() {
        if(Input.GetKeyDown(KeyCode.Space)) {
            StartCoroutine(Hop());
        }
    }

    // Hop Coroutine
    IEnumerator Hop() {

        // Prevent overlapping hops
        if (_hopping)
            yield break;
        
        // Start hop
        _hopping = true;

        _rb.AddForce(_movementSpeed * new Vector3(1, 1, 0));

        // Stop hop
        _hopping = false;
        
        yield break;
    }
}
