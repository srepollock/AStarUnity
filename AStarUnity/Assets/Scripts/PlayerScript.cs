using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour {
	/// <summary>
	/// Path for the user to follow.
	/// </summary>
	public List<Node> _path;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void traversePath() {
		foreach (Node _node in _path) {
			float x = _node.x, y = _node.y;
		}
	}
}
