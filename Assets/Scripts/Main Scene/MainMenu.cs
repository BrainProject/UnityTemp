using UnityEngine;
using System.Collections;
using System;

namespace MainScene {
	public class MainMenu : MonoBehaviour {

		private string[] mainMenuItemNames = new string[] {"frontalLobe", "parietalLobe",
			"occipitalLobe", "cerebellum", "temporalLobe"};

		private string[] frontalLobeMenuNames = new string[] {"f00", "f01", "f02"};

		private string[] parietalLobeMenuNames = new string[] {"p00", "p01",
			"p02", "p03", "p04", "p05", "p06"};

		private string[] occipitalLobeMenuNames = new string[] {"o00", "o01",
			"o02", "o03"};

		private string[] cerebellumMenuNames = new string[] {"c00", "c01",
			"c02", "c03", "c04", "c05"};

		private string[] temporalLobeMenuNames = new string[] {"t00", "t01"};

		/// <summary>
		/// The game tile prefab.
		/// </summary>
		public GameObject gameTilePrefab;

		/// <summary>
		/// Determines in which menu we currently are.
		/// </summary>
		private string currentMenu = null;

		/// <summary>
		/// Menu tiles.
		/// </summary>
		private GameObject[] gameTiles;

		/// <summary>
		/// Used for mouse click detection
		/// </summary>
		private Ray ray;
		private RaycastHit hit;

		// Use this for initialization
		void Start ()
		{
			gameTiles = GameTiles.createTiles(mainMenuItemNames.Length, gameTilePrefab, "MenuItem");
			for (int i = 0; i < mainMenuItemNames.Length; i++)
			{
				gameTiles[i].name = mainMenuItemNames[i];
				//Debug.Log(mainMenuItemNames[i]);
				//TODO load textures for items here
			}
			/*gameTiles[0].transform.GetChild(0).renderer.material.mainTexture = Resources.Load("insert texture path here") as Texture2D;
			gameTiles[1].transform.GetChild(0).renderer.material.mainTexture = Resources.Load("insert texture path here") as Texture2D;
			gameTiles[2].transform.GetChild(0).renderer.material.mainTexture = Resources.Load("insert texture path here") as Texture2D;
			gameTiles[3].transform.GetChild(0).renderer.material.mainTexture = Resources.Load("insert texture path here") as Texture2D;
			gameTiles[4].transform.GetChild(0).renderer.material.mainTexture = Resources.Load("insert texture path here") as Texture2D;
			*/
		}
		
		// Update is called once per frame
		void Update ()
		{
			ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out hit))
			{
				if (Input.GetMouseButtonUp(0) && hit.collider.tag == "MenuItem" && hit.collider.name != "Empty")
				{
					StartCoroutine(DropOther(hit.collider.gameObject, gameTiles));
				}
			}
		}

		private void SecondMenu(string menuName)
		{
			gameTiles = null;
			gameTiles = GameTiles.createTiles(GetNameArrayFromString(menuName).Length, gameTilePrefab, "MenuItem");
			for (int i = 0; i < GetNameArrayFromString(menuName).Length; i++)
			{
				gameTiles[i].name = GetNameArrayFromString(menuName)[i];
				Debug.Log(GetNameArrayFromString(menuName)[i]);
			}
		}

		private void GameStart(string gameName)
		{
			Debug.Log ("We are selecting the game now");
			//TODO launch selected game by name
		}

		private string[] GetNameArrayFromString(string name)
		{
			if (name == "frontalLobe")
			{
				return frontalLobeMenuNames;
			}
			else if (name == "parietalLobe")
			{
				return parietalLobeMenuNames;
			}
			else if (name == "occipitalLobe")
			{
				return occipitalLobeMenuNames;
			}
			else if (name == "cerebellum")
			{
				return cerebellumMenuNames;
			}
			else //name == "temporalLobe"
			{
				return temporalLobeMenuNames;
			}
		}

		/// <summary>
		/// COROUTINE. Not selected buttons fall down.
		/// </summary>
		/// <param name="chosenButton">Chosen button.</param>
		/// <param name="buttons">Buttons.</param>
		public IEnumerator DropOther(GameObject chosenButton, GameObject[] buttons)
		{
			float t = 0;
			
			for (int i = 0; i < buttons.Length; i++)
			{
				if(chosenButton != buttons[i])
				{
					if(buttons[i] != null)
					{
						buttons[i].GetComponent<Rigidbody>().isKinematic = false;
						buttons[i].GetComponent<Rigidbody>().useGravity = true;
						buttons[i].GetComponent<Rigidbody>().AddForce(chosenButton.transform.position * (-100));
					}
				}
			}
			
			while (t < 1)
			{
				t += Time.deltaTime;
				yield return null;
			}
			StartCoroutine(SelectButton(chosenButton));
		}
		
		/// <summary>
		/// COROUTINE. Selected button flies towards camera. Also starts new game.
		/// </summary>
		/// <param name="chosenButton">Chosen button.</param>
		/// <param name="buttons">Buttons.</param>
		public IEnumerator SelectButton(GameObject chosenButton)
		{
			float t = 0;
			
			Color backTextureColor = chosenButton.transform.GetChild(0).GetComponent<Renderer>().material.color;
			backTextureColor.a = 0;
			chosenButton.transform.GetChild(1).GetComponent<Renderer>().material.color = backTextureColor;
			
			while (t - 1f < 0)
			{
				t += Time.deltaTime * 2;
				
				Color frontTextureColor = chosenButton.transform.GetChild(0).GetComponent<Renderer>().material.color;
				Color backColor = chosenButton.GetComponent<Renderer>().material.color;
				
				frontTextureColor.a = 1f - t;
				backColor.a = 1f - t;
				
				chosenButton.transform.GetChild(0).GetComponent<Renderer>().material.color = frontTextureColor;
				chosenButton.GetComponent<Renderer>().material.color = backColor;
				yield return null;
			}

			currentMenu = chosenButton.name;
			for (int i = 0; i < gameTiles.Length; i++)
			{
				GameObject.Destroy(gameTiles[i]);
			}

			if (Array.IndexOf(mainMenuItemNames, currentMenu) > -1)
			{
				SecondMenu (currentMenu);
			}
			else
			{
				GameStart(currentMenu);
			}

		}
	}
}
