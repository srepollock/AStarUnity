using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AStarSearch : MonoBehaviour {
	public char[,] _map;
	/// <summary>
	/// All the nodes on the map.
	/// </summary>
	public GameObject pathObject;
	Node[,] _nodes {get; set;}
	public NodeRecord[,] _nodeRecords {get; set;}
	public List<NodeRecord> _path = new List<NodeRecord>();
	public List<NodeRecord> _open = new List<NodeRecord>();
	public List<NodeRecord> _closed = new List<NodeRecord>();

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void buildConnections() {
		Debug.Log("Size: " + _map.GetLength(0) + ", " + _map.GetLength(1));
		for (int col = 0;col<_map.GetLength(0);col++) {
			for(int row = 0;row<_map.GetLength(1);row++) {
				if (!(col-1 <= 0)) {
					this._nodeRecords[col, row].connections.Add(new Connection().setConnection(1, this._nodeRecords[col, row], _nodeRecords[col-1, row]));
				}
				if (!(row-1 <= 0)) {
					this._nodeRecords[col, row].connections.Add(new Connection().setConnection(1, this._nodeRecords[col, row], _nodeRecords[col, row-1]));
				}
				if (!(col+1 >= _map.GetLength(0))) {
					this._nodeRecords[col, row].connections.Add(new Connection().setConnection(1, this._nodeRecords[col, row], _nodeRecords[col+1, row]));
				}
				if (!(row+1 >= _map.GetLength(1))) {
					this._nodeRecords[col, row].connections.Add(new Connection().setConnection(1, this._nodeRecords[col, row], _nodeRecords[col, row+1]));
				}
				Debug.Log(this._nodeRecords[col,row].node.type + " connections: " + this._nodeRecords[col,row].connections.ToString());
			}
		}
	}

	public NodeRecord getStartingNode() {
		// for (int x = 0; x < _map.GetLength(0); x++) {
		// 	for (int y = 0; y < _map.GetLength(1); y++) {
		// 		if (_map[x,y] == 's' || _map[x,y] == 'S') {
		// 			return new Node(x, y, "start");
		// 		}
		// 	}
		// }
		// return null;
		return GameObject.Find("Start").GetComponent<NodeRecord>();
	}

	public NodeRecord getGoalNode() {
		// for (int x = 0; x < _map.GetLength(0); x++) {
		// 	for (int y = 0; y < _map.GetLength(1); y++) {
		// 		if (_map[x,y] == 'g' || _map[x,y] == 'G') {
		// 			return new Node(x, y, "goal");
		// 		}
		// 	}
		// }
		// return null;
		return GameObject.Find("Goal").GetComponent<NodeRecord>();
	}

	public void findPath(NodeRecord[,] nodeRecords) {
		this._nodeRecords = nodeRecords;
		Heuristic heuristic = new Heuristic(getGoalNode().node);
		_open.Add(getStartingNode());
		buildConnections();
		_path = pathLoop(_open, _closed, heuristic);
		if (_path == null) {
			Debug.Log("No path");
		}
	}

	/// <summary>
	/// The main pathfinding loop
	/// </summary>
	/// <param name="open"></param>
	/// <param name="closed"></param>
	/// <param name="heuristic"></param>
	/// <returns>Best path from start to goal</returns>
	/// TODO: fix this whole block. Mostly when adding the node to a list. Look at using parent node to build the path
	/// TODO: Future: fix this whole nonesense with node/noderecord. Confusing
	public List<NodeRecord> pathLoop(List<NodeRecord> open, List<NodeRecord> closed, Heuristic heuristic) {
		NodeRecord current = new NodeRecord();
		List<NodeRecord> path;
		while (open.Count > 0) {
			current = open.OrderBy(node => node.estimatedTotalCost).First();
			if (current.node.type == "goal") {
				break;
			}
			List<Connection> connections = current.getConnections();
			float endNodeHeuristic;
			foreach (Connection c in connections) {
				NodeRecord endNode = c.toNode;
				if (endNode.node.type == "wall") continue;
				NodeRecord endNodeRecord;
				c.cost = 1;
				float endNodeCost = current.costSoFar + c.cost;
				if (closed.Contains(endNode)) {
					endNodeRecord = closed.Find(_endNode => _endNode.node == endNode.node);
					if (endNodeRecord.costSoFar <= endNodeCost) {
						continue;
					}
					closed.Remove(endNodeRecord);
					endNodeHeuristic = endNodeRecord.cost - endNodeRecord.costSoFar;
				} else if (open.Contains(endNode)) {
					endNodeRecord = open.Find(_endNode => _endNode.node == endNode.node);
					if (endNodeRecord.costSoFar <= endNodeCost) {
						continue;
					}
					endNodeHeuristic = endNodeRecord.cost - endNodeRecord.costSoFar;
				} else {
					endNodeRecord = new NodeRecord();
					endNodeRecord.node = endNode.node;
					endNodeHeuristic = heuristic.estimate(1, endNode.node);
				}
				// NodeRecord endNodeRecord = new NodeRecord();
				endNodeRecord.cost = endNodeCost;
				endNodeRecord.parentNode = endNode; // TODO: Parent node?
				endNodeRecord.estimatedTotalCost = endNodeCost + endNodeHeuristic;
				if (!open.Contains(endNode)) {
					open.Add(endNode);
				}
			}
			open.Remove(current);
			closed.Add(current);
		}
		if (current.node.type != "goal") {
			return null; // TODO: Handle this
		} else {
			path = new List<NodeRecord>();
			while(current.node.type != "start") {
				path.Add(current);
				GameObject.Instantiate(pathObject, new Vector3(current.node.x, 0, current.node.y), new Quaternion());
				if (current.connections == null) break;
				current = current.connections.First().toNode; // Here is where it breaks (infinite)
			}
			path.Reverse();
		}
		Debug.Log("Path: " + path.ToString());
		return path;
	}
}