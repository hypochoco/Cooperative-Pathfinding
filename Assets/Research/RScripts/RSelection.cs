using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Researching Selection System
public class RSelection : MonoBehaviour {


    [SerializeField] private RPathHandler _rPathHandler;


    private void Update() {

        if (Input.GetMouseButtonDown(0)) {

            RaycastHit[] hits = CastRay();

            foreach (RaycastHit hit in hits) {

                if (hit.collider.gameObject.TryGetComponent<RSelectable>(out var rSelectable)) {
                    rSelectable.ToggleSelect();
                }

                // if (hit.collider.gameObject.TryGetComponent<RGround>(out var ground)) {
                //     _rPathHandler.RequestPath(hit.point);
                // }

            }
        }
    }


    // Finds all objects clicked
    private RaycastHit[] CastRay() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits = Physics.RaycastAll(ray.origin, 
        ray.direction, 2000f);
        return hits;
    }


    // Finds objects pointer has clicked

    // Calls the Selection function on each seelcted item




}
