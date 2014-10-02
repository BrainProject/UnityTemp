using UnityEngine;
using System.Collections;

namespace FindIt
{
	
	public class ChooseNumberImagesScript : MonoBehaviour
	{
		public int numberPictures = 20;

		private Color noActionColor = new Color32(0xFF, 0x77, 0x44, 0xFF);

		private Color pointerOverColor = new Color32(225, 215, 0, 0xFF);

		// Use this for initialization
		void OnMouseDown()
		{
			PlayerPrefs.SetInt("numberPieces", numberPictures);
			Application.LoadLevel("FindItGame");
		}
		
		void Start()
		{
			GetComponent<SpriteRenderer>().color = noActionColor;
		}
		
		void OnMouseEnter()
		{
			GetComponent<SpriteRenderer>().color = pointerOverColor;
		}
		
		void OnMouseExit()
		{
			GetComponent<SpriteRenderer>().color = noActionColor;
		}
	}
}