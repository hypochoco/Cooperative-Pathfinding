using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RGizmos : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnDrawGizmos()
    {
        // Draw a semitransparent blue cube at the transforms position
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawCube(transform.position, new Vector3(1, 1, 1));
    }
}
