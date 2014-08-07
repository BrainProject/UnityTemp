using UnityEngine;
using System;
using System.Collections;

namespace MinigamePexeso {
	public class GameStart : MonoBehaviour {

	    public GameObject main;
	    public GameObject resourcePackMenu;

	    private GameObject[] buttonPlanes;
	    private GameObject[] buttonCubes;

	    /// <summary>
	    /// Number of rows of menu items.
	    /// </summary>
	    private int menuRows = 2;
	    /// <summary>
	    /// Number of columns of menu items.
	    /// </summary>
	    private int menuColumns = 2;

		// Use this for initialization
		void Start ()
	    {
		}

	    public void CreateMenu()
	    {
	        resourcePackMenu.SetActive(false);

	        buttonPlanes = new GameObject[menuRows * menuColumns];
	        buttonCubes = new GameObject[menuRows * menuColumns];
	        for (int i = 0; i < menuColumns; i++)
	        {
	            for (int o = 0; o < menuRows; o++)
	            {
	                buttonCubes[menuRows*i + o] = GameObject.CreatePrimitive(PrimitiveType.Cube);//create cube
	                buttonCubes[menuRows*i + o].transform.localScale = new Vector3(1, 1, 0.1f);//flatten it
	                buttonCubes[menuRows*i + o].transform.position = new Vector3((i * 1.2f) - (0.1125f*(float)Math.Pow(menuColumns, 2)),(o * 1.2f) - (0.05625f*(float)Math.Pow(menuRows, 2)),-1);//move them
	                buttonCubes[menuRows*i + o].renderer.material.mainTexture = Resources.Load("Textures/back") as Texture2D;//load texture
	                
	                buttonPlanes[menuRows*i + o] = GameObject.CreatePrimitive(PrimitiveType.Plane);//create plane
	                buttonPlanes[menuRows*i + o].transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);//shrink them
	                Quaternion q = new Quaternion(0, 0, 0, 1);//create rotation quaternion
	                q.SetLookRotation(new Vector3(0,-1000,-1), new Vector3(0,1,0));//assign values to quaternion
	                buttonPlanes[menuRows*i + o].transform.rotation = q;//assign quaternion to object
	                buttonPlanes[menuRows*i + o].transform.position = new Vector3((i * 1.2f) - (0.1125f*(float)Math.Pow(menuColumns, 2)),(o * 1.2f) - (0.05625f*(float)Math.Pow(menuRows, 2)),-1.051f);//move them
	                
	                buttonCubes[menuRows*i + o].transform.parent = buttonPlanes[menuRows*i + o].transform;//make plane parent of cube
	                
	                buttonPlanes[menuRows*i + o].renderer.material.shader = Shader.Find("Particles/Alpha Blended");//set shader
	                buttonCubes[menuRows*i + o].renderer.material.shader = Shader.Find("Particles/Alpha Blended");//set shader

	                Destroy(buttonPlanes[menuRows*i + o].collider);
	                buttonPlanes[menuRows*i + o].AddComponent<BoxCollider>();
	                Rigidbody gameObjectsRigidBody = buttonPlanes[menuRows*i + o].AddComponent<Rigidbody>(); // Add the rigidbody.
	                gameObjectsRigidBody.mass = 5;//set weight
	                gameObjectsRigidBody.useGravity = false;//disable gravity
	                
	                buttonPlanes[menuRows*i + o].tag = "MenuItem";
	            }
	        }
	        buttonPlanes [0].name = "2x2";
	        buttonPlanes [0].renderer.material.mainTexture = Resources.Load("Textures/Menu/2x2") as Texture2D;
	        buttonPlanes [1].name = "2x3";
	        buttonPlanes [1].renderer.material.mainTexture = Resources.Load("Textures/Menu/2x3") as Texture2D;
	        buttonPlanes [2].name = "3x4";
	        buttonPlanes [2].renderer.material.mainTexture = Resources.Load("Textures/Menu/3x4") as Texture2D;
	        buttonPlanes [3].name = "4x4";
	        buttonPlanes [3].renderer.material.mainTexture = Resources.Load("Textures/Menu/4x4") as Texture2D;
	    }
		
	    private Ray ray;
	    private RaycastHit hit;

		// Update is called once per frame
		void Update ()
	    {
	        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
	        if (Physics.Raycast(ray, out hit))
	        {
	            if (Input.GetMouseButtonUp(0) && hit.collider.tag == "MenuItem")
	            {
	                StartCoroutine(DropOther(hit.collider.gameObject, buttonPlanes));
	            }
	        }
		}

	    /// <summary>
	    /// COROUTINE. Not selected buttons fall down.
	    /// </summary>
	    /// <returns>The other.</returns>
	    /// <param name="chosenButton">Chosen button.</param>
	    /// <param name="buttons">Buttons.</param>
	    private IEnumerator DropOther(GameObject chosenButton, GameObject[] buttons)
	    {
	        float t = 0;

	        for (int i = 0; i < buttons.Length; i++)
	        {
	            if(chosenButton != buttons[i])
	            {
	                buttons[i].rigidbody.useGravity = true;
	                buttons[i].rigidbody.AddForce(chosenButton.transform.position * (-100));
	            }
	        }

	        while (t < 1)
	        {
	            t += Time.deltaTime;
	            yield return null;
	        }
	        StartCoroutine(SelectButton(chosenButton));
	        yield return 0;
	    }

	    /// <summary>
	    /// COROUTINE. Selected button flies towards camera. Also starts new game.
	    /// </summary>
	    /// <returns>The other.</returns>
	    /// <param name="chosenButton">Chosen button.</param>
	    /// <param name="buttons">Buttons.</param>
	    private IEnumerator SelectButton(GameObject chosenButton)
	    {

	        Vector3 startPosition = chosenButton.transform.position;
	        float t = 0;
	        
	        Vector3 endPosition = new Vector3(Camera.main.ViewportToWorldPoint(Vector3.zero).x,
	                                  Camera.main.ViewportToWorldPoint(Vector3.zero).y,
	                                  Camera.main.ViewportToWorldPoint(Vector3.zero).z);

	        while (t - 0.9f < 0)
	        {
	            t += Time.deltaTime * 2;
	            chosenButton.transform.position = Vector3.Lerp(startPosition, endPosition, t);
	            yield return null;
	        }
	        CreateMainGameObject(chosenButton);
	        chosenButton.transform.position = endPosition;

	        yield return 0;
	    }

	    /// <summary>
	    /// Creates new game by setting game dimensions and starting main game script.
	    /// </summary>
	    /// <param name="chosenButton">Chosen button.</param>
	    private void CreateMainGameObject(GameObject chosenButton)
	    {
	        string[] dimensions = chosenButton.name.Split('x');
	        int rows = Int32.Parse(dimensions [0]);
	        int columns = Int32.Parse(dimensions [1]);

	        for (int i = 0; i < menuColumns*menuRows; i++)
	        {
	            GameObject.Destroy(buttonCubes[i]);
	            GameObject.Destroy(buttonPlanes[i]);
	        }

	        if (main != null)
	        {
	            GameScript mainGameScript = main.GetComponent("GameScript") as GameScript;
	            mainGameScript.rows = rows;
	            mainGameScript.columns = columns;
	            mainGameScript.enabled = true;
	            mainGameScript.CreateGameBoard();

	            AudioSource musicPlayer = GameObject.Find("MusicPlayer").GetComponent("AudioSource") as AudioSource;
	            if(musicPlayer == null)
	            {
	                Debug.Log("ERROR");
	            }
	            musicPlayer.Play();
	        }
	    }
	}
}