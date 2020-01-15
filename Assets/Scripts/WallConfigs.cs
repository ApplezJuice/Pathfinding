using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Create Wall Config")]
public class WallConfigs : ScriptableObject
{
    [Range(1,100)]
    public int wallDef;
    public string wallName;
    public GameObject wallPrefab;
    public Sprite wallSprite;

    public WallSpec InitWall()
    {
        WallSpec wallspec = new WallSpec();
        wallspec.wallDef = wallDef;
        wallspec.wallName = wallName;

        return wallspec;
    }
}
