using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingPath : MonoBehaviour
{
    [SerializeField] public PathfindingVisuals pathfindingVisual;
    private Pathfinding pathfinding;
    void Start()
    {
        pathfinding = new Pathfinding(10,20, this.gameObject);
        pathfindingVisual.SetGrid(pathfinding.GetGrid());
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            pathfinding.GetGrid().GetXY(GetMouseWorldPos(), out int x, out int y);
            pathfinding.GetNode(x, y).SetIsWalkable(!pathfinding.GetNode(x,y).isWalkable);
        }

        if (Input.GetMouseButtonDown(0))
        {
            

            Vector3 mouseWorldPosition = GetMouseWorldPos();
            pathfinding.GetGrid().GetXY(mouseWorldPosition, out int x, out int y);
            List<PathNode> path = pathfinding.FindPath(0, 0, x, y);

            Vector3 offset = new Vector3(-2.5f,-4f,0);

            if (path != null)
            {
                for (int i = 0; i < path.Count - 1; i++)
                {
                    //Debug.Log(path[i].x + " " + path[i].y);
                    Debug.DrawLine(new Vector3((float)path[i].x / 2 + .25f, (float)path[i].y / 2 + .25f) + offset, 
                    new Vector3((float)path[i+1].x / 2 + .25f,(float) path[i+1].y / 2 + .25f) + offset, 
                    Color.green, 4f);
                }
            }
        }
    }

    private Vector3 GetMouseWorldPos()
    {
        var mousePos = Input.mousePosition;
        mousePos.z = Mathf.Abs(Camera.main.transform.position.z);
        
        Vector3 vec = Camera.main.ScreenToWorldPoint(mousePos);
        //Debug.Log(vec);

        return vec;
    }
}
