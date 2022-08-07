using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Researching Selection System
public class RSelection : MonoBehaviour {



    private void Update() {

        if (Input.GetMouseButtonDown(0)) {

            RaycastHit[] hits = CastRay();

            foreach (RaycastHit hit in hits) {

                if (hit.collider.gameObject.TryGetComponent<RSelectable>(out RSelectable rSelectable)) {
                    rSelectable.ToggleSelect();
                }


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
