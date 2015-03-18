using UnityEngine;
using System.Collections;

namespace FindIt_backup
{

    public class ChoosePicturesSetScript : MonoBehaviour
    {

        public string resourcePackName = "Animals";
        public int initial_number_pieces = 8;

        // Use this for initialization
        void OnMouseDown()
        {
            PlayerPrefs.SetString("resourcePackName", resourcePackName);
            PlayerPrefs.SetInt("numberPieces", initial_number_pieces);
            Application.LoadLevel("FindItGame");
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
			Debug.Log ("Resource pack " + resourcePackName + " loaded " + images.Length + " images.");
			return (images.Length >= numberImages);
		}
    }
}