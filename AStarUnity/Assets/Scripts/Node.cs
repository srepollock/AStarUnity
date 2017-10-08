using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A node class used to track the metrics of the path in the game
/// </summary>
public class Node : MonoBehaviour {
	/*
	public float x, y, gn, hn;
	public float fn {get{return this.gn + this.hn;}}
	public float state; //0:open, 1:visit, 2:closed
	public string type;
	public Node(int _x, int _y, string _type) {
		this.x = _x;
		this.y = _y;
		this.state = 0;
		this.type = _type;
	}

	public void setNode(int _x, int _y, string _type) {
		this.x = _x;
		this.y = _y;
		this.state = 0;
		this.type = _type;
	}
	*/
	/// <summary>
	/// Node X position
	/// </summary>
	public int x {get; set;}
	/// <summary>
	/// Node Y position
	/// </summary>
	public int y {get; set;}
	/// <summary>
	/// gn: cost to get to the node
	/// </summary>
	public float gn {get; set;}
	/// <summary>
	/// hn: linear distance to the goal
	/// heuristic estimate!
	/// </summary>
	public float hn {get; set;}
	/// <summary>
	/// fn: heuristic cost
	/// </summary>
	public float fn {get{return this.gn + this.hn;}}
	/// <summary>
	/// Parent connection
	/// </summary>
	public Node parent {get; set;}
	/// <summary>
	/// Type of node (start, goal, wall, ground)
	/// </summary>
	public string type;
	/// <summary>
	/// 0: unvisited; 1: open; 2: closed
	/// </summary>
	public int state {get; set;}

	public Node(int _x, int _y, float _gn, float _hn, string _type) {
		this.x = _x;
		this.y = _y;
		this.gn = _gn;
		this.hn = _hn;
		this.state = 0;
		this.type = _type;
	}

	public void setNode(int _x, int _y, string _type) {
		this.x = _x;
		this.y = _y;
		this.gn = 0;
		this.hn = 0;
		this.state = 0;
		this.parent = null;
		this.type = _type;
	}

	public void setNode(int _x, int _y, float _gn, float _hn, Node _parent, int _state, string _type) {
		this.x = _x;
		this.y = _y;
		this.gn = _gn;
		this.hn = _hn;
		this.state = _state;
		this.parent = _parent;
		this.type = _type;
	}

	public Vector3 getPosition() {
		return new Vector3(this.x, 0, this.y);
	}


	void Start() {
		
	}

	void Update() {

	}
}