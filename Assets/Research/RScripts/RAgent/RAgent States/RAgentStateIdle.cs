using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RAgentStateIdle : RAgentState {

    #region State Variables

    #endregion

    #region Constructor

    // Constructor
    public RAgentStateIdle(RAgent _stateMachine, 
        RAgentStateFactory _stateFactory) : 
        base (_stateMachine, _stateFactory) {
        
        RootState = true;
    }
    
    #endregion

    #region State Functions

    public override void EnterState() {

        // Check if Goal has been reached
        if (Ctx.Goal != null) {
            RGridNode _n = Ctx.Goal;
            Vector3 goal = Ctx.Grid.
                GetWorld(_n.x, _n.y, _n.z);
            Ctx.GoalReached = 
                (goal - Ctx.Transform.position).sqrMagnitude < 2.5f;
        }

        // Testing Purposes
        Ctx.Material.color = Color.white;

    }

    public override void UpdateState() {}
    public override void CheckSwitchState() {}
    public override void FixedUpdateState() {}
    public override void InitializeSubState() {}
    public override void ExitState() {}

    #endregion
    
}
