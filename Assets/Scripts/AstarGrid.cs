using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstarGrid
{
    public AstarNode[,] grid;
    public int gridx, gridy;
    public float nodeSize;
    public float maxX, maxY;

    public AstarNode source, destination;


    public AstarGrid(int gridx_, int gridy_, float nodeSize_){
        gridx = gridx_;
        gridy = gridy_;
        nodeSize = nodeSize_;
        float halfNodeSize = nodeSize_ / 2f;
        maxX = gridx * nodeSize;
        maxY = gridy * nodeSize;

        grid = new AstarNode[gridx,gridy];
        
        for(int x = 0; x < gridx; x++){
            for(int y = 0; y < gridy; y++){
                grid[x,y] = new AstarNode(new Vector2Int(x, y), new Vector2(x * halfNodeSize, y * halfNodeSize));
            }
        }
    }

    public List<AstarNode> GetPath(){
        Debug.Log(source.gridPos + " " + destination.gridPos);

        List<AstarNode> openList = new List<AstarNode>();
        HashSet<AstarNode> closedList = new HashSet<AstarNode>();

        List<AstarNode> path = new List<AstarNode>();

        openList.Add(source);

        while(openList.Count > 0){ //0 means traversable nodes have been tried
            AstarNode currentNode = openList[0];
            for(int index = 1; index < openList.Count; index++){
                if(openList[index].GetFCost() < currentNode.GetFCost() || openList[index].GetFCost() == currentNode.GetFCost()){
                    if (openList[index].hCost < currentNode.hCost)
						currentNode = openList[index];
                }
            }

            openList.Remove(currentNode);
			closedList.Add(currentNode);

			if (currentNode.gridPos == destination.gridPos) {
				path = RetracePath(source,destination);
				return path;
			}

			foreach (AstarNode neighbor in GetNeighbors(currentNode)) {
                //Debug.Log("Neighbors Found");
				if (neighbor.blocked || closedList.Contains(neighbor)) {
					continue;
				}

				uint cost = currentNode.gCost + GetDirectionCost(currentNode, neighbor);
				if (cost < neighbor.gCost || !openList.Contains(neighbor)) {
					neighbor.gCost = cost;
					neighbor.hCost = GetDirectionCost(neighbor, destination);
					neighbor.parent = currentNode;

					if (!openList.Contains(neighbor))
						openList.Add(neighbor);
				}
			}
		}

        Debug.LogError("No Path Found");
        return null;
	}

    private uint GetDirectionCost(AstarNode neighborNode, AstarNode destinationNode){
        uint x,y;
        x = (uint)Mathf.Abs(neighborNode.gridPos.x - destinationNode.gridPos.x);
        y = (uint)Mathf.Abs(neighborNode.gridPos.y - destinationNode.gridPos.y);

        if(x > y){
            return 14 * y + (10 * (x - y));
        }
        else{
            return 14 * x + (10 * (y - x));
        }
    }

    private List<AstarNode> GetNeighbors(AstarNode currentNode){ // Cardinal and Inter-Cardinal Neighbors
        
        List<AstarNode> neighborList = new List<AstarNode>();
        for (int x = -1; x <= 1; x++) {
			for (int y = -1; y <= 1; y++) {
				if (x == 0 && y == 0)
					continue;

				int posX = currentNode.gridPos.x + x;
				int posY = currentNode.gridPos.y + y;

                //Debug.Log(posX + " " + posY);
				if (posX > -1 && posY > -1 && posX < maxX && posY < maxY && grid[posX,posY] != null) {
                    //Debug.Log(grid[x,y]);
					neighborList.Add(grid[posX,posY]);
				}
			}
		}
        return neighborList;
    }

    /*private List<AstarNode> GetNeighbors(AstarNode currentNode){ // Cardinal Neighbors ONLY
        
        List<AstarNode> neighborList = new List<AstarNode>();
        for (int x = -1; x <= 1; x++) {
			for (int y = -1; y <= 1; y++) {
				if (x == y)
					continue;

				int posX = currentNode.gridPos.x + x;
				int posY = currentNode.gridPos.y + y;

                //Debug.Log(posX + " " + posY);
				if (posX > -1 && posY > -1 && posX < maxX && posY < maxY && grid[posX,posY] != null) {
                    //Debug.Log(grid[x,y]);
					neighborList.Add(grid[posX,posY]);
				}
			}
		}
        return neighborList;
    }*/

    private List<AstarNode> RetracePath(AstarNode start, AstarNode end) {
		List<AstarNode> path = new List<AstarNode>();
		AstarNode currentNode = end;

		while (currentNode != start) {
			path.Add(currentNode);
			currentNode = currentNode.parent;
		}
		path.Reverse();

        return path;
	}
}

    /*
        Open set of nodes to be evaluated
        Closed set of nodes already evaluated

        loop
            current = NODE in Open with the lowest fcost
            remove current from Open
            add current to Closed

            if current is the target node, return

            foreach neighbor of the current node
                if neighbor is not traversable or in the closed list skip to next neighbor

                if new path to neighbor is shorter OR neighbor is not open
                    set fcost of neighbor
                    set parent of neighbor to current
                    if neighbor is not in open
                        add to open
    */
