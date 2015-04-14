using UnityEngine;
using System.Collections;


namespace MinigameSnake
{
public class ChangeRightTexture : MonoBehaviour {
	public Material samePlane;
	public Material back;
	public Material front;
	GameObject snake;


	// Use this for initialization
	void Start () {
		snake = GameObject.FindGameObjectWithTag ("Snake");
		//	print (snake);
		if (snake.transform.position.z == this.transform.position.z) {
			this.renderer.material = samePlane;
				}
		if (snake.transform.position.z > this.transform.position.z) {
			this.renderer.material = back;
		}
		if (snake.transform.position.z < this.transform.position.z) {
			this.renderer.material = front;
		}
	
	}
	
	// Update is called once per frame
	void Update () {
		snake = GameObject.FindGameObjectWithTag ("Snake");
			if (snake != null) {
				if (snake.transform.position.z == this.transform.position.z) {
					this.renderer.material = samePlane;
				}
				if (snake.transform.position.z > this.transform.position.z) {
					this.renderer.material = back;
				}
				if (snake.transform.position.z < this.transform.position.z) {
					this.renderer.material = front;
				}
			}
	}
}
}
