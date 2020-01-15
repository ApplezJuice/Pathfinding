using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
    [SerializeField] public GameHandler gameHandler;
    [SerializeField] public GameObject actionbarPanel;
    [SerializeField] public Toggle[] wallButtons;

    private List<WallConfigs> usableWalls;
    private Dictionary<Toggle,WallConfigs> loadedWallConfigs = new Dictionary<Toggle, WallConfigs>();
    private WallConfigs selectedConfig;

    public void Init()
    {
        usableWalls = new List<WallConfigs>();
        usableWalls.Add(gameHandler.GetAvailableWalls(0));
        usableWalls.Add(gameHandler.GetAvailableWalls(1));
        usableWalls.Add(gameHandler.GetAvailableWalls(2));
        usableWalls.Add(gameHandler.GetAvailableWalls(3));
        GenerateWallButtons();
    }

    private void GenerateWallButtons()
    {
        for (int i = 0; i < 4; i++)
        {
            wallButtons[i].gameObject.GetComponentInChildren<Image>().sprite = usableWalls[i].wallSprite;
            loadedWallConfigs.Add(wallButtons[i],usableWalls[i]);
        }
    }

    public void WallButtonPressed(Toggle wall)
    {
        loadedWallConfigs.TryGetValue(wall, out selectedConfig);
        Debug.Log(selectedConfig);
        gameHandler.SetSelectedWall(selectedConfig);
    }
}
