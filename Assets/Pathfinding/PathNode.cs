using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode
{
    private GridCustom<PathNode> grid;

    public int x;
    public int y;
    public int gCost;
    public int hCost;
    public int fCost;

    public bool isWalkable;
    public bool canPlaceWall;

    // Node we came form to reach this one
    public PathNode cameFromNode;

    public PathNode(GridCustom<PathNode> grid, int x, int y)
    {
        this.grid = grid;
        this.x = x;
        this.y = y;
        isWalkable = true;
        canPlaceWall = false;
    }

    public void SetIsWalkable(bool isWalkable)
    {
        this.isWalkable = isWalkable;
        grid.TriggerGridObjectChanged(x, y);
    }

    public void SetCanPlaceWall(bool canPlaceWall)
    {
        this.canPlaceWall = canPlaceWall;
        grid.TriggerGridObjectChanged(x, y);
    }

    public void CalculateFCost()
    {
        fCost = gCost + hCost;
    }

    public override string ToString()
    {
        return x + "," + y;
    }
}
