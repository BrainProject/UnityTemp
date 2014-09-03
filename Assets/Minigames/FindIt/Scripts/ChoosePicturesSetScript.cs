using UnityEngine;
using System.Collections;

namespace FindIt
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
            Application.LoadLevel("GameScene");
        }

        void OnMouseEnter()
        {

        }

        void OnMouseLeave()
        {

        }
    }
}