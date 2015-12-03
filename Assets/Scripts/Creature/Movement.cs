﻿using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {

	Vector2 currentSpeed;
	public float speed;
	public float direction;

	public void ChangeSpeed(Vector2 speed){
		currentSpeed = speed;
		if (speed.y != 0) {
			if (speed.y > 0) {
				direction = 3;
			}
			else {
				direction = 1;
			}
		}
		else if (speed.x != 0) {
			if(speed.x > 0){
				direction = 0;
			}
			else {
				direction = 2;
			}
		}
	}
	void FixedUpdate(){
		transform.Translate (currentSpeed * speed / 15);
	}
}