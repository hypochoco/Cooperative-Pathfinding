using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    

    [SerializeField] private Rigidbody _rb;
    [SerializeField] private Renderer _r;




    private bool _forwardPressed;
    private bool _backwardPressed;
    private bool _leftPressed;
    private bool _rightPressed;

    private float _movementSpeed;

    private void Start() {



        _movementSpeed = 5;




    }


    private void Update() {

        float vertInput = Input.GetAxisRaw("Vertical");
        float horizInput = Input.GetAxisRaw("Horizontal");

        _forwardPressed = vertInput > 0;
        _backwardPressed = vertInput < 0;
        _leftPressed = horizInput > 0;
        _rightPressed = horizInput < 0;




        if (Input.GetKeyDown("space")) {
            _r.material.color = Color.red;
        }




    }


    private void FixedUpdate() {
        
        if (_forwardPressed) {
            _rb.AddForce(_movementSpeed * new Vector3(1, 1, 1));
        }


    }

}