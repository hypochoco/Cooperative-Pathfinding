using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// RInputHandler
public class RInputHandler : MonoBehaviour {

    #region RInputHandler Priorities

    // 0 - Do Nothing
    // 1 - Move
    // 2 - RSelectable

    #endregion

    #region RInputHandler Variables

    // Reference Variables
    private Camera _mainCam; 

    // RPathHandler... 

    // Input Handler Variables
    private bool _actionCompleted;
    private Queue<RAction> _queue;

    #endregion

    #region Unity Functions

    private void Start() {

        // Set Reference Variables
        _mainCam = Camera.main;

        // Set Input Handler Variables
        _actionCompleted = true;
        _queue = new Queue<RAction>();
    }

    private void Update() {

        // Queue Actions
        EnqueueAction();

        // Dequeue Actions
        DequeueAction();

    }

    #endregion

    #region RInputHandler Functions

    // Finds all objects clicked
    private RaycastHit[] CastRay() {
        Ray ray = _mainCam.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits = Physics.RaycastAll(ray.origin, 
        ray.direction, 2000f);
        return hits;
    }

    // Processes and Queues Actions
    private void EnqueueAction() {

        // Enqueue only on click
        if (!Input.GetMouseButtonDown(0))
            return;

        // Set Initial Actions
        float actionPriority = 0;
        RAction action = null;

        // Cast a ray
        RaycastHit[] hits = CastRay();

        // Loop through all hit objects
        foreach (RaycastHit hit in hits) {

            GameObject hitObj = hit.collider.gameObject;

            if (hitObj.TryGetComponent<RSelectable>(out var rSelectable)) {
                if (actionPriority < 2) {
                    actionPriority = 2;
                    action = new RToggleSelectableAction(ref _actionCompleted, rSelectable);
                }
            }

            // if (hit.collider.gameObject.TryGetComponent<RGround>(out var ground)) {
            //     _rPathHandler.RequestPath(hit.point);
            // }

        }

        // Enqueue Action
        if (action != null)
            _queue.Enqueue(action);

    }

    // Dequeue and Run Actions
    private void DequeueAction() {

        // Ensure actions exist in queue
        if (_queue.Count <= 0)
            return;

        // Ensure the previous action has been completed
        if (!_actionCompleted)
            return;

        // Dequeue and start action
        RAction action = _queue.Dequeue();
        action.StartAction();

    }

    #endregion

}
