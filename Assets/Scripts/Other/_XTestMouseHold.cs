/*
 * Created by: Milan Doležal
 */ 

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace MinigameKinectHoldTest {
	public class _XTestMouseHold: MonoBehaviour {
		private Color OriginalColor { get; set; }
		private Vector3 tmp;

		void Start()
		{
			OriginalColor = this.renderer.material.color;
		}
		
		void OnMouseEnter()
		{
			this.renderer.material.color = Color.green;
		}
		
		void OnMouseExit()
		{
			this.renderer.material.color = OriginalColor;
		}
		
		void OnMouseDrag()
		{
			tmp = Camera.main.WorldToScreenPoint (transform.position);
			tmp.x = Input.mousePosition.x;
			tmp.y = Input.mousePosition.y;
			tmp = Camera.main.ScreenToWorldPoint (tmp);
			tmp.z = 0;
			transform.position = tmp;
		}
	}
}