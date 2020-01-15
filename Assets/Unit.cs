using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    private List<Vector3> pathVectorList;
    private int currentPathIndex;

    public void Init()
    {
        transform.position = Pathfinding.Instance.GetGrid().GetWorldPos(0,0) + new Vector3(.25f,.25f,0);
    }

    private void Update() {
        HandleMovement();
    }

    public void SetTargetPosition (Vector3 targetPos)
    {
        currentPathIndex = 0;
        pathVectorList = Pathfinding.Instance.FindPath(GetPos(), targetPos);

        if (pathVectorList != null && pathVectorList.Count > 1)
        {
            pathVectorList.RemoveAt(0);
        }
    }

    private void HandleMovement()
    {
        Vector3 offset = new Vector3(-2.5f,-4f,0);
        
        if (pathVectorList != null)
        {
            Vector3 targetPos = pathVectorList[currentPathIndex];
            //targetPos = new Vector3(targetPos.x + .5f, targetPos.y + .5f) + offset;
            if (Vector3.Distance(transform.position, targetPos) > .24f)
            {
                Vector3 moveDir = (targetPos - transform.position).normalized;

                float distanceBefore = Vector3.Distance(transform.position, targetPos);
                transform.position = transform.position + moveDir * .75f * Time.deltaTime;
            }else
            {
                currentPathIndex++;
                if (currentPathIndex >= pathVectorList.Count)
                {
                    Debug.Log(transform.position + " " + targetPos);
                    StopMoving();
                }
            }
        }
    }

    public Vector3 GetPos()
    {
        return transform.position;
    }

    private void StopMoving()
    {
        pathVectorList = null;
    }
}
