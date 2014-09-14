using UnityEngine;
using System.Collections;

namespace FindIt
{
	public class ButtonLoadChooseResourcePackScreen : MonoBehaviour
	{
		public int RequireNumberPictures = 20;

		private Color noActionColor = new Color32(0xFF, 0x77, 0x44, 0xFF);
		//new Color32(0x33, 0x33, 0x33, 0xFF);
		
		private Color pointerOverColor = new Color32(225, 215, 0, 0xFF);
		//new Color32(0xFF, 0x77, 0x44, 0xFF);
		
		
		void Start()
		{
			GetComponent<SpriteRenderer>().color = noActionColor;
			//noActionColor.
		}
		
		void OnMouseEnter()
		{
			GetComponent<SpriteRenderer>().color = pointerOverColor;
		}
		
		void OnMouseExit()
		{
			GetComponent<SpriteRenderer>().color = noActionColor;
		}
		
		void OnMouseDown()
		{
			PlayerPrefs.SetInt("numberImagesDemanded",RequireNumberPictures);
			Application.LoadLevel("FindItChooseImageSet");
		}
	}
}
