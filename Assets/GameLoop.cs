using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLoop : MonoBehaviour
{
    [SerializeField] public TestingPath testingPath;
    [SerializeField] public Unit unit;
    [SerializeField] public GameHandler handler;
    void Start()
    {
        testingPath.Init();
        handler.Init();
        unit.Init();
    }
}
