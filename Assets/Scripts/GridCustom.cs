using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCustom<TGridObject>
{
    public event EventHandler<OnGridObjectChangedEventArgs> OnGridObjectChanged;
    public class OnGridObjectChangedEventArgs : EventArgs {
        public int x;
        public int y;
    }

    private int width;
    private int height;
    private float cellSize;
    private TGridObject[,] gridArray;
    private GameObject playField;
    private Vector3 originPosition;

    public GridCustom(int width, int height, float cellSize, GameObject playField, Vector3 originPos, Func<TGridObject> createdGrid,
    Func<GridCustom<TGridObject>,int,int,TGridObject> createGridObject = null)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.playField = playField;
        this.originPosition = originPos;

        gridArray = new TGridObject[width, height];

        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                if (createGridObject == null)
                {
                    gridArray[x,y] = createdGrid();
                }
                else
                {
                gridArray[x,y] = createGridObject(this, x, y);
                }
            }
        }

        TextMesh[,] debugTextArray = new TextMesh[width,height];

        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                debugTextArray[x, y] = CreateWorldText(x + " " + y, 
                playField.transform, GetWorldPos(x, y) + new Vector3(cellSize, cellSize) * .5f, 20,
                 Color.white, TextAnchor.MiddleCenter);
                 
                Debug.DrawLine(GetWorldPos(x, y), GetWorldPos(x, y + 1), Color.white, 100f);
                Debug.DrawLine(GetWorldPos(x, y), GetWorldPos(x + 1, y), Color.white, 100f);
                
            }
        }
        Debug.DrawLine(GetWorldPos(0, height), GetWorldPos(width, height), Color.white, 100f);
        Debug.DrawLine(GetWorldPos(width, 0), GetWorldPos(width, height), Color.white, 100f);

        //SetValue(2, 1, 56);
    }

    public void TriggerGridObjectChanged(int x, int y) {
        if (OnGridObjectChanged != null)
        { 
            OnGridObjectChanged(this, new OnGridObjectChangedEventArgs { x = x, y = y });
        }
    }

    public Vector3 GetWorldPos(int x, int y)
    {
        return new Vector3(x, y) * cellSize + originPosition;
    }

    public void GetXY(Vector3 worldPos, out int x, out int y)
    {
        x = Mathf.FloorToInt((worldPos - originPosition).x / cellSize);
        y = Mathf.FloorToInt((worldPos - originPosition).y / cellSize);
    }

    public static TextMesh CreateWorldText (string text, Transform parent, Vector3 localPos, int fontSize, 
    Color color, TextAnchor textAnchor)
    {
        GameObject gameObject = new GameObject("World_Text", typeof(TextMesh));
        Transform transform = gameObject.transform;
        transform.SetParent(parent, false);
        transform.localPosition = localPos;
        TextMesh textMesh = gameObject.GetComponent<TextMesh>();
        textMesh.anchor = textAnchor;
        textMesh.text = text;
        textMesh.fontSize = fontSize;
        textMesh.color = color;
        textMesh.transform.localScale = new Vector3(.1f,.1f,0f);

        return textMesh;
    }

    public void SetValue(int x, int y, TGridObject value)
    {
        //Debug.Log(x + " " + y);
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            gridArray[x, y] = value;
            //debugTextArray[x,y].text = gridArray[x,y].ToString();
            //debugTextArray[x,y].text = x + " " + y;
        }
    }

    public void SetValue(Vector3 worldPos, TGridObject value)
    {
        int x, y;
        GetXY(worldPos, out x, out y);
        SetValue(x, y, value);
    }

    public TGridObject GetValue(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            return gridArray[x, y];
        }else
        {
            return default(TGridObject);
        }
    }

    public TGridObject GetValue(Vector3 worldPos)
    {
        int x, y;
        GetXY(worldPos, out x, out y);
        return GetValue(x, y);
    }

    public int GetWidth() { return width; }
    public int GetHeight() { return height; }

    public float GetCellSize() { return cellSize; }
}
