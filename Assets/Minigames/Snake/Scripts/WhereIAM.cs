﻿using UnityEngine;
using System.Collections;
  //plati pre head

namespace MinigameSnake {
public class WhereIAM : MonoBehaviour {
	Vector3 currentPosition;
	public Vector3 previousPosition;
	int timer = 0;
	// Use this for initialization
	void Start () {
		currentPosition = transform.position;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		print ("changed parent coords");
	//timer - kym sa hlava posunie o 1
		if (timer > 6) {
			//zmena pozice
			previousPosition = currentPosition;
			currentPosition = transform.position;
			timer = 0;
		} else {
			timer ++;
		}
	}
	public Vector3 returnPreviousPosition()
	{
		return previousPosition;
		}
}
}