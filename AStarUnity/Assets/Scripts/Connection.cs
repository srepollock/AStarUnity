using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Connection {
	public int cost {get; set;}
	public NodeRecord fromNode {get; set;}
	public NodeRecord toNode {get; set;}

	public Connection setConnection(int cost, NodeRecord from, NodeRecord to) {
		this.cost = cost;
		this.fromNode = from;
		this.toNode = to;
		return this;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
