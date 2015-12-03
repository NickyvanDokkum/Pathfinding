﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AStar : MonoBehaviour {
	public Transform target;
	
	Grid grid;
	
	void Awake() {
		grid = GetComponent<Grid>();
		target = GameObject.FindWithTag ("Player").transform;
	}
	public List<Node> Path(Transform seeker) {
		FindPath(seeker.position,target.position);
		return(path);
	}
	public List<Node> path = new List<Node>();
	
	void FindPath(Vector3 startPos, Vector3 endPos) {
		Node startNode = grid.NodeFromWorldPoint(startPos);
		Node targetNode = grid.NodeFromWorldPoint(endPos);
		
		List<Node> openSet = new List<Node>();
		HashSet<Node> closedSet = new HashSet<Node>();
		openSet.Add(startNode);
		
		while (openSet.Count > 0) {
			Node currentNode = openSet[0];
			for (int i = 1; i < openSet.Count; i ++) {
				if (openSet[i].fCost < currentNode.fCost || openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost) {
					currentNode = openSet[i];
				}
			}
			
			openSet.Remove(currentNode);
			closedSet.Add(currentNode);
			
			if (currentNode == targetNode) {
				RetracePath(startNode,targetNode);
				return;
			}
			
			foreach (Node neighbour in grid.GetNeighbours(currentNode)) {
				if (!neighbour.walkable || closedSet.Contains(neighbour)) {
					continue;
				}
				
				int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
				if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour)) {
					neighbour.gCost = newMovementCostToNeighbour;
					neighbour.hCost = GetDistance(neighbour, targetNode);
					neighbour.parent = currentNode;
					
					if (!openSet.Contains(neighbour)) {
						openSet.Add(neighbour);
					}
				}
			}
		}
	}
	
	void RetracePath(Node startNode, Node endNode) {
		path.Clear ();
		path = new List<Node> ();
		Node currentNode = endNode;
		
		while (currentNode != startNode) {
			path.Add(currentNode);
			currentNode = currentNode.parent;
		}
		path.Reverse();
		
	}
	
	int GetDistance(Node nodeA, Node nodeB) {
		int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
		int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);
		if (dstX != 0 && dstY != 0) {
			return(999);
		}
		else {
			if (dstX > dstY){
				return 14 * dstY + 10 * (dstX - dstY);
			}
			else {
				return 14 * dstX + 10 * (dstY - dstX);
			}
		}
	}
}