using System;
using UnityEngine;

// Handles Generic Grid Class
    // Goal - Create and interact with a grid
    //      - Used for pathfinding
    // Known Bugs - Moving the start location to something 
    //              other than Vector3.zero makes PathFinding 
    //              break. They are coded to work 
    //              but it doesn’t work.
    // TODO - N/A

public class Grid<TGridObject> {

    // Variables
    private int width;
    private int height;
    private float cellSize;
    private Vector3 originPosition;
    private TGridObject[,] gridArray;
    

    // Testing Purpose
    private TextMesh[,] debugTextArray;
    private bool debug = false;


    // Grid Constructor 
    public Grid(int _width, int _height, float _cellSize, 
        Vector3 _originPosition, 
        Func<Grid<TGridObject>, int, int, TGridObject> createGridObject) {

        width = _width;
        height = _height;
        cellSize = _cellSize;
        originPosition = _originPosition;

        gridArray = new TGridObject[_width, _height];

        for(int x = 0; x < gridArray.GetLength(0); x++) {
            for(int y = 0; y < gridArray.GetLength(1); y++) {
                gridArray[x, y] = createGridObject(this, x, y);
            }
        }

        // Testing Purposes
        if (debug) {
            debugTextArray = new TextMesh[width, height];

            // Debug.Log(width + " " + height);
            for(int i = 0; i < gridArray.GetLength(0); i++) {
                for(int j = 0; j < gridArray.GetLength(1); j++) {
                    debugTextArray[i, j] = CreateWorldText(
                    gridArray[i,j]?.ToString(), null, 
                    GetWorldPosition(i,j) + new Vector3(cellSize, 
                    cellSize) * 0.5f, 20, Color.white, 
                    TextAnchor.MiddleCenter);

                    Debug.DrawLine(GetWorldPosition(i, j), 
                    GetWorldPosition(i, j + 1), Color.white, 100000f);
                    Debug.DrawLine(GetWorldPosition(i, j), 
                    GetWorldPosition(i + 1, j), Color.white, 100000f);
                }
            }
            Debug.DrawLine(GetWorldPosition(0, height), 
            GetWorldPosition(width, height), Color.white, 100f);
            Debug.DrawLine(GetWorldPosition(width, 0), 
            GetWorldPosition(width, height), Color.white, 100f);
        }   
    }

    // Given grid coordinates, returns the world position of the
    // grid object
    private Vector3 GetWorldPosition(int x, int y) {
        return new Vector3(x, y) * cellSize + originPosition; 
    }

    // Given a world position, gives the grid coordinates in the
    // outX, outY, arguments
    public void GetXY(Vector3 worldPosition, out int outX, out int outY) {
        outX = Mathf.FloorToInt((worldPosition - originPosition).x / 
        cellSize);
        outY = Mathf.FloorToInt((worldPosition - originPosition).y / 
        cellSize);
    }

    // Sets the value in a grid object at the given grid coordinates
    public void SetValue(int x, int y, TGridObject value) {

        if (x >= 0 && y >= 0 && x < width && y < height) {
            // Add conditions here for valid values, ignore non-valid
            gridArray[x, y] = value;

            // Testing Purposes
            if (debug) {
                debugTextArray[x ,y].text = gridArray[x, y]?.ToString();
            }
        }
    }

    // Returns the grid object at the given grid coordinates
    public TGridObject GetGridObject(int x, int y) {
        if (x >= 0 && y >= 0 && x < width && y < height) {
            return gridArray[x, y];
        } else {
            return default(TGridObject);
        }
    }

    // Returns the width of the grid
    public int GetWidth() {
        return width;
    }

    // Returns the height of the grid
    public int GetHeight() {
        return height;
    }

    public float GetCellSize() {
        return cellSize;
    }


    // Testing Purposes
    public const int sortingOrderDefault = 5000;

    public void SetValue(Vector3 worldPosition, TGridObject value) {
        int x, y;
        GetXY(worldPosition, out x, out y);
        SetValue(x, y, value);
    }

    public TGridObject GetGridObject(Vector3 worldPosition) {
        int x, y;
        GetXY(worldPosition, out x, out y);
        return GetGridObject(x, y);
    }

    public static TextMesh CreateWorldText(string text, 
        Transform parent = null, Vector3 localPosition = default(Vector3), 
        int fontSize = 40, Color? color = null, 
        TextAnchor textAnchor = TextAnchor.UpperLeft, 
        TextAlignment textAlignment = TextAlignment.Left, 
        int sortingOrder = sortingOrderDefault) {

        if (color == null) color = Color.white;
        return CreateWorldText(parent, text, localPosition, fontSize, 
        (Color)color, textAnchor, textAlignment, sortingOrder);
    }

    public static TextMesh CreateWorldText(Transform parent, 
        string text, Vector3 localPosition, int fontSize, 
        Color color, TextAnchor textAnchor, 
        TextAlignment textAlignment, int sortingOrder) {

        GameObject gameObject = new GameObject("World_Text", typeof(TextMesh));
        Transform transform = gameObject.transform;
        transform.SetParent(parent, false);
        transform.localPosition = localPosition;
        TextMesh textMesh = gameObject.GetComponent<TextMesh>();
        textMesh.anchor = textAnchor;
        textMesh.alignment = textAlignment;
        textMesh.text = text;
        textMesh.fontSize = fontSize;
        textMesh.color = color;
        textMesh.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;
        return textMesh;
    }
}