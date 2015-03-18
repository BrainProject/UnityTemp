using UnityEngine;
using System.Collections;

namespace FindIt
{
	
	public class ChoosePicturesSetScript : MonoBehaviour
	{
		
		const int BIG_SIZE = 44;
		const int SMALL_SIZE = 20;

		public string resourcePackName = "Animals";

		// Use this for initialization
		void OnMouseDown()
		{
			PlayerPrefs.SetString("resourcePackName", resourcePackName);

			if(!checkResourcePackForEnoughImages(BIG_SIZE))
			{
				PlayerPrefs.SetInt("numberPieces", SMALL_SIZE);
				//Application.LoadLevel("FindItGame");
				MGC.Instance.sceneLoader.LoadScene("FindItGame",true);
			}
			else 
			{
				//Application.LoadLevel("FindItSize");
				MGC.Instance.sceneLoader.LoadScene("FindItSize",true);
			}
		}
		
		void OnMouseEnter()
		{
			
		}
		
		void OnMouseLeave()
		{
			
		}
		
		public bool checkResourcePackForEnoughImages(int numberImages)
		{
			Sprite[] images = Resources.LoadAll<Sprite>(resourcePackName);
			//Debug.Log ("Resource pack " + resourcePackName + " loaded " + images.Length + " images.");
			return (images.Length >= numberImages);
		}
	}
}