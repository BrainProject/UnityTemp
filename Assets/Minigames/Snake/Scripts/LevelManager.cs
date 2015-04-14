using UnityEngine;
using System.Collections;


namespace MinigameSnake
{
public class LevelManager : MonoBehaviour 
{

	public GameObject snakehead;
	public GameObject snakeprefab;
	public GameObject gridCube;
	public GameObject gridCubeRight;

	public int level;

	public GameObject apple;
	public GameObject poison1;
	public GameObject foodColored;
	public GameObject poisonColored;
	public Vector3[] foodPosition;
	public Vector3[] poisonPosition;
 	private GameObject snake1;

		public GameManager manager;
	//public MGC controller;

	public Camera mainCamera;
		public GameObject looserMessage;
		public Move2 snake1length;

	// Use this for initialization
	void Start () 
	{
		//MGC.Instance.startMiniGame("Snake");

		Screen.showCursor = false;
		/*	MGC.Instance.minigamesGUI.transform.position.x = 4;
			MGC.Instance.minigamesGUI.transform.position.y = 2;
			MGC.Instance.minigamesGUI.transform.position.y = 12;
			MGC.Instance.minigamesGUI.transform.rotation.x = 0;
			MGC.Instance.minigamesGUI.transform.rotation.y = 180;
			MGC.Instance.minigamesGUI.transform.rotation.z = 0;*/

		// creates snake
		manager = GameObject.Find ("_GameManager_").GetComponent<GameManager> ();
		manager.game = true;
		manager.score = 0;
		snake1 = (GameObject)Instantiate (snakehead, new Vector3(4, 4, 2), Quaternion.identity);
		snake1.name = "snake1";
		GameObject snake2 = (GameObject)Instantiate (snakeprefab, new Vector3(3, 4, 2), Quaternion.identity);
		snake2.name = "snake2";
		GameObject snake3 = (GameObject)Instantiate (snakeprefab, new Vector3(2, 4, 2), Quaternion.identity);
		snake3.name = "snake3";

		// creates the grid
		for (int i = 0; i < 10; i++) 
		{
			for (int j = 0; j < 10; j++) 
			{
				for (int k = 0; k < 10; k++) 
				{
					Instantiate(gridCube, new Vector3(i, j, k), Quaternion.identity);
					if (i==9)
					{
						Instantiate(gridCubeRight, new Vector3(i+2, j, k), Quaternion.identity);
						//print("created back plne");
					}
					//GameObject.Find("gridCube(Clone)").gameObject.transform.parent = GameObject.Find("_LevelManager_").transform;
					GameObject.Find("gridCube(Clone)").gameObject.name = "gridCell(" + i + ", " + j + ", " + k + ")";
					
				}
			}
		}
			manager = GameObject.Find ("_GameManager_").GetComponent<GameManager> ();
			manager.game = true;
			manager.score = 0;


		// creates initial food
		//level = GameObject.Find ("_GameManager_").GetComponent<GameManager> ().currentLevel;
		level = MGC.Instance.selectedMiniGameDiff;
		int foodNumber = 3 /*+ level / 2*/;
		foodPosition = new Vector3[foodNumber];
		for (int i = 0; i < foodNumber; i++) 
		{
//			print(i);
			float px = (float) Random.Range(0,10);
			float py = (float) Random.Range(0,10);
			float pz = (float) Random.Range(0,10);
			foodPosition[i] = new Vector3(px, py, pz);
			//creates food for both screens
			Instantiate(apple, foodPosition[i], Quaternion.identity);
			//print("created normal food");
			Instantiate(foodColored, foodPosition[i], Quaternion.identity);
			//print("created right food");
			//print("food created");
		}
		// creates poison
		int poisonNumber = level;
		poisonPosition = new Vector3[poisonNumber];
		for (int i = 0; i < poisonNumber; i++) 
		{
			float px = (float) Random.Range(0,10);
			float py = (float) Random.Range(0,10);
			float pz = (float) Random.Range(0,10);
			poisonPosition[i] = new Vector3(px, py, pz);
			Instantiate(poison1, poisonPosition[i], Quaternion.identity);
			Instantiate(poisonColored, poisonPosition[i], Quaternion.identity);
			//print("poison created");
		}	

			looserMessage = GameObject.Find ("loserMessage");
			snake1length = GameObject.Find ("snake1").GetComponent<Move2> ();

			MGC.Instance.getMinigameStates ().SetPlayed ("Snake", level);

			StartCoroutine("Levels");
	}


	void Update () 
		{

		}

	IEnumerator Levels () 
	{
			bool inside = true;
			while (inside) {
				//	print("started update");
				GameObject[] snakeBody;
			//	bool inside = false;
				//print(GameObject.Find ("snake1"));
				int snakeLength = snake1length.snakeLength;
				if (snake1 != null)
				{
				for (int i = 1; i <= snakeLength; i++) {
					// check whether is snake out of gameplane
					inside = GeometryUtility.TestPlanesAABB (GeometryUtility.CalculateFrustumPlanes (mainCamera), GameObject.Find ("snake" + i).GetComponent<BoxCollider> ().bounds);
					if (GameObject.Find ("snake" + i).GetComponent<BoxCollider> ().bounds.center.z < -20) {
						inside = false;
					}
					if (inside) {
						break;
					}
					
				}
				if (!inside) {
					//GameObject.Find ("_GameManager_").GetComponent<GameManager> ().game = false;
					looserMessage.guiText.enabled = true;
					manager.game = false;
					GameObject.Find ("snake1").GetComponent<Move2> ().Stop ();
				
				}
				}
				yield return null;
			}
		}
	
}
}