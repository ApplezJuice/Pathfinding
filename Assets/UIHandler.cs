using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
    [SerializeField] GameHandler gameHandler;
    [SerializeField] GameObject actionbarPanel;
    [SerializeField] Toggle[] wallButtons;

    private List<WallConfigs> usableWalls;
    private Dictionary<Toggle,WallConfigs> loadedWallConfigs = new Dictionary<Toggle, WallConfigs>();
    private WallConfigs selectedConfig;

    public void Init()
    {
        usableWalls = new List<WallConfigs>();
        usableWalls.Add(gameHandler.GetAvailableWalls());
        GenerateWallButtons();
    }

    private void GenerateWallButtons()
    {
        for (int i = 0; i < 4; i++)
        {
            wallButtons[i].gameObject.GetComponentInChildren<Image>().sprite = usableWalls[0].wallSprite;
            loadedWallConfigs.Add(wallButtons[i],usableWalls[0]);
        }
    }

    public void WallButtonPressed(Toggle wall)
    {
        loadedWallConfigs.TryGetValue(wall, out selectedConfig);
        Debug.Log(selectedConfig);
        gameHandler.SetSelectedWall(selectedConfig);
    }
}
