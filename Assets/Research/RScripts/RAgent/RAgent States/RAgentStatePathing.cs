using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// RAgentStatePathing
public class RAgentStatePathing : RAgentState {

    #region State Variables

    // State Variables
    private float _turnDst;
    private float _turnSpeed;
    private float _movemntSpeed;
    private RPath _path;

    #endregion

    #region Constructor

    // Constructor
    public RAgentStatePathing(RAgent _stateMachine, 
        RAgentStateFactory _stateFactory) : 
        base (_stateMachine, _stateFactory) {
        
        RootState = true;
    }
    
    #endregion

    #region State Functions

    public override void EnterState() {

        // Change Color
        Ctx.Material.color = Color.red;

        // Set Initial Variables
        _turnDst = 2f;
        _turnSpeed = 200f;
        _movemntSpeed = 100f;

        // Create Path
        _path = new RPath(Ctx.Path, Ctx.Transform.position, _turnDst);

        // Testing Purposes
        Ctx.DebugPath = _path;

        // Start Pathing
        Ctx.StartCoroutine(PathingCoroutine());

    }

    public override void UpdateState() {}
    public override void CheckSwitchState() {}
    public override void FixedUpdateState() {}
    public override void InitializeSubState() {}
    public override void ExitState() {}

    #endregion

    #region Pathing Functions

    private IEnumerator PathingCoroutine() {

        // Set initial variables
        Transform t = Ctx.Transform;
        Rigidbody rb = Ctx.Rigidbody;
        int pathIndex = 0;

        while (true) {

            // Increase pathIndex or stop coroutine
            Vector2 pos2D = new Vector2(t.position.x, t.position.z);
            while (_path.TurnBoundaries[pathIndex].HasCrossedLine(pos2D)) {
                if (pathIndex == _path.FinishLineIndex) {
                    goto end;
                } else {
                    pathIndex++;
                }
            }

            // Look at next point
            Quaternion targetRotation = 
                Quaternion.LookRotation(_path.LookPoints[pathIndex] - t.position);
            targetRotation.x = 0;
            targetRotation.z = 0;
            t.rotation = Quaternion.Lerp(t.rotation, targetRotation, 
                Time.deltaTime * _turnSpeed);

            // Move toward next point
            rb.AddRelativeForce(_movemntSpeed * new Vector3(0, 1, 1));

            // Wait
            yield return new WaitForSeconds(0.45f);

        }

        // End Label
        end:

        // Stop Coroutine
        Ctx.Pathing = false;
        yield break;

    }

    #endregion
    
}