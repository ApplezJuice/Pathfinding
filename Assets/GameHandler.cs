using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    [SerializeField] public PathfindingVisuals pathfindingVisual;
    [SerializeField] public GameObject castlePrefab;

    private bool isInEditMode = false;
    private GridCustom<PathNode> grid;
    private List<PathNode> editableNodes = new List<PathNode>();

    public void Init()
    {
        grid = Pathfinding.Instance.GetGrid();
        pathfindingVisual.SetGrid(grid);

        for (int x = 0; x < grid.GetWidth(); x++)
        {
            for (int y = 0; y < 8; y++)
            {
                var node = Pathfinding.Instance.GetNode(x,y);
                editableNodes.Add(node);
                node.SetCanPlaceWall(true);
            }
        }

        pathfindingVisual.SetEditableNodes(editableNodes);

        Instantiate(castlePrefab, grid.GetWorldPos(5,1),Quaternion.identity);
    }

    private void Update() 
    {
        // TEST ENTER EDIT MODE
        if (Input.GetKeyDown(KeyCode.F1))
        {
            isInEditMode = !isInEditMode;
            pathfindingVisual.isInEditMode = isInEditMode;
            DrawPlaceableGrid();
        }

        if (Input.GetKeyDown(KeyCode.Escape) && isInEditMode)
        {
            // EXIT EDIT MODE
            isInEditMode = false;
            pathfindingVisual.isInEditMode = isInEditMode;
            HidePlaceableGrid();
        }
    }

    private void DrawPlaceableGrid()
    {
        pathfindingVisual.ShowEditables();
    }

    private void HidePlaceableGrid()
    {
        pathfindingVisual.ShowEditables();
    }
}
