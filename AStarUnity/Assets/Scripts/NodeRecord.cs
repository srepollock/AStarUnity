using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A record for each node in a list of all nodes visited.
/// </summary>
public class NodeRecord : MonoBehaviour {
	public Node node;
	public float cost {get; set;}
	public float costSoFar;
	public float estimatedTotalCost;
	public float category; //0:open, 1:visit, 2:closed
	public List<Connection> connections = new List<Connection>();
	public NodeRecord parentNode {get; set;}
	public void setStartNode(Node _startNode, Heuristic _heuristic) {
		this.node = _startNode;
		this.costSoFar = 0;
		this.estimatedTotalCost = _heuristic.estimate(1, this.node);
	}
	public List<Connection> getConnections() {
		return this.connections;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
