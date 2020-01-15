using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    [SerializeField] public PathfindingVisuals pathfindingVisual;
    [SerializeField] public GameObject castlePrefab;
    [SerializeField] public WallConfigs[] wallConfigs;
    [SerializeField] public GameObject wallSelectedPrefab;

    private bool isInEditMode = false;
    private GridCustom<PathNode> grid;
    private List<PathNode> editableNodes = new List<PathNode>();
    private List<PathNode> wallsToBePlaced = new List<PathNode>();
    private PathNode mouseHoverNode;
    private GameObject tempNodeObject;

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
            var v3 = Input.mousePosition;
            v3.z = 10;
            var mouseWorldLoc = Camera.main.ScreenToWorldPoint(v3);
            
            grid.GetXY(mouseWorldLoc, out int mouseX, out int mouseY);
            
            var mouseLocationNode = Pathfinding.Instance.GetNode(mouseX, mouseY);
            // OVER A NODE THAT CAN PLACE A WALL
            if (mouseLocationNode != null && mouseLocationNode.canPlaceWall)
            {
                Debug.Log(mouseLocationNode.x + " " + mouseLocationNode.y);
                // NEW NODE NOT BEGINNING
                if (mouseHoverNode != mouseLocationNode && mouseHoverNode != null)
                {
                    mouseHoverNode = mouseLocationNode;
                    var nodeWorldPos = grid.GetWorldPos(mouseHoverNode.x, mouseHoverNode.y);
                    //tempNodeObject = Instantiate(wallSelectedPrefab, nodeWorldPos + new Vector3(.25f,.25f), Quaternion.identity);
                    //tempNodeObject.transform.localScale = new Vector3(.5f,.5f);

                    if (tempNodeObject != null)
                    {
                        //Destroy(tempNodeObject);
                        tempNodeObject.transform.position = nodeWorldPos + new Vector3(.25f,.25f);
                    }else
                    {
                        tempNodeObject = Instantiate(wallSelectedPrefab, nodeWorldPos + new Vector3(.25f,.25f), Quaternion.identity);
                        tempNodeObject.transform.localScale = new Vector3(.5f,.5f);
                    }
                   
                    
                }
                else if (mouseHoverNode != mouseLocationNode && mouseHoverNode == null)
                {
                    // START OF A HOVER
                    mouseHoverNode = mouseLocationNode;
                    var nodeWorldPos = grid.GetWorldPos(mouseHoverNode.x, mouseHoverNode.y);
                    
                    if (tempNodeObject != null)
                    {
                        tempNodeObject = Instantiate(wallSelectedPrefab, nodeWorldPos + new Vector3(.25f,.25f), Quaternion.identity);
                        tempNodeObject.transform.localScale = new Vector3(.5f,.5f);
                    }else
                    {
                        tempNodeObject = Instantiate(wallSelectedPrefab, nodeWorldPos + new Vector3(.25f,.25f), Quaternion.identity);
                        tempNodeObject.transform.localScale = new Vector3(.5f,.5f);
                    }
                    
                    
                }

            }

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
            Destroy(tempNodeObject);
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
