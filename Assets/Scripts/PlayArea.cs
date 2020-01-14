using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayArea : MonoBehaviour
{
    private GridCustom<int> grid;
    private int screenWidth;
    private int screenHeight;
    void Start()
    {
        screenHeight = Screen.height;
        screenWidth = Screen.width;
        //transform.position = Camera.main.ScreenToWorldPoint(new Vector3(0,0,10));
        
        // grid = new GridCustom(10, 20, .5f, this.transform.gameObject, Camera.main.ScreenToWorldPoint(
        //     new Vector3(
        //         screenWidth/2 - 385,
        //         200,10
        //         )));

        grid = new GridCustom<int>(10, 20, .5f, this.transform.gameObject, new Vector3(-2.5f,-4f,0), () => new int());
        //Debug.Log(Screen.height);
    }

    private void Update() 
    {
        if (Input.GetMouseButtonDown(0))
        {
            grid.SetValue(GetMouseWorldPos(), 56);
            //GetMouseWorldPos();
        }

        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log(grid.GetValue(GetMouseWorldPos()));
            //GetMouseWorldPos();
        }
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
