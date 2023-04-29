using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstarNode
{
    public bool blocked;
    public Vector2 worldPos;
    public Vector2Int gridPos;

    public uint gCost; //Gcost is the distance from the starting node
    public uint hCost; //Hcost is the distance from the end node
    public uint fCost; //Hcost is the distance from the end node

    public AstarNode parent;

    public AstarNode(Vector2Int gridPos_, Vector2 worldPos_){
        worldPos = worldPos_;
        gridPos = gridPos_;
    }

    public uint GetFCost(){
        fCost = gCost + hCost;
        return fCost;
    }
}
