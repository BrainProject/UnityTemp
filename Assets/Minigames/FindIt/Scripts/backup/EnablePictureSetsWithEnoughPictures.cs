using UnityEngine;
using System.Collections;

namespace FindIt
{
public class EnablePictureSetsWithEnoughPictures : MonoBehaviour 
{
	const int NUMBER_IMAGES_REQUIRED = 20;
	// Use this for initialization
	void Start () 
	{
		GameObject[] choosers = GameObject.FindGameObjectsWithTag("FindItResourcePackChooser");

		int numberImagesRequired = NUMBER_IMAGES_REQUIRED;

		if (PlayerPrefs.HasKey("numberImagesDemanded"))
		{
			numberImagesRequired = PlayerPrefs.GetInt("numberImagesDemanded");
			PlayerPrefs.DeleteKey("numberImagesDemanded");
		}

			Debug.Log ("It is required " + numberImagesRequired + " images.");
		foreach(GameObject chooser in choosers)
		{
				ChoosePicturesSetScript script = chooser.GetComponent<ChoosePicturesSetScript>();
				if(script.checkResourcePackForEnoughImages(numberImagesRequired))
				{
					chooser.GetComponent<BoxCollider2D>().enabled = true;
					chooser.GetComponent<SpriteRenderer>().color = Color.white;
					chooser.GetComponent<FindIt_backup.ChoosePicturesSetScript>().initial_number_pieces = numberImagesRequired;
				}
				else
				{
					chooser.GetComponent<BoxCollider2D>().enabled = false;
					chooser.GetComponent<SpriteRenderer>().color = new Color(0.1f,0.1f,0.1f);
					//chooser.GetComponent<SpriteRenderer>().color.g = 35;
					//chooser.GetComponent<SpriteRenderer>().color.b = 35;
				}
		}
	}
}
}