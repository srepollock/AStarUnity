using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A node class used to track the metrics of the path in the game
/// </summary>
public class Node : MonoBehaviour {
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

	void Start() {
		
	}

	void Update() {

	}
}