using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// RPath
public class RPath {
    
    #region RPath Variables

    // Variables
    private readonly Vector3[] _lookPoints;
    private readonly RLine[] _turnBoundaries;
    private readonly int _finishLineIndex;

    // Getters and Setters
    public Vector3[] LookPoints {
        get { return _lookPoints; }
        private set {}
    }
    public RLine[] TurnBoundaries {
        get { return _turnBoundaries; }
        private set {}
    }
    public int FinishLineIndex {
        get { return _finishLineIndex; }
        private set {}
    }

    #endregion

    #region Constructor

    // Constructor
    public RPath(List<Vector3> waypoints, Vector3 startPos, float turnDst) {
        _lookPoints = waypoints.ToArray();
        _turnBoundaries = new RLine[_lookPoints.Length];
        _finishLineIndex = _turnBoundaries.Length - 1;

        Vector2 previousPoint = V3ToV2(startPos);

        // Line creation
        for (int i = 0; i < _lookPoints.Length; i++) {
            Vector2 currentPoint = V3ToV2(_lookPoints[i]);
            Vector2 dirToCurrentPoint = 
                (currentPoint - previousPoint).normalized;
            Vector2 turnBoundaryPoint = 
                currentPoint - dirToCurrentPoint * turnDst;
            _turnBoundaries[i] = 
                new RLine(turnBoundaryPoint, 
                previousPoint - dirToCurrentPoint * turnDst);
            previousPoint = turnBoundaryPoint;
        }
    }

    #endregion

    #region RPath Functions

    // Converts a Vector3 into a Vector2
    Vector2 V3ToV2(Vector3 v3) {
        return new Vector2 (v3.x, v3.z);
    }

    #endregion

    #region Debug

    // Testing Purposes
    public void DrawWithGizmos() {
        Gizmos.color = Color.black;
        foreach (Vector3 p in _lookPoints) {
            Gizmos.DrawCube(p, 0.1f * Vector3.one);
        }
        foreach (RLine l in _turnBoundaries) {
            l.DrawWithGizmos(1f);
        }
    }

    #endregion

}
