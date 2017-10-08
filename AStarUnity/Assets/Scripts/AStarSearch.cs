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
	public List<Node> _path = new List<Node>();
	public List<Node> _open = new List<Node>();
	public List<Node> _closed = new List<Node>();

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	/// <summary>
	/// Generates the connections for the node. Handles walls
	/// </summary>
	/// <param name="current">Current node looking at</param>
	/// <returns>A list of nodes connecting to it.</returns>
	List<Node> generateConnections(Node current) {
		int x = current.x, y = current.y;
		List<Node> connections = new List<Node>();
		if (!(x-1 < 0)) {
			if (!(this._nodes[x-1,y].type == "wall")){
				connections.Add(this._nodes[x-1,y]);
			}
		}
		if (!(y-1 < 0)) {
			if (!(this._nodes[x,y-1].type == "wall")) {
				connections.Add(this._nodes[x,y-1]);
			}
		}
		if (!(x+1 >= _nodes.GetLength(0))) {
			if (!(this._nodes[x+1,y].type == "wall")) {
				connections.Add(this._nodes[x+1,y]);
			}
		}
		if (!(y+1 >= _nodes.GetLength(1))) {
			if (!(this._nodes[x,y+1].type == "wall")) {
				connections.Add(this._nodes[x,y+1]);
			}
		}
		return connections; 
	}

	/// <summary>
	/// Gets the start node in the game.
	/// </summary>
	/// <returns>Start node</returns>
	Node getStartingNode() {
		return GameObject.Find("Start").GetComponent<Node>();
	}

	/// <summary>
	/// Gets the goal node in the game.
	/// </summary>
	/// <returns>Goal node</returns>
	Node getGoalNode() {
		return GameObject.Find("Goal").GetComponent<Node>();
	}

	/// <summary>
	/// The encapsulation call function for finding the path. Could be put here?
	/// TODO: Put the finding path loop here?
	/// </summary>
	/// <param name="node">Double array to set this map</param>
	public void findPath(Node[,] node) {
		this._nodes = node;
		Heuristic heuristic = new Heuristic(getGoalNode());
		// buildConnections();
		_path = pathLoop(_open, _closed, heuristic);
		if (_path == null) {
			Debug.Log("No path");
		}
	}
	List<Node> pathLoop(List<Node> open, List<Node> closed, Heuristic heuristic) {
		List<Node> foundPath = new List<Node>();
		Node current = null;
		open.Add(getStartingNode());
		while (open.Count > 0) {
			current = open.First(); // Always get the first, it will be the lowest
				//TODO: Get lowest instead?
			open.RemoveAt(0); // Remove the first if we get the first
			List<Node> connections = generateConnections(current); // Builds connections from this node
			foreach (Node successor in connections) { // Look at each connection (none are walls, handled in generateConnections)
				if (successor.type == "goal"){ // Found the goal before the end of open
					successor.parent = current; // set the parent of goal to the one that found first
					open.Clear(); // Clear the rest to exit the while loop (it's found, stop looking)
					break; // exit the foreach
				}
				successor.gn = current.gn + 1; // Updates the steps taken so far
				successor.hn = heuristic.estimate(1, successor); // Calculate the heuristic for successor
				float fn = successor.fn; // Grabs the successors fn
				if (open.Find(delegate (Node n) { // Check if successor in open
						if (n.x == successor.x
							&& n.y == successor.y) {
								if (n.fn > successor.fn) {
									return n;
								} else {
									return successor;
								}
							}
						return false; // None found
					})) {
					continue;
				}
				if (closed.Find(delegate (Node n) { // Check in successor if closed
						if (n.x == successor.x
							&& n.y == successor.y) {
								if (n.fn > successor.fn) {
									return n;
								} else {
									return successor;
								}
							}
						return false; // None found
					})) {
					continue;
				}
				successor.parent = current; // Set to parent if adding the successor
				open.Add(successor); // Add the successor to the list
				open.OrderBy(node=>node.fn).ToList(); // Sort the list for the lowest FN at the top
			}
			closed.Add(current); // Add the current to closed (we've looked at it)
		}
		// Traverse back through the parents
		while (current.type != "start") {
			foundPath.Add(current);
			GameObject.Instantiate(pathObject, new Vector3(current.x, 0, current.y), new Quaternion());
			current = current.parent;
		}
		foundPath.Reverse(); // Start from the beginning
		return foundPath;
	}
}