using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerScript : MonoBehaviour {
	/// <summary>
	/// Path for the user to follow.
	/// </summary>
	public List<Node> _path {get; set;}
	/// <summary>
	/// Target waypoint for movement
	/// </summary>
	Node _target;
	/// <summary>
	/// Counter for index in the path
	/// </summary>
	int pathPositionCounter = 0;
	/// <summary>
	/// Speed to move by
	/// Speed set in map controller
	/// </summary>
	public float speed;
	/// <summary>
	/// Move or not move
	/// </summary>
	public bool move = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.S)) {
			move = !move;
		}
		if (move) {
			this.walk(this._path);
		}
	}
	/// <summary>
	/// Walk the found path.
	/// </summary>
	/// <param name="path">Path to walk</param>
	public void walk(List<Node> path) {
		if (pathPositionCounter >= path.Count) {
			move = false;
		}
		if (!move && pathPositionCounter > path.Count-1) {
			pathPositionCounter = path.Count-1;
		}
		_target = path[pathPositionCounter];
		// rotate towards the _target
		this.transform.forward = Vector3.RotateTowards(this.transform.forward, _target.getPosition() - this.transform.position, speed*Time.deltaTime, 0.0f);

		// move towards the _target
		this.transform.position = Vector3.MoveTowards(this.transform.position, _target.getPosition(), speed*Time.deltaTime);

		if(this.transform.position == _target.getPosition()) {
			pathPositionCounter++;
		}
	}
}
