using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    [SerializeField] public PathfindingVisuals pathfindingVisual;
    [SerializeField] public GameObject castlePrefab;
    [SerializeField] public WallConfigs[] wallConfigs;

    private bool isInEditMode = false;
    private GridCustom<PathNode> grid;
    private List<PathNode> editableNodes = new List<PathNode>();
    private List<PathNode> wallsToBePlaced = new List<PathNode>();

    public WallConfigs selectedWall;

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

    public void SetSelectedWall(WallConfigs config)
    {
        selectedWall = config;
    }

    public WallConfigs GetAvailableWalls(int? specificWall = null)
    {
        if (specificWall == null)
        {
            WallConfigs config = wallConfigs[Random.Range(0,wallConfigs.Length)];

        return config;
        }else
        {
            WallConfigs config = wallConfigs[(int)specificWall];
            return config;
        }
        
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

        if (isInEditMode)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (selectedWall == null)
                {
                    Debug.Log("No wall selected");
                }else
                {
                    var mousePosition = GetMouseWorldPos();

                    int x, y;
                    grid.GetXY(mousePosition, out x, out y);
                    var node = Pathfinding.Instance.GetNode(x, y);
                    if (node == null)
                    {
                        goto Skip;
                    }
                    if (node.isWalkable == false || !node.canPlaceWall)
                    {
                        // CANNOT PLACE A WALL
                        Debug.Log("Can't place here");
                    }else
                    {
                        // THIS HIDES EDITABLE TILES DUE TO THE DELEGATE
                        //node.SetIsWalkable(false);
                        wallsToBePlaced.Add(node);
    
                        var gameobj = Instantiate(selectedWall.wallPrefab, grid.GetWorldPos(x,y) + new Vector3(.25f, .25f), Quaternion.identity);
                        gameobj.transform.localScale = new Vector3(.5f,.5f);
                    }
                }
            }
        }
        Skip:

        if (Input.GetKeyDown(KeyCode.Escape) && isInEditMode)
        {
            // EXIT EDIT MODE
            isInEditMode = false;
            pathfindingVisual.isInEditMode = isInEditMode;
            HidePlaceableGrid();
            foreach(var node in wallsToBePlaced)
            {
                node.SetIsWalkable(false);
            }
            wallsToBePlaced.Clear();
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

    private Vector3 GetMouseWorldPos()
    {
        var mousePos = Input.mousePosition;
        mousePos.z = Mathf.Abs(Camera.main.transform.position.z);
        
        Vector3 vec = Camera.main.ScreenToWorldPoint(mousePos);
        //Debug.Log(vec);

        return vec;
    }
}
