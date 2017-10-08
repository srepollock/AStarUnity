using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AStarSearch : MonoBehaviour {
	/// <summary>
	/// Map of characters. TODO: see if needed? (Later)
	/// </summary>
	public char[,] _map;
	/// <summary>
	/// All the nodes on the map.
	/// </summary>
	public GameObject pathObject;
	/// <summary>
	/// List of nodes
	/// </summary>
	Node[,] _nodes {get; set;}
	/// <summary>
	/// Found path in the game.
	/// </summary>
	public List<Node> _path = new List<Node>();
	/// <summary>
	/// Open list of nodes
	/// </summary>
	public List<Node> _open = new List<Node>();
	/// <summary>
	/// Closed list of nodes we've seen
	/// </summary>
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
	/// </summary>
	/// <param name="node">Double array to set this map</param>
	public void findPath(Node[,] node) {
		this._nodes = node;
		Heuristic heuristic = new Heuristic(getGoalNode());
		// buildConnections();
		_path = pathLoop(_open, _closed, heuristic);
		if (_path == null) {
			Debug.Log("No path");
			// TODO: Throw error
		}
		GameObject player = GameObject.Find("Player");
		player.GetComponent<PlayerScript>()._path = this._path; // Set player path to this path
	}
	/// <summary>
	/// Find the path from start to goal in the map.
	/// </summary>
	/// <param name="open">Open list of nodes</param>
	/// <param name="closed">Closed list of nodes</param>
	/// <param name="heuristic">Heuristic for calculating the hn of a node</param>
	/// <returns>A path from start to goal</returns>
	List<Node> pathLoop(List<Node> open, List<Node> closed, Heuristic heuristic) {
		List<Node> foundPath = new List<Node>(); // Initialize the found path
		Node current = null; // Initialize current
		open.Add(getStartingNode()); // Add the starting node to current
		while (open.Count > 0) { // While open not empty, looks
			current = open.First(); // Always get the first, it will be the lowest
			open.RemoveAt(0); // Remove the first if we get the first
			List<Node> connections = generateConnections(current); // Builds connections from this node
			foreach (Node successor in connections) { // Look at each connection (none are walls, handled in generateConnections)
				if (successor.type == "goal"){ // Found the goal before the end of open
					successor.parent = current; // set the parent of goal to the one that found first
					current = successor; // Sets the goal node to the current node, adding to path later
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
			foundPath.Add(current); // Add the current
			GameObject go = GameObject.Instantiate(pathObject, new Vector3(current.x, 0, current.y), new Quaternion()); // Add game object (for visual)
			go.transform.parent = this.transform;
			current = current.parent; // Set the current to the parent
		}
		foundPath.Reverse(); // Changes for Start > Goal list instead (easier to read for the player)
		return foundPath;
	}
	/// <summary>
	/// Cleanup the GameObjects and the lists
	/// </summary>
	public void cleanUp() {
		_map = null;
		_nodes = null;
		_path.Clear();
		_open.Clear();
		_closed.Clear();
		foreach (Transform child in this.transform) {
			GameObject.Destroy(child.gameObject);
		}
	}
}