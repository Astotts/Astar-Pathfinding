using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridRenderer : MonoBehaviour
{
    public AstarGrid aStarGrid;

    private List<AstarNode> path;
    private List<AstarNode> obstacles;

    [SerializeField] private Vector2Int worldSize;
    [SerializeField] private float tileSize = 1;

    void Start(){
        aStarGrid = new AstarGrid(worldSize.x, worldSize.y, tileSize);
        obstacles = new List<AstarNode>();
        path = new List<AstarNode>();
    }

    void Update(){
        //Debug.Log(Camera.main.ScreenToWorldPoint(Input.mousePosition)); //FYI Only seems to work with an orthographic camera
        if(Input.GetMouseButtonDown(0)){
            SetDestination();
            if(aStarGrid.source != null){
                Debug.Log("test");
                path = aStarGrid.GetPath();
            }
        }
        if(Input.GetMouseButtonDown(1)){
            SetSource();
            if(aStarGrid.destination != null){
                Debug.Log("test");
                path = aStarGrid.GetPath();
            }
        }
        if(Input.GetMouseButton(2)){
            SetObstacle();
        }
        if(Input.GetMouseButtonUp(2)){
            if(aStarGrid.destination != null && aStarGrid.source != null){
                path = aStarGrid.GetPath();
            }
        }
    }

    public void SetDestination(){
        aStarGrid.destination = GetNodeFromPos(Camera.main.ScreenToWorldPoint(Input.mousePosition)); //NOTE: if you dont have the main camera set in the project settings you will have to serialze a camera to use here
    }

    public void SetSource(){
        aStarGrid.source = GetNodeFromPos(Camera.main.ScreenToWorldPoint(Input.mousePosition)); //NOTE: if you dont have the main camera set in the project settings you will have to serialze a camera to use here
    }

    public void SetObstacle(){
        AstarNode blockedNode = GetNodeFromPos(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        blockedNode.blocked = true;
        obstacles.Add(blockedNode); //NOTE: if you dont have the main camera set in the project settings you will have to serialze a camera to use here
    }

    private AstarNode GetNodeFromPos(Vector2 pos){
        AstarNode node;
        //Debug.Log(pos.x + " " + pos.y + " " + aStarGrid.maxX + " " + aStarGrid.maxY);

        if(pos.x < aStarGrid.maxX && pos.y < aStarGrid.maxY && (int)pos.x > -1 && (int)pos.y > -1){
            int x,y;
            x = (int)Mathf.Round(pos.x / (float)aStarGrid.nodeSize * 2);
            y = (int)Mathf.Round(pos.y / (float)aStarGrid.nodeSize * 2);
            node = aStarGrid.grid[x,y];
            return node;
        }
        else{
            Debug.LogError("Node Out of Range or Does Not Exist");
            return null;
        }

        
    }

    void OnDrawGizmos(){
        Gizmos.color = Color.white;
        if(aStarGrid == null){
           aStarGrid = new AstarGrid(10, 10, tileSize); 
        }
        for(int x = 0; x < aStarGrid.gridx; x++){
            for(int y = 0; y < aStarGrid.gridy; y++){
                Gizmos.DrawCube(new Vector3(aStarGrid.grid[x,y].worldPos.x, aStarGrid.grid[x,y].worldPos.y, 0f), Vector3.one * aStarGrid.nodeSize / 2);
            }
        }

        if(aStarGrid != null && obstacles != null){
            Gizmos.color = Color.grey;
            foreach(AstarNode node in obstacles){
                Gizmos.DrawCube(new Vector3(node.worldPos.x, node.worldPos.y, 1f), Vector3.one * aStarGrid.nodeSize / 2);
            }
            if(aStarGrid.destination != null && aStarGrid.source != null && path != null && path.Count > 0){
                Gizmos.color = Color.green;
                Gizmos.DrawCube(new Vector3(aStarGrid.destination.worldPos.x, aStarGrid.destination.worldPos.y, 1f), Vector3.one * aStarGrid.nodeSize / 2);
                Gizmos.color = Color.yellow;
                Gizmos.DrawCube(new Vector3(aStarGrid.source.worldPos.x, aStarGrid.source.worldPos.y, 1f), Vector3.one * aStarGrid.nodeSize / 2);
                Gizmos.color = Color.blue;
                foreach(AstarNode node in path){
                    Gizmos.DrawCube(new Vector3(node.worldPos.x, node.worldPos.y, 1f), Vector3.one * aStarGrid.nodeSize / 2);
                }
            }
        }
        Gizmos.color = Color.white;
    }
}
