using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System;

public class MapController : MonoBehaviour {
	public string _mapFileName;
	public char[,] _map;
	public Node[,] _nodes;
	public GameObject _wall, _ground, _start, _goal;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.R)) {
			resetMap();
		}
	}
	/// <summary>
	/// Reades the map passed in and creates the objects.
	/// </summary>
	/// <param name="fileName">Filename to get the map from.</param>
	/// <returns>A node double array that is the map in node objects.</returns>
	public Node[,] readMap (string fileName) {
		int width = getWidth(), height = getHeight();
		_map = new char[width,height];
		Node[,] records = new Node[width,height];
		int col=0, row=0;
		string line;
		// s=start,g=goal,x=wall,' '=blank
		string path = "Assets/Assets/";
        StreamReader reader = new StreamReader(fileName); 
		GameObject child;
        while (!reader.EndOfStream) {
			line = reader.ReadLine();
			col=0;
			child = null;
			foreach(char ch in line) {
				if (ch == 'x' || ch == 'X') {
					child = GameObject.Instantiate(_wall, new Vector3(col, 0, row), new Quaternion());
					child.name = "Wall";
					child.transform.parent = this.transform;
					child.AddComponent<Node>();
					child.GetComponent<Node>().setNode(col, row, "wall");
				}
				else if (ch == 's' || ch == 'S') {
					child = GameObject.Instantiate(_start, new Vector3(col, 0, row), new Quaternion());
					child.name = "Start";
					child.transform.parent = this.transform;
					child.AddComponent<Node>();
					child.GetComponent<Node>().setNode(col, row, "start");
				}
				else if (ch == 'g' || ch == 'G') {
					child = GameObject.Instantiate(_goal, new Vector3(col, 0, row), new Quaternion());
					child.name = "Goal";
					child.transform.parent = this.transform;
					child.AddComponent<Node>();
					child.GetComponent<Node>().setNode(col, row, "goal");
				}
				else {
					child = GameObject.Instantiate(_ground, new Vector3(col, 0, row), new Quaternion());
					child.name = "Ground";
					child.transform.parent = this.transform;
					child.AddComponent<Node>();
					child.GetComponent<Node>().setNode(col, row, "ground");
				}
				records[col,row] = child.GetComponent<Node>();
				_map[col,row] = ch;
				col++;
			}
			row++;
		}
        reader.Close();
		return records;
	}
	/// <summary>
	/// Resets the map and researches
	/// </summary>
	public void resetMap() {
		cleanUp();
		GameObject aStarObj = GameObject.FindWithTag("AStar");
		AStarSearch aStar = aStarObj.GetComponent<AStarSearch>();
		this._nodes = readMap(_mapFileName);
		aStar._map = _map;
		aStar.findPath(this._nodes);
	}
	/// <summary>
	/// Cleans the current map
	/// </summary>
	public void cleanUp() {
		foreach (Transform child in this.transform) {
			GameObject.Destroy(child.gameObject);
		}
	}
	/// <summary>
	/// UI button funciton
	/// </summary>
    public void Apply() {
		String path = EditorUtility.OpenFilePanel("Open a map .txt file", "", "txt");
		String[] array = path.Split('/');
		_mapFileName = array[array.Length-1];
		resetMap();
    }
	/// <summary>
	/// Gets the width of the maze
	/// </summary>
	/// <returns>Width</returns>
	public int getWidth() {
		string line;
		int x = 0;
		StreamReader reader;
		try {
			reader = new StreamReader(_mapFileName);
			using (reader) {
				line = reader.ReadLine();
				if(line != null) {
					foreach(char c in line) {
						x++;
					}
					reader.Close();
				}
			}
			return x;
		} catch(Exception e) {
			Debug.Log(e);
			return -1;
		}
	}
	/// <summary>
	/// Gets the height of the maze
	/// </summary>
	/// <returns>Height</returns>
	public int getHeight() {
		string line;
		int x = 0;
		StreamReader reader;
		try {
			reader = new StreamReader(_mapFileName);
			using (reader) {
				do {
					line = reader.ReadLine();
					if (line != null) {
						x++;
					}
				} while(line != null);
				reader.Close();
			}
			return x;
		} catch(Exception e) {
			Debug.Log(e);
			return -1;
		}
		return x;
	}
}
