using UnityEngine;
using System.Collections;

public class RocketLauncher : MonoBehaviour {
	public GameObject Rocket;
	public float RocketDelay;
	public Vector3[] PositionsOfLaunchersRight;
	public Vector3[] PositionsOfLaunchersLeft;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Launch()
	{
		StartCoroutine(LaunchRockets());
	}

	IEnumerator LaunchRockets()
	{
		bool right = true;
		int leftLaunched =0;
		int rightLaunched=0;
		int sum = PositionsOfLaunchersLeft.Length + PositionsOfLaunchersRight.Length;
		for(int i =0;i<sum; i++)
		{
			if(right)
			{
				GameObject.Instantiate(Rocket,PositionsOfLaunchersRight[rightLaunched]+transform.position,Quaternion.AngleAxis(10,Vector3.forward));
				rightLaunched++;
				right = false;
			}
			else
			{
				GameObject.Instantiate(Rocket,PositionsOfLaunchersLeft[leftLaunched]+transform.position,Quaternion.AngleAxis(-10,Vector3.forward));
				leftLaunched++;
				right = true;
			}
			yield return new WaitForSeconds(RocketDelay);
		}
	}

	void OnDrawGizmos()
	{
		Gizmos.color=Color.red;
		//Gizmos.DrawFrustum(transform.parent.position + transform.position,180,1,2,3);
		foreach (var launcher in PositionsOfLaunchersLeft) {
			Gizmos.DrawWireCube(launcher+transform.position,new Vector3(0.1f,0.1f,0.1f));
		}
		foreach (var launcher in PositionsOfLaunchersRight) {
			Gizmos.DrawWireCube(launcher+transform.position,new Vector3(0.1f,0.1f,0.1f));
		}
	}
}
