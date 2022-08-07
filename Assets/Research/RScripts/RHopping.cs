using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RHopping : MonoBehaviour { 

    // TestUnit Variables
    [SerializeField] private Transform _t;
    [SerializeField] private Rigidbody _rb;

    // Target Variables
    [SerializeField] private Transform _targetT;

    // Timing
    [SerializeField] private float _waitTime;


    // Hop toward Target Object
    private void Start() {

        StartCoroutine(HopTarget());

    }

    // Hop toward the target object
    IEnumerator HopTarget() {

        // While true...
        while (true) {

            // Look at the correct direction
            Vector3 dir = _targetT.position - _t.position;
            Quaternion rot = Quaternion.LookRotation(dir, Vector3.up);
            rot.x = 0;
            rot.z = 0;
            _t.rotation = rot; 

            // Hop toward target
            Vector3 ndir = dir.normalized;
            ndir.y = 1;
            _rb.AddForce(ndir * 100);

            // Wait for hop to end
            yield return new WaitForSeconds(_waitTime);

        }

    }

}
