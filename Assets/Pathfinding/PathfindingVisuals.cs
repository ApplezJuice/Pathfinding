﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingVisuals : MonoBehaviour
{
    private GridCustom<PathNode> grid;
    private Mesh mesh;
    private static Quaternion[] cachedQuaternionEulerArr;
    private List<PathNode> editableNodes;
    public bool updateMesh = false;
    public bool isInEditMode = false;

    private void Awake() 
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
    }
    private void LateUpdate() 
    {
        if(updateMesh)
        {
            updateMesh = false;
            UpdateVisualTemp();
        }    
    }

    public void SetEditableNodes(List<PathNode> nodes)
    {
        editableNodes = nodes;
    }

    public void SetGrid(GridCustom<PathNode> grid)
    {
        this.grid = grid;
        UpdateVisualTemp();

        grid.OnGridObjectChanged += Grid_OnGridValueChanged;
    }

    private void Grid_OnGridValueChanged(object sender, GridCustom<PathNode>.OnGridObjectChangedEventArgs e) {
        updateMesh = true;
    }

    public void ShowEditables()
    {
        Vector3 quadSize = new Vector3(1, 1) * grid.GetCellSize();

        CreateEmptyMeshArraysTemp(grid.GetWidth() * grid.GetHeight(), out Vector3[] verticies, out Vector2[] uvs, out int[] triangles);

        foreach(var node in editableNodes)
        {
            int index = node.x * grid.GetHeight() + node.y;
            Vector2 gridValueUV = new Vector2(10f, 0f);
            if (isInEditMode)
            {
                quadSize = new Vector3(1, 1) * grid.GetCellSize();
                AddToMeshArrays(verticies, uvs, triangles, index, grid.GetWorldPos(node.x, node.y) + quadSize * .5f, 0f, quadSize, gridValueUV, gridValueUV);
                mesh.vertices = verticies;
                mesh.uv = uvs;
                mesh.triangles = triangles;
            }else
            {
                UpdateVisualTemp();
                Debug.Log(isInEditMode);
                break;
            }
        }
    }

    public void UpdateVisualTemp()
    {
        CreateEmptyMeshArraysTemp(grid.GetWidth() * grid.GetHeight(), out Vector3[] verticies, out Vector2[] uvs, out int[] triangles);

        for (int x = 0; x < grid.GetWidth(); x++)
        {
            for (int y = 0; y < grid.GetHeight(); y++)
            {
                int index = x * grid.GetHeight() + y;
                Vector3 quadSize = new Vector3(1, 1) * grid.GetCellSize();

                PathNode pathNode = grid.GetValue(x, y);

                if (pathNode.isWalkable)
                {
                    quadSize = Vector3.zero;
                }
                AddToMeshArrays(verticies, uvs, triangles, index, grid.GetWorldPos(x, y) + quadSize * .5f, 0f, quadSize, Vector2.zero, Vector2.zero);
            }
        }
        mesh.vertices = verticies;
        mesh.uv = uvs;
        mesh.triangles = triangles;
    }

    public static void CreateEmptyMeshArraysTemp(int quadCount, out Vector3[] verticies, out Vector2[] uvs, out int[] triangles)
    {
        verticies = new Vector3[4 * quadCount];
        uvs = new Vector2[4 * quadCount];
        triangles = new int[6 * quadCount];
    }

    public static void AddToMeshArrays(Vector3[] vertices, Vector2[] uvs, int[] triangles, int index, Vector3 pos, float rot, Vector3 baseSize, Vector2 uv00,
    Vector2 uv11)
    {
        int vIndex = index * 4;
        int vIndex0 = vIndex;
        int vIndex1 = vIndex+1;
        int vIndex2 = vIndex+2;
        int vIndex3 = vIndex+3;

        baseSize *= .5f;

        bool skewed = baseSize.x != baseSize.y;
        if (skewed) {
			vertices[vIndex0] = pos+GetQuaternionEuler(rot)*new Vector3(-baseSize.x,  baseSize.y);
			vertices[vIndex1] = pos+GetQuaternionEuler(rot)*new Vector3(-baseSize.x, -baseSize.y);
			vertices[vIndex2] = pos+GetQuaternionEuler(rot)*new Vector3( baseSize.x, -baseSize.y);
			vertices[vIndex3] = pos+GetQuaternionEuler(rot)*baseSize;
		} else {
			vertices[vIndex0] = pos+GetQuaternionEuler(rot-270)*baseSize;
			vertices[vIndex1] = pos+GetQuaternionEuler(rot-180)*baseSize;
			vertices[vIndex2] = pos+GetQuaternionEuler(rot- 90)*baseSize;
			vertices[vIndex3] = pos+GetQuaternionEuler(rot-  0)*baseSize;
		}
		
		//Relocate UVs
		uvs[vIndex0] = new Vector2(uv00.x, uv11.y);
		uvs[vIndex1] = new Vector2(uv00.x, uv00.y);
		uvs[vIndex2] = new Vector2(uv11.x, uv00.y);
		uvs[vIndex3] = new Vector2(uv11.x, uv11.y);
		
		//Create triangles
		int tIndex = index*6;
		
		triangles[tIndex+0] = vIndex0;
		triangles[tIndex+1] = vIndex3;
		triangles[tIndex+2] = vIndex1;
		
		triangles[tIndex+3] = vIndex1;
		triangles[tIndex+4] = vIndex3;
		triangles[tIndex+5] = vIndex2;
        
    }

    private static void CacheQuaternionEuler() {
        if (cachedQuaternionEulerArr != null) return;
        cachedQuaternionEulerArr = new Quaternion[360];
        for (int i=0; i<360; i++) {
            cachedQuaternionEulerArr[i] = Quaternion.Euler(0,0,i);
        }
    }
    
    private static Quaternion GetQuaternionEuler(float rotFloat) {
        int rot = Mathf.RoundToInt(rotFloat);
        rot = rot % 360;
        if (rot < 0) rot += 360;
        //if (rot >= 360) rot -= 360;
        if (cachedQuaternionEulerArr == null) CacheQuaternionEuler();
        return cachedQuaternionEulerArr[rot];
    }
}
