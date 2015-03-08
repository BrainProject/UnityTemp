using UnityEngine;
using System.Collections;

public class BlobMenu : MonoBehaviour {

    // constants determined from editor according to Tom Pouzar's scene model
    const float X_MAX_COORD = -0.2f;   
    const float X_MIN_COORD = -2.3f;
 
    const float Y_COORD = 0.5f;
    const float Z_COORD = -4.0f;

    const float SCALE_X = 0.2f;
    const float SCALE_Y = 0.2f;
    const float SCALE_Z = 0.2f;

    // attention! Quaternion nor Vector3 cannot be declared const, but should never change!
    static Quaternion ROTATION = Quaternion.Euler(90, 180, 0);
    static Vector3 SCALE = new Vector3(0.02f, 0.02f, 0.02f);

    public Object BlobPrefab;

	// Use this for initialization
    void Start()
    {                       //,new Vector3(X_MIN_COORD,Y_COORD,Z_COORD),ROTATION
        GameObject obj = GameObject.Instantiate(BlobPrefab) as GameObject;
        obj.transform.localScale = SCALE;

        Transform t = gameObject.transform.FindChild("Blobs");
        if(t==null)
        {
            throw new MissingComponentException("Blobs holder missing! Unable to continue!");
        }
        Debug.Log("Child found, name: " + t.name);
        obj.name = "RATATATATATA";
        obj.transform.parent = t;

        obj.transform.rotation = ROTATION;
        obj.transform.localScale = SCALE;
        obj.transform.localPosition = new Vector3(X_MIN_COORD, Y_COORD, Z_COORD);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnMouseOver()
    {
        Debug.Log("Mouse over");
    }
}
