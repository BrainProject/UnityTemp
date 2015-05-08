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
		public bool paused;
		public GameManager manager;
	//public MGC controller;

	public Camera mainCamera;
		public GameObject looserMessage;
		public MoveSnake snake1length;

	// Use this for initialization
	void Start () 
	{
		
		// creates snake
		manager = GameObject.Find ("_Level Manager_").GetComponent<GameManager> ();
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
						//Ctreates grid visible in Snake scene (the right one)
					GameObject.Find("gridCubeHighligted(Clone)").gameObject.name = "gridCell(" + i + ", " + j + ", " + k + ")";
					
				}
			}
		}
			manager = GameObject.Find ("_Level Manager_").GetComponent<GameManager> ();
			manager.game = true;
			manager.score = 0;


		// creates initial food
		level = MGC.Instance.selectedMiniGameDiff;
		int foodNumber = 3 /*+ level / 2*/;
		foodPosition = new Vector3[foodNumber];
		for (int i = 0; i < foodNumber; i++) 
		{
			float px = (float) Random.Range(0,10);
			float py = (float) Random.Range(0,10);
			float pz = (float) Random.Range(0,10);
			foodPosition[i] = new Vector3(px, py, pz);
			//creates food for both screens
			Instantiate(apple, foodPosition[i], Quaternion.identity);
			
			Instantiate(foodColored, foodPosition[i], Quaternion.identity);
			
		}
		// creates poison
		int poisonNumber = level;
		poisonPosition = new Vector3[poisonNumber];
		for (int i = 0; i < poisonNumber; i++) 
		{
			float px = (float) Random.Range(0,10);
			float py = (float) Random.Range(-0.3f,9.7f);
			float pz = (float) Random.Range(0,10);
			poisonPosition[i] = new Vector3(px, py, pz);
			Instantiate(poison1, poisonPosition[i], Quaternion.identity);
			Instantiate(poisonColored, poisonPosition[i], Quaternion.identity);
			
		}	

			looserMessage = GameObject.Find ("loserMessage");
			snake1length = GameObject.Find ("snake1").GetComponent<MoveSnake> ();
			// Sets the game as played
			MGC.Instance.getMinigameStates ().SetPlayed ("Snake", level);
			//pause snake before Play is pressed
			paused = true;
			StartCoroutine("Levels");

	}


	

	IEnumerator Levels () 
	{

			bool inside = true;
			while (inside) {

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
					
					looserMessage.guiText.enabled = true;
					manager.game = false;
					GameObject.Find ("snake1").GetComponent<MoveSnake> ().Stop ();
				
				}
					if (paused) {

						GameObject.Find ("snake1").GetComponent<MoveSnake> ().StopMove ();
						
					}
				}
				yield return null;
			}
		}
	
}
}