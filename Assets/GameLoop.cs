using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLoop : MonoBehaviour
{
    [SerializeField] public TestingPath testingPath;
    [SerializeField] public Unit unit;
    void Start()
    {
        testingPath.Init();
        unit.Init();
    }
}
