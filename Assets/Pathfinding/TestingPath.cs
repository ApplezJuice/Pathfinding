using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingPath : MonoBehaviour
{
    [SerializeField] public PathfindingVisuals pathfindingVisual;
    [SerializeField] public bool debugMode;
    [SerializeField] public Unit unit;
    private Pathfinding pathfinding;

    private int startX, startY = 0;

    public void Init()
    {
        pathfinding = new Pathfinding(debugMode, 10,20, this.gameObject);
        pathfindingVisual.SetGrid(pathfinding.GetGrid());
        SetTowers();
    }

    private void SetTowers()
    {
        // BOTTOM CASTLE
        pathfinding.GetNode(4,0).SetIsWalkable(false);
        pathfinding.GetNode(4,1).SetIsWalkable(false);
        pathfinding.GetNode(5,0).SetIsWalkable(false);
        pathfinding.GetNode(5,1).SetIsWalkable(false);

        // BOTTOM RIGHT TOWER
        pathfinding.GetNode(8,3).SetIsWalkable(false);
        pathfinding.GetNode(8,4).SetIsWalkable(false);
        pathfinding.GetNode(9,3).SetIsWalkable(false);
        pathfinding.GetNode(9,4).SetIsWalkable(false);

        // BOTTOM LEFT TOWER
        pathfinding.GetNode(0,3).SetIsWalkable(false);
        pathfinding.GetNode(0,4).SetIsWalkable(false);
        pathfinding.GetNode(1,3).SetIsWalkable(false);
        pathfinding.GetNode(1,4).SetIsWalkable(false);

        // TOP CASTLE
        pathfinding.GetNode(4,19).SetIsWalkable(false);
        pathfinding.GetNode(4,18).SetIsWalkable(false);
        pathfinding.GetNode(5,19).SetIsWalkable(false);
        pathfinding.GetNode(5,18).SetIsWalkable(false);

        // TOP RIGHT TOWER
        pathfinding.GetNode(8,16).SetIsWalkable(false);
        pathfinding.GetNode(8,15).SetIsWalkable(false);
        pathfinding.GetNode(9,16).SetIsWalkable(false);
        pathfinding.GetNode(9,15).SetIsWalkable(false);

        // TOP LEFT TOWER
        pathfinding.GetNode(0,16).SetIsWalkable(false);
        pathfinding.GetNode(0,15).SetIsWalkable(false);
        pathfinding.GetNode(1,16).SetIsWalkable(false);
        pathfinding.GetNode(1,15).SetIsWalkable(false);

        // NON WALKABLES
        for (int i = 2; i < 8; i++)
        {
            pathfinding.GetNode(i,10).SetIsWalkable(false);
            pathfinding.GetNode(i,9).SetIsWalkable(false);
        }
        
    }

    private void Update()
    {
        // DEBUG CLICKS
        if (Input.GetMouseButtonDown(1))
        {
            pathfinding.GetGrid().GetXY(GetMouseWorldPos(), out int x, out int y);
            startX = x;
            startY = y;

            if (!pathfinding.GetGrid().GetValue(startX, startY).isWalkable)
            {
                // NOT WALKABLE
                var neighborList = pathfinding.GetNeighborList(pathfinding.GetNode(startX,startY));
                var tempBestNode = pathfinding.GetNode(startX,startY);
                foreach(var node in neighborList)
                {
                    if (node.fCost <= tempBestNode.fCost && node.isWalkable){
                        tempBestNode = node;
                    }
                }

                startX = tempBestNode.x;
                startY = tempBestNode.y;
                
            }

            Debug.Log(startX + " " + startY);
            //pathfinding.GetNode(x, y).SetIsWalkable(!pathfinding.GetNode(x,y).isWalkable);
        }

        if (!pathfindingVisual.isInEditMode)
        {
            if (Input.GetMouseButtonDown(0))
            {
                

                Vector3 mouseWorldPosition = GetMouseWorldPos();
                pathfinding.GetGrid().GetXY(mouseWorldPosition, out int x, out int y);
                List<PathNode> path = pathfinding.FindPath(startX, startY, x, y);

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

                unit.SetTargetPosition(mouseWorldPosition);
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
