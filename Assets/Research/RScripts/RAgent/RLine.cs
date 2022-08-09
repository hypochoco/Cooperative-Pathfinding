using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// RLine
public class RLine {
    
    #region RLine Variables

    // Variables
	private float _gradient;
	private float _intercept;
	private Vector2 _pointOnLine_1;
	private Vector2 _pointOnLine_2;
	private bool _approachSide;
	private const float verticalLine_Gradient = 1e5f;

    #endregion

    #region Constructor

	// Line Constructor
	public RLine(Vector2 pointOnLine, Vector2 pointPerpendicularToLine) {
		float dx = pointOnLine.x - pointPerpendicularToLine.x;
		float dy = pointOnLine.y - pointPerpendicularToLine.y;

        _gradient = (dy == 0)? verticalLine_Gradient : - dx / dy;

		_intercept = pointOnLine.y - _gradient * pointOnLine.x;
		_pointOnLine_1 = pointOnLine;
		_pointOnLine_2 = pointOnLine + new Vector2 (1, _gradient);

		_approachSide = false;
		_approachSide = GetSide(pointPerpendicularToLine);
	}

    #endregion

    #region RLine Functions

	// Finds the side a point p is on of the Line
	bool GetSide(Vector2 p) {
		return (p.x - _pointOnLine_1.x) * 
            (_pointOnLine_2.y - _pointOnLine_1.y) > 
            (p.y - _pointOnLine_1.y) * 
            (_pointOnLine_2.x - _pointOnLine_1.x);
	}

	// Returns whether a point p has crossed the Line
	public bool HasCrossedLine(Vector2 p) {
		return GetSide(p) != _approachSide;
	}

    #endregion

    #region Debug

	// Testing purposes
    public void OnDrawGizmos() {
		DrawWithGizmos(2f);
	}
	public void DrawWithGizmos(float length) {
		Gizmos.color = Color.black;
		Vector3 lineDir = new Vector3(1, 0, _gradient).normalized;
		Vector3 lineCenter = 
        new Vector3(_pointOnLine_1.x, 0, _pointOnLine_1.y) + 0.1f * Vector3.up;
		Gizmos.DrawLine(lineCenter - lineDir * length / 2f, 
            lineCenter + lineDir * length / 2f);
	}

    #endregion

}
