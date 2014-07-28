using UnityEngine;
using System.Collections;

public class MetaGameController : MonoBehaviour 
{

    public Transform mainCam;
    public float animationSpeed;
    public Vector3 cameraShiftVector;


    private bool cameramovement = false;

    private Vector3 defaultCameraPosition;
    private Vector3 initialCameraPosition;
    private Vector3 targetCameraPosition;

    private float startTime;
    private float distance;

	void Start ()
    {
        defaultCameraPosition = initialCameraPosition = mainCam.position;   
	}
	

	void Update () 
    {
	    if (cameramovement)
	    {

            float distCovered = (Time.time - startTime) * animationSpeed;
            float fraction = distCovered / distance;
            //print("fraction: " + fraction);

            mainCam.position = Vector3.Lerp(initialCameraPosition, targetCameraPosition, fraction);

            if (fraction > 0.999)
            {
                cameramovement = false;
                print("end of animation");
            }
	    }

        
        
	    
	}

    public void MoveCamera(bool forward, Vector3 targetPosition)
    {
        cameramovement = true;

        initialCameraPosition = mainCam.position;

        if (forward)
        {
            print("moving forward");
            targetCameraPosition = targetPosition + cameraShiftVector;
        }

        //return to default position
        else
        {
            print("moving backward");
            targetCameraPosition = defaultCameraPosition;
        }

        startTime = Time.time;
        distance = Vector3.Distance(mainCam.position, targetCameraPosition);
        

    }
}
