using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heuristic {
	public Node goalNode;
	public Heuristic(Node _node) {
		this.goalNode = _node;
	}
	public int estimate(float _travelCost, Node _currentNode) {
		float ex = Mathf.Abs(_currentNode.x - goalNode.x);
		float ey = Mathf.Abs(_currentNode.y - goalNode.y);
		return (int)(_travelCost * (ex + ey));
	}
	// public int estimate(float _travelCost, NodeRecord _currentNode) {
	// 	float ex = Mathf.Abs(_currentNode.node.x - goalNode.x);
	// 	float ey = Mathf.Abs(_currentNode.node.y - goalNode.y);
	// 	return (int)(_travelCost * (ex + ey));
	// }

	void Start() {}

	void Update() {}
}