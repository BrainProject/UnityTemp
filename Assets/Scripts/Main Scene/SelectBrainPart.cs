using UnityEngine;
using System.Collections;

public class SelectBrainPart : MonoBehaviour {
	public string descriptionText;
	public string iconName;
	public currentBrainPartEnum brainPartToLoad;
	public bool CanRotate{ get; set; }

	private string levelName;
	private Color selectionColor;
	private Color originalColor;
	private GUITexture icon { get; set; }
	private GameObject Description{ get; set; }

	void Start()
	{
		CanRotate = false;
		icon = GameObject.Find ("Brain Part Icon").guiTexture;
		Description = GameObject.Find ("Description");
		originalColor = this.renderer.material.color;
		levelName = "MirkaSelection";
	}


	void OnMouseEnter()
	{
		if(CanRotate)
		{
			Texture2D tmp = (Texture2D)Resources.Load ("Main/" + iconName, typeof(Texture2D));
			if(tmp != null)
				icon.texture = tmp;
			this.transform.localScale = new Vector3(this.transform.localScale.x + 0.1f, this.transform.localScale.y + 0.1f, this.transform.localScale.z + 0.1f);
			Description.GetComponent<TextMesh> ().text = descriptionText;
			this.renderer.material.color = new Color(originalColor.r + 0.4f, originalColor.g + 0.4f, originalColor.b + 0.4f);
		}
	}

	void OnMouseExit()
	{
		if(CanRotate)
		{
			icon.texture = null;
			this.transform.localScale = new Vector3(this.transform.localScale.x - 0.1f, this.transform.localScale.y - 0.1f, this.transform.localScale.z - 0.1f);
			Description.GetComponent<TextMesh> ().text = "";
		}
		this.renderer.material.color = originalColor;
	}

	void OnMouseOver()
	{
		if(CanRotate)
		{
			if(Input.GetButtonDown ("Fire1"))
			{
				switch(descriptionText)
				{
				case "Frontal Lobe": GameObject.Find("_GameManager").GetComponent<GameManager>().currentCameraDefaultPosition = new Vector3(0,0,0);
					break;
				case "Pariental Lobe": GameObject.Find("_GameManager").GetComponent<GameManager>().currentCameraDefaultPosition = new Vector3(0,0,0);
					break;
				case "Occipital Lobe": GameObject.Find("_GameManager").GetComponent<GameManager>().currentCameraDefaultPosition = new Vector3(0,0,0);
					break;
				}
				GameObject.Find("_GameManager").GetComponent<GameManager>().selectedBrainPart = brainPartToLoad;
				StartCoroutine(GameObject.Find("LoadLevelWithFade").GetComponent<LoadLevelWithFade>().LoadSeledctedLevelWithColorLerp(false, levelName));
			}
		}
	}

//	void Fade(Color currentColor, Color nextColor)
//	{
//		duration += Time.deltaTime;
//		this.renderer.material.color = Color.Lerp(currentColor, nextColor, duration);
//	}
}
